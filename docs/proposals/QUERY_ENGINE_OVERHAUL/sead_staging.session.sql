-- @block
with anchors (anchor) as (
    select anchor
    from unnest(array['tbl_sites', 'tbl_datasets', 'tbl_physical_samples', 'tbl_datasets']) as anchor
), facet_source as (
    select f.facet_id, t.table_or_udf_name, t.primary_key_name, t.is_udf, ft.udf_call_arguments
    from facet.facet f
    join (
        select x.facet_id, min(x.sequence_id) as sequence_id
        from facet.facet_table x
        group by x.facet_id
    ) as m using (facet_id)
    join facet.facet_table ft
      on ft.facet_id = m.facet_id
     and ft.sequence_id = m.sequence_id
    join facet.table t using (table_id)
)
    select distinct format('[("%s", "%s")],', table_or_udf_name, anchor) as facet_source
    from anchors
    cross join facet_source
    where table_or_udf_name like '%tbl_%'
      and table_or_udf_name != anchor;

-- @block
