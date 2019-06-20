
insert into tbl_taxa_tree_orders(order_id, date_updated, order_name, record_type_id, sort_order)
	values (139, '2015-05-22', 'Sphagnales', 2, NULL);
    
insert into tbl_taxa_tree_families(family_id, date_updated, family_name, order_id)
	values (1980, '2015-05-22', 'Sphagnaceae', 139)
    
insert into tbl_taxa_tree_genera(genus_id, date_updated, family_id, genus_name)
	values (15468, '2015-05-22', 1980, 'Sphagnum');

select *
from tbl_taxa_tree_families
where family_id = 1980

create or replace function sead_utility.sync_sequences() returns void language plpgsql as $$
declare
    v_template varchar;
    v_sql text;
	r.row record;
begin
	for r_row in
        select table_schema, table_name, column_name, pg_get_serial_sequence(table_name, column_name) as sequence_name
        from information_schema.columns
        where table_schema = 'public'
          and ordinal_position > 0
          and column_default like 'nextval(%'
  
    loop
        v_template = 'select setval(%s, max(%s)) from %s;'
        v_sql = format(v_template)
        select 'select setval(' || quote_literal(quote_ident(pgt.schemaname) || '.'|| quote_ident(s.relname)) ||
                ', max(' || quote_ident(c.attname) || ') ) from ' || quote_ident(pgt.schemaname) || '.' || quote_ident(t.relname) || ';' as fix_query
		execute sql.fix_query;
	end loop;
end;
$$;
    v_template = 'select setval(%s, max(%s)) from %s;'

    select -- setval(
        quote_literal(pg_get_serial_sequence(table_name, column_name))
        --table_schema,
        --table_name, column_name, pg_get_serial_sequence(table_name, column_name) as sequence_name
    from information_schema.columns
    where table_schema = 'public'
      and ordinal_position > 0
      and column_default like 'nextval(%'
  

        
select 'select setval(' || quote_literal(quote_ident(pgt.schemaname) || '.'|| quote_ident(s.relname)) ||
        ', max(' || quote_ident(c.attname) || ') ) from ' || quote_ident(pgt.schemaname) || '.' || quote_ident(t.relname) || ';' as fix_query
from pg_class as s
join pg_depend as d
  on s.oid = d.objid
join pg_class as t
  on d.refobjid = t.oid
join pg_attribute as c
  on d.refobjid = c.attrelid
 and d.refobjsubid = c.attnum
join pg_tables as pgt
  on t.relname = pgt.tablename
where s.relkind = 's'
order by s.relname
        
select pg_get_serial_sequence('tbl_biblio', 'biblio_id')

SELECT setval(pg_get_serial_sequence('tbl', 'tbl_id'), max(tbl_id)) FROM tbl;


  
  
      
    