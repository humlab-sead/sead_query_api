WITH foreign_keys AS (
	SELECT tc.table_schema, tc.table_name, kcu.column_name, ccu.table_name AS foreign_table_name, ccu.column_name AS foreign_column_name
	FROM information_schema.table_constraints AS tc
	JOIN information_schema.key_column_usage AS kcu USING (table_schema, constraint_name)
	JOIN information_schema.constraint_column_usage AS ccu USING (table_schema, constraint_name)
	WHERE tc.constraint_type = 'FOREIGN KEY'
	  AND tc.table_schema = 'public'
), primary_keys AS (
	SELECT tc.table_schema, tc.table_name, kcu.column_name
	FROM information_schema.table_constraints AS tc
	JOIN information_schema.key_column_usage AS kcu USING (table_schema, constraint_name)
	WHERE tc.constraint_type = 'PRIMARY KEY'
	  AND tc.table_schema = 'public'
)
  SELECT
  	format('alter table public.%s add column if not exists %s uuid not null default(uuid_generate_v4());',
					fk.table_name, replace(pk.column_name, '_id', '_uuid')) as add_sql,
  	format('alter table public.%s add constraint fk_%s_%s foreign key (%s) references %s (%s);',
				fk.table_name, replace(fk.table_name, 'tbl_', ''), fk.column_name, fk.column_name, foreign_table_name, foreign_column_name) as fk_sql,
  	format('alter table public.%s drop column if exists %s;', fk.table_name, replace(pk.column_name, '_id', '_uuid')) as drop_sql
  FROM foreign_keys fk
  JOIN primary_keys pk
    ON pk.table_schema = fk.table_schema
   AND pk.table_name = fk.table_name
  WHERE foreign_table_name = 'tbl_biblio'


create table tbl_biblio_references (
    biblio_reference_id serial primary key not null,
    reference_uuid uuid not null,
    bibio_uuid uuid not null,
    date_updated timestamp with time zone DEFAULT now(),
    CONSTRAINT fk_biblio_references_biblio_uuid FOREIGN KEY (bibio_uuid)
        REFERENCES public.tbl_biblio (bibio_uuid) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

-- create extension "uuid-ossp";

alter table public.tbl_aggregate_datasets drop column if exists aggregate_dataset_uuid;
alter table public.tbl_dataset_masters add column if exists master_set_uuid;
alter table public.tbl_datasets add column if exists dataset_uuid;
alter table public.tbl_ecocode_systems add column if exists ecocode_system_uuid;
alter table public.tbl_geochronology add column if exists geochron_uuid -- tbl_geochron_refs
alter table public.tbl_methods add column if exists method_uuid;
alter table public.tbl_rdb_systems add column if exists rdb_system_uuid;
alter table public.tbl_relative_ages add column if exists relative_age_uuid; -- tbl_relative_age_refs
alter table public.tbl_sample_groups add column if exists sample_group_uuid;--tbl_sample_group_references
alter table public.tbl_site_other_records add column if exists site_other_records_uuid;
alter table public.tbl_sites add column if exists site_uuid; -- tbl_site_references
alter table public.tbl_species_associations add column if exists species_association_uuid;
alter table public.tbl_taxa_synonyms add column if exists synonym_uuid;
alter table public.tbl_taxonomic_order_biblio add column if exists taxonomic_order_biblio_uuid;
alter table public.tbl_taxonomy_notes add column if exists taxonomy_notes_uuid;
alter table public.tbl_tephras add column if exists tephra_uuid; --tbl_tephra_refs
alter table public.tbl_text_biology add column if exists biology_uuid;
alter table public.tbl_text_distribution add column if exists distribution_uuid;
alter table public.tbl_text_identification_keys add column if exists key_uuid;
alter table public.biblio_uuid add column if exists biblio_uuid;

alter table public.biblio_uuid add column if not exists biblio_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_aggregate_datasets add column if not exists aggregate_dataset_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_dataset_masters add column if not exists master_set_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_datasets add column if not exists dataset_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_ecocode_systems add column if not exists ecocode_system_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_geochronology add column if not exists geochron_uuid uuid not null default(uuid_generate_v4()) -- tbl_geochron_refs
alter table public.tbl_methods add column if not exists method_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_rdb_systems add column if not exists rdb_system_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_relative_ages add column if not exists relative_age_uuid uuid not null default(uuid_generate_v4()); -- tbl_relative_age_refs
alter table public.tbl_sample_groups add column if not exists sample_group_uuid uuid not null default(uuid_generate_v4());--tbl_sample_group_references
alter table public.tbl_site_other_records add column if not exists site_other_records_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_sites add column if not exists site_uuid uuid not null default(uuid_generate_v4()); -- tbl_site_references
alter table public.tbl_species_associations add column if not exists species_association_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_taxa_synonyms add column if not exists synonym_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_taxonomic_order_biblio add column if not exists taxonomic_order_biblio_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_taxonomy_notes add column if not exists taxonomy_notes_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_tephras add column if not exists tephra_uuid uuid not null default(uuid_generate_v4()); --tbl_tephra_refs
alter table public.tbl_text_biology add column if not exists biology_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_text_distribution add column if not exists distribution_uuid uuid not null default(uuid_generate_v4());
alter table public.tbl_text_identification_keys add column if not exists key_uuid uuid not null default(uuid_generate_v4());

with consolidated_references (reference_uuid, biblio_uuid) as (
    select aggregate_dataset_uuid, biblio_uuid
    from tbl_aggregate_datasets
    join tbl_biblio using (biblio_id)
        UNION
    select master_set_uuid, biblio_uuid
    from tbl_dataset_masters
    join tbl_biblio using (biblio_id)
        UNION
    select master_set_uuid, biblio_uuid
    from tbl_datasets
    join tbl_biblio using (biblio_id)
        UNION
    select ecocode_system_uuid, biblio_uuid
    from tbl_ecocode_systems
    join tbl_biblio using (biblio_id)
        UNION
    select geochron_uuid, biblio_uuid
    from tbl_geochron_refs
    from tbl_geochronology using (geochron_id)
    join tbl_biblio using (biblio_id)
        UNION
    select method_uuid, biblio_uuid
    from tbl_methods
    join tbl_biblio using (biblio_id)
        UNION
    select rdb_system_uuid, biblio_uuid
    from tbl_rdb_systems
    join tbl_biblio using (biblio_id)
        UNION
    select relative_age_uuid, biblio_uuid
    from tbl_relative_age_refs
    from tbl_relative_ages using (relative_age_id)
    join tbl_biblio using (biblio_id)
        UNION
    select sample_group_uuid, biblio_uuid
    from tbl_sample_group_references
    from tbl_sample_groups using (sample_group_id)
    join tbl_biblio using (biblio_id)
        UNION
    select site_other_records_uuid, biblio_uuid
    from tbl_site_other_records
    join tbl_biblio using (biblio_id)
        UNION
    select site_uuid, biblio_uuid
    from tbl_site_references
    from tbl_sites using (site_id)
    join tbl_biblio using (biblio_id)
        UNION
    select species_association_uuid, biblio_uuid
    from tbl_species_associations
    join tbl_biblio using (species_association_id)
        UNION
    select synonym_uuid, biblio_uuid
    from tbl_taxa_synonyms
    join tbl_biblio using (synonym_id)
        UNION
    select taxonomic_order_biblio_uuid, biblio_uuid
    from tbl_taxonomic_order_biblio
    join tbl_biblio using (taxonomic_order_biblio_id)
        UNION
    select taxonomy_notes_uuid, biblio_uuid
    from tbl_taxonomy_notes
    join tbl_biblio using (taxonomy_notes_id)
        UNION
    select tephra_uuid, biblio_uuid
    from tbl_tephra_refs
    from tbl_tephras using (tephra_id)
    join tbl_biblio using (biblio_id)
        UNION
    select biology_uuid, biblio_uuid
    from tbl_text_biology
    join tbl_biblio using (biology_id)
        UNION
    select distribution_uuid, biblio_uuid
    from tbl_text_distribution
    join tbl_biblio using (distribution_id)
        UNION
    select key_uuid, biblio_uuid
    from tbl_text_identification_keys
    join tbl_biblio using (key_id)
)
    insert into tbl_biblio_references (reference_uuid, bibio_uuid)
    select reference_uuid, bibio_uuid
    from consolidated_references

