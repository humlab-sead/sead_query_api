with anchors (anchor) as (
    select anchor
    from unnest(array['tbl_sites', 'tbl_datasets', 'tbl_physical_samples', 'tbl_datasets']) as anchor
)