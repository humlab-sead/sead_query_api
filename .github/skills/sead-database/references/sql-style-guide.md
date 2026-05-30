# SQL Style Guide

## Ground Rules

- Use explicit `JOIN` clauses and name the join path in prose before writing SQL.
- Start from a narrow scope such as one site, one dataset, or one sample.
- Prefer verified backbone chains from trusted foreign-key metadata or live schema exports over guessed shortcuts.
- State whether the query is about abundance records, generic analysis values, interpreted ages, or absolute dates.
- Warn when typed analysis-value tables need live-schema validation.

## Common Pitfalls

- Joining site-level, sample-level, and dataset-contact tables in one flat query can multiply rows quickly.
- `tbl_analysis_entities` is the bridge; skipping it often produces wrong joins.
- `tbl_analysis_entity_ages` and `tbl_geochronology` are different concepts. One is interpreted chronology, the other is a dating record.
- `tbl_datasets` is proxy packaging, not a synonym for site or project.
- `tbl_taxa_tree_master` is the taxon endpoint for abundance and other taxon-linked tables, but taxonomy context may require extra joins to genus, family, or order tables.

## Query Template: samples and datasets for a site

```sql
select
    s.site_id,
    s.site_name,
    sg.sample_group_id,
    sg.sample_group_name,
    ps.physical_sample_id,
    ps.sample_name,
    d.dataset_id,
    d.dataset_name,
    m.method_name
from public.tbl_sites s
join public.tbl_sample_groups sg
    on sg.site_id = s.site_id
join public.tbl_physical_samples ps
    on ps.sample_group_id = sg.sample_group_id
join public.tbl_analysis_entities ae
    on ae.physical_sample_id = ps.physical_sample_id
join public.tbl_datasets d
    on d.dataset_id = ae.dataset_id
left join public.tbl_methods m
    on m.method_id = d.method_id
where s.site_id = $1
order by sg.sample_group_name, ps.sample_name, d.dataset_name;
```

## Query Template: taxa recorded for a site

```sql
select
    s.site_name,
    ps.sample_name,
    d.dataset_name,
    t.taxon_id,
    t.species,
    a.abundance
from public.tbl_sites s
join public.tbl_sample_groups sg
    on sg.site_id = s.site_id
join public.tbl_physical_samples ps
    on ps.sample_group_id = sg.sample_group_id
join public.tbl_analysis_entities ae
    on ae.physical_sample_id = ps.physical_sample_id
join public.tbl_datasets d
    on d.dataset_id = ae.dataset_id
join public.tbl_abundances a
    on a.analysis_entity_id = ae.analysis_entity_id
join public.tbl_taxa_tree_master t
    on t.taxon_id = a.taxon_id
where s.site_id = $1
order by ps.sample_name, d.dataset_name, t.species;
```

## Query Template: dating context for a site

```sql
select
    s.site_name,
    ps.sample_name,
    ch.chronology_name,
    aea.age,
    aea.age_younger,
    aea.age_older,
    g.age as measured_age,
    g.error_younger,
    g.error_older,
    g.lab_number
from public.tbl_sites s
join public.tbl_sample_groups sg
    on sg.site_id = s.site_id
join public.tbl_physical_samples ps
    on ps.sample_group_id = sg.sample_group_id
join public.tbl_analysis_entities ae
    on ae.physical_sample_id = ps.physical_sample_id
left join public.tbl_analysis_entity_ages aea
    on aea.analysis_entity_id = ae.analysis_entity_id
left join public.tbl_chronologies ch
    on ch.chronology_id = aea.chronology_id
left join public.tbl_geochronology g
    on g.analysis_entity_id = ae.analysis_entity_id
where s.site_id = $1
order by ps.sample_name;
```

## Review Checklist For Generated SQL

- Are the joins anchored on the correct backbone path?
- Does the query mix interpreted ages and absolute dates intentionally?
- Is row multiplication acceptable, or should the query aggregate first?
- Are method, project, and provenance joins added only when needed?
- Does the answer clearly label assumptions and validation needs?