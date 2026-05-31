-- COLUMN IN 
-- ALTER TABLE public.tbl_sites ADD COLUMN site_location_accuracy character varying;
  
WITH t_prod as (select site_id as id, * from public_production.tbl_sites),
     t_old8 as (select site_id as id, * from public.tbl_sites)
    SELECT coalesce(t1.id, t2.id) as id,
           case 
             when t1.id is null then 'NEW'
             when t2.id is null then 'MISSING'
             else 'DIFF'
           end as status, -- to_json(t1),
           *
    FROM t_old8 t1
    FULL OUTER JOIN t_prod t2 using (id)
    WHERE (t1 IS DISTINCT FROM t2)
       OR (t1.id IS null)
       OR (t2.id IS null);


/*        
with table_p as (select site_id as id, * from public_production.tbl_sites),
     table_8 as (select site_id as id, * from public.tbl_sites)
    select id as id, key as column, val[1] as value_1, val[2] as value_2
    from (
        select id, key, array_agg(value order by t) val
        from (
            select t, id, key, value
            from (
                (select 1 t, * from table_8 except select 1 t, * from table_p)
                union all
                (select 2 t, * from table_p except select 2 t, * from table_8)
            ) s,
            lateral json_each_text(to_json(s))
            group by 1, 2, 3, 4
        ) s
        group by 1, 2
    ) s
    where key <> 't'
      and coalesce(val[1],'') <> coalesce(val[2], '')
    order by 1;
*/

CREATE SCHEMA diff_schema;
DO $$
DECLARE
  v_tablename varchar;
  v_sql varchar;
BEGIN
    FOR v_tablename IN
        select table_name
        from INFORMATION_SCHEMA.tables
        where table_schema = 'public'
          and table_type = 'BASE TABLE'
          and table_name like 'tbl_%'
    LOOP
        v_sql = format('
            DROP VIEW IF EXISTS data_diffs.%s;
                       
                       ')
        -- EXECUTE 'DROP TABLE ' || v_tablename || ' CASCADE;';
        raise notice 'Dropped: %', v_tablename;
    END LOOP;
END $$ LANGUAGE plpgsql;

select * from v
