SELECT category, count(value) AS count
FROM (
    SELECT tbl_sites.site_id AS category, tbl_analysis_entities.analysis_entity_id AS value
    FROM tbl_analysis_entities
    LEFT JOIN tbl_physical_samples
	  ON tbl_physical_samples."physical_sample_id" = tbl_analysis_entities."physical_sample_id"
	LEFT JOIN tbl_sample_groups
	  ON tbl_sample_groups."sample_group_id" = tbl_physical_samples."sample_group_id"
	LEFT JOIN tbl_sites
	  ON tbl_sites."site_id" = tbl_sample_groups."site_id"
	LEFT JOIN tbl_datasets
	  ON tbl_datasets."dataset_id" = tbl_analysis_entities."dataset_id"
	INNER JOIN facet.method_measured_values method_values
	  ON method_values."physical_sample_id" = tbl_physical_samples."physical_sample_id"
    WHERE 1 = 1
    AND ( (method_values.measured_value >= 100 and method_values.measured_value <= 200))
    GROUP BY tbl_sites.site_id, tbl_analysis_entities.analysis_entity_id
) AS x
GROUP BY category
