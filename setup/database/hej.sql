
insert into tbl_taxa_tree_orders(order_id, date_updated, order_name, record_type_id, sort_order)
    values (139, '2015-05-22', 'Sphagnales', 2, NULL);

insert into tbl_taxa_tree_families(family_id, date_updated, family_name, order_id)
    values (1980, '2015-05-22', 'Sphagnaceae', 139)

insert into tbl_taxa_tree_genera(genus_id, date_updated, family_id, genus_name)
    values (15468, '2015-05-22', 1980, 'Sphagnum');



with public_foreign_keys as (
    select table_name          as source_name,
           foreign_table_name  as target_name,
           foreign_column_name as column_name
    from sead_utility.foreign_key_columns
    where table_schema = 'public'
), public_relations as (
    select source_name, target_name, column_name
    from public_foreign_keys
    union all
    select target_name, source_name, column_name
    from public_foreign_keys
), facet_undirected_relations as (
    select t1.table_or_udf_name as source_name,
           t2.table_or_udf_name as target_name,
           r.target_column_name as column_name
    from facet.table_relation r
    join facet.table t1 on t1.table_id = r.source_table_id
    join facet.table t2 on t2.table_id = r.target_table_id
    where r.source_column_name = r.target_column_name
), facet_relations as (
    select source_name, target_name, column_name
    from facet_undirected_relations
    union all
    select target_name, source_name, column_name
    from facet_undirected_relations
), consolidated_relations as (
    select coalesce(p.source_name, f.source_name) as source_name,
           coalesce(p.target_name, f.target_name) as target_name,
           coalesce(p.column_name, f.column_name) as column_name,
           case when p.source_name is null then '' else 'YES' end as is_public,
           case when f.source_name is null then '' else 'YES' end as is_facet
    from public_relations p
    full outer join facet_relations f
      on f.source_name = p.source_name
     and f.target_name = p.target_name
) select *
  from consolidated_relations
  where is_public || is_facet <> 'YESYES'


