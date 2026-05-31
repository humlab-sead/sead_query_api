
update facet.result_field set result_field_key = 'category_id', table_name = 'tbl_sites', column_name = 'category_id', display_text = 'Site ID' where result_field_id = 18;
update facet.result_field set result_field_key = 'category_name', table_name = 'tbl_sites', column_name = 'category_name', display_text = 'Site Name' where result_field_id = 19;
update facet.result_field set result_field_key = 'latitude_dd', table_name = '', column_name = 'latitude_dd', display_text = 'Latitude (dd)' where result_field_id = 20;
update facet.result_field set result_field_key = 'longitude_dd', table_name = '', column_name = 'longitude_dd', display_text = 'Longitude (dd)' where result_field_id = 21;

UPDATE facet.result_field set table_name = null where result_field_id in (18,19,20,21)

alter table facet.result_aggregate_field add column sequence_id int not null default(0);

select -- ra.aggregate_key, raf.aggregate_field_id, raf.field_type_id, raf.sequence_id, rf.result_field_id, rf.result_field_key,
	'UPDATE facet.result_aggregate_field set sequence_id = ' || rf.result_field_id || ' where aggregate_field_id = ' || raf.aggregate_field_id || '; -- ' || ra.aggregate_key || '.' || rf.result_field_key
from facet.result_aggregate ra
join facet.result_aggregate_field raf
  on raf.aggregate_id = ra.aggregate_id
join facet.result_field rf
  on rf.result_field_id = raf.result_field_id
order by ra.aggregate_id, raf.aggregate_field_id;

UPDATE facet.result_aggregate_field set sequence_id = 1 where aggregate_field_id = 4; -- site_level.sitename'
UPDATE facet.result_aggregate_field set sequence_id = 2 where aggregate_field_id = 5; -- site_level.record_type'
UPDATE facet.result_aggregate_field set sequence_id = 3 where aggregate_field_id = 8; -- site_level.analysis_entities'
UPDATE facet.result_aggregate_field set sequence_id = 4 where aggregate_field_id = 10; -- site_level.site_link'
UPDATE facet.result_aggregate_field set sequence_id = 5 where aggregate_field_id = 16; -- site_level.site_link_filtered'
UPDATE facet.result_aggregate_field set sequence_id = 99 where aggregate_field_id = 13; -- site_level.sitename'

UPDATE facet.result_aggregate_field set sequence_id = 1 where aggregate_field_id = 15; -- aggregate_all.aggregate_all_filtered'
UPDATE facet.result_aggregate_field set sequence_id = 2 where aggregate_field_id = 7; -- aggregate_all.analysis_entities'

UPDATE facet.result_aggregate_field set sequence_id = 1 where aggregate_field_id = 1; -- sample_group_level.sitename'
UPDATE facet.result_aggregate_field set sequence_id = 2 where aggregate_field_id = 2; -- sample_group_level.sample_group'
UPDATE facet.result_aggregate_field set sequence_id = 3 where aggregate_field_id = 3; -- sample_group_level.record_type'
UPDATE facet.result_aggregate_field set sequence_id = 4 where aggregate_field_id = 6; -- sample_group_level.analysis_entities'
UPDATE facet.result_aggregate_field set sequence_id = 5 where aggregate_field_id = 9; -- sample_group_level.sample_group_link'
UPDATE facet.result_aggregate_field set sequence_id = 6 where aggregate_field_id = 14; -- sample_group_level.sample_group_link_filtered'
UPDATE facet.result_aggregate_field set sequence_id = 99 where aggregate_field_id = 11; -- sample_group_level.sitename'
UPDATE facet.result_aggregate_field set sequence_id = 99 where aggregate_field_id = 12; -- sample_group_level.sample_group'

UPDATE facet.result_aggregate_field set sequence_id = 1 where aggregate_field_id = 23; -- map_result.category_id'
UPDATE facet.result_aggregate_field set sequence_id = 2 where aggregate_field_id = 24; -- map_result.category_name'
UPDATE facet.result_aggregate_field set sequence_id = 3 where aggregate_field_id = 21; -- map_result.latitude_dd'
UPDATE facet.result_aggregate_field set sequence_id = 4 where aggregate_field_id = 22; -- map_result.longitude_dd'

SELECT category, sum(value) AS count FROM (
    SELECT tbl_taxa_tree_master.taxon_id AS category, metainformation.view_abundance.abundance AS value 
    FROM metainformation.view_abundance LEFT JOIN tbl_taxa_tree_master ON tbl_taxa_tree_master."taxon_id" = metainformation.view_abundance."taxon_id" LEFT JOIN tbl_taxa_tree_genera ON tbl_taxa_tree_genera."genus_id" = tbl_taxa_tree_master."genus_id" LEFT JOIN tbl_taxa_tree_authors ON tbl_taxa_tree_authors."author_id" = tbl_taxa_tree_master."author_id" LEFT JOIN tbl_analysis_entities ON tbl_analysis_entities."analysis_entity_id" = metainformation.view_abundance."analysis_entity_id" LEFT JOIN tbl_physical_samples ON tbl_physical_samples."physical_sample_id" = tbl_analysis_entities."physical_sample_id" LEFT JOIN tbl_sample_groups ON tbl_sample_groups."sample_group_id" = tbl_physical_samples."sample_group_id" LEFT JOIN tbl_sites ON tbl_sites."site_id" = tbl_sample_groups."site_id" WHERE 1 = 1 AND metainformation.view_abundance.abundance is not null GROUP BY tbl_taxa_tree_master.taxon_id, metainformation.view_abundance.abundance ) AS x GROUP BY category; 
  
select * from facet.facet order by facet_id

update facet.facet set category_name_expr = 'concat_ws('' '', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)' where facet_key = 'species';
update facet.facet set aggregate_facet_id = 32 where facet_key = 'species';
