drop view if exists view_foreign_keys;
drop function if exists fn_script_to_sqlite_columns(BOOLEAN);
drop function if exists fn_script_to_sqlite_table(p_source_schema character varying, p_table_name character varying, BOOLEAN);
drop function if exists fn_script_to_sqlite_tables();

create or replace view view_foreign_keys
 as
 with table_columns as (
        select t_1.oid,
        ns.nspname,
        t_1.relname,
        attr.attname,
        attr.attnum
        from pg_class t_1
            join pg_namespace ns on ns.oid = t_1.relnamespace
            join pg_attribute attr on attr.attrelid = t_1.oid and attr.attnum > 0
    )
  select distinct t.nspname as schema_name,
    t.oid as table_oid,
    t.relname as table_name,
    t.attname as column_name,
    t.attnum,
    s.nspname as f_schema_name,
    s.relname as f_table_name,
    s.attname as f_column_name,
    s.oid as f_table_oid,
    t.attnum as f_attnum
  from pg_constraint
     join table_columns t on t.oid = pg_constraint.conrelid and t.attnum = pg_constraint.conkey[1] and (t.attnum = any (pg_constraint.conkey))
     join table_columns s on s.oid = pg_constraint.confrelid and (s.attnum = any (pg_constraint.confkey))
  where pg_constraint.contype = 'f'::"char";


create or replace function fn_script_to_sqlite_columns() returns table(
    table_name information_schema.sql_identifier,
    column_name information_schema.sql_identifier,
    ordinal_position information_schema.cardinal_number,
    data_type text, is_nullable information_schema.yes_or_no,
    is_pk information_schema.yes_or_no,
    is_fk information_schema.yes_or_no,
    fk_table_name information_schema.sql_identifier,
    fk_column_name information_schema.sql_identifier
) language 'plpgsql' as $BODY$
begin
    return query
        select
            pg_tables.tablename::information_schema.sql_identifier  as table_name,
            pg_attribute.attname::information_schema.sql_identifier as column_name,
            pg_attribute.attnum::information_schema.cardinal_number as ordinal_position,
            case
            when pg_attribute.atttypid in (16, 20, 21, 23) then 'INTEGER'
            when pg_attribute.atttypid in (18, 19, 25, 1002, 1043, 1082, 1114, 1184, 12790, 12797) then 'TEXT'
            when pg_attribute.atttypid in (700, 1700) then 'NUMERIC'
            when pg_attribute.atttypid in (17, 1005, 1009, 1021) then 'BLOB'
            else null end::text,
            case pg_attribute.attnotnull when false then 'YES' else 'NO' end::information_schema.yes_or_no as is_nullable,
            case when pk.contype is null then 'NO' else 'YES' end::information_schema.yes_or_no as is_pk,
            case when fk.table_oid is null then 'NO' else 'YES' end::information_schema.yes_or_no as is_fk,
            fk.f_table_name::information_schema.sql_identifier,
            fk.f_column_name::information_schema.sql_identifier
    from pg_tables
    join pg_class
        on pg_class.relname = pg_tables.tablename
    join pg_namespace ns
        on ns.oid = pg_class.relnamespace
        and ns.nspname  = pg_tables.schemaname
    join pg_attribute
        on pg_class.oid = pg_attribute.attrelid
        and pg_attribute.attnum > 0
    left join pg_constraint pk
        on pk.contype = 'p'::"char"
        and pk.conrelid = pg_class.oid
        and (pg_attribute.attnum = any (pk.conkey))
    left join view_foreign_keys as fk
        on fk.table_oid = pg_class.oid
        and fk.attnum = pg_attribute.attnum
    where true
        and pg_tables.tableowner = 'sead_master'
        and pg_attribute.atttypid <> 0::oid
        and pg_tables.schemaname = 'public'
    order by table_name, ordinal_position asc;
end
$BODY$;


create or replace function fn_script_to_sqlite_table(p_source_schema character varying, p_table_name character varying, p_generate_fks BOOLEAN=TRUE)
returns text language 'plpgsql' as $BODY$
	declare sql_stmt text = '';
	declare data_columns text = null;
	declare foreign_key_columns text = '';
begin

    select string_agg(
		format('%s %s%s%s', column_name, data_type,
			case when is_nullable = 'NO' then ' NOT NULL' else '' end,
			case when is_pk = 'YES' then ' PRIMARY KEY' else '' end
		), E',\n ' order by ordinal_position asc),
		string_agg(
			case when is_fk = 'NO' then ''
				 else format(E' , FOREIGN KEY (%s) REFERENCES %s (%s)\n',
							column_name, fk_table_name, fk_column_name) end
			, '')

    into strict data_columns, foreign_key_columns
    from fn_script_to_sqlite_columns() s
    where table_name = p_table_name;

    if data_columns is not null then
	sql_stmt = format('
CREATE TABLE %s (
 %s
%s
);', p_table_name, data_columns, case when p_generate_fks = TRUE then foreign_key_columns else '' end);
	end if;
	return sql_stmt;
end $BODY$;


create or replace function fn_script_to_sqlite_tables(p_generate_fks BOOLEAN=TRUE)
	returns varchar language 'plpgsql' as $body$
declare x record;
	declare sql_tables text = '';
	declare sql_table text = '';
begin
	For x In (
		with recursive fk_tree as (
		  select t.oid as reloid,
				 t.relname as table_name,
				 s.nspname as schema_name,
				 null::name as referenced_table_name,
				 null::name as referenced_schema_name,
				 1 as level
		  from pg_class t
			join pg_namespace s on s.oid = t.relnamespace
		  where relkind = 'r'
			and not exists (select * from pg_constraint where contype = 'f' and conrelid = t.oid)
			and s.nspname = 'public' -- limit to one schema
		  union all
		  select ref.oid, ref.relname, rs.nspname, p.table_name, p.schema_name, p.level + 1
		  from pg_class ref
		   join pg_namespace rs on rs.oid = ref.relnamespace
		   join pg_constraint c on c.contype = 'f' and c.conrelid = ref.oid
		   join fk_tree p on p.reloid = c.confrelid
		  where ref.oid != p.reloid  -- do not enter to tables referencing theirselves.
		), all_tables as (
		  select schema_name, table_name, level,
				 row_number() over (partition by schema_name, table_name order by level desc) as last_table_row
		  from fk_tree
		)
		select schema_name, table_name
		from all_tables at
		where last_table_row = 1
		order by level
	)
	loop
		sql_table = fn_script_to_sqlite_table(x.schema_name::character varying, x.table_name::character varying, p_generate_fks);
        -- raise notice '%', sql_table;
		sql_tables = sql_tables || e'\n' || sql_table;
	end loop;

    if p_generate_fks = TRUE then
        sql_tables = sql_tables || e'\nPRAGMA foreign_keys = ON;\n\n';
    end if;

    sql_tables = e'\n' || sql_tables || e'\n';

	return sql_tables;

end
$body$;

select fn_script_to_sqlite_tables(FALSE);
