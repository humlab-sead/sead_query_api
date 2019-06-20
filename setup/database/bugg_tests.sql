select lower, upper, facet_term, count(facet_term) as direct_count 
from (
    select COALESCE(lower || '=>' || upper, 'data missing') as facet_term, group_column --lower, upper 
    from (
        select lower, upper, tbl_analysis_entities.analysis_entity_id as group_column
        from metainformation.tbl_denormalized_measured_values 
        left join (
            SELECT n::text || ' => ' || (n + 23)::text, n as lower, n + 23 as upper
            FROM generate_series(110, 2904+23, 23) as a(n)
            WHERE n <= 2904
        ) as temp_interval
          on metainformation.tbl_denormalized_measured_values.value_33_0 :: integer >= lower 
         and metainformation.tbl_denormalized_measured_values.value_33_0 :: integer < upper 
        inner join tbl_physical_samples on tbl_physical_samples."physical_sample_id" = metainformation.tbl_denormalized_measured_values."physical_sample_id" 
        left join tbl_analysis_entities on tbl_analysis_entities."physical_sample_id" = tbl_physical_samples."physical_sample_id" 
          and ((
              metainformation.tbl_denormalized_measured_values.value_33_0 >= 110 
              and metainformation.tbl_denormalized_measured_values.value_33_0 <= 2904
          ))
        where lower = 110 and upper = 133
        group by lower, upper, tbl_analysis_entities.analysis_entity_id 
        order by lower
      ) as tmp4 
    group by lower, upper, group_column
  ) as tmp3 
where TRUE
  and lower is not null 
  and upper is not null 
group by lower, upper, facet_term 
order by lower, upper ;

select lower, upper, tbl_physical_samples."physical_sample_id", COUNT(DISTINCT tbl_analysis_entities.analysis_entity_id) AS count_column --, tbl_analysis_entities.analysis_entity_id as group_column
from metainformation.tbl_denormalized_measured_values 
left join (
    SELECT n::text || ' => ' || (n + 23)::text, n as lower, n + 23 as upper
    FROM generate_series(110, 2904+23, 23) as a(n)
    WHERE n <= 2904
) as temp_interval
  on metainformation.tbl_denormalized_measured_values.value_33_0 :: integer >= lower 
 and metainformation.tbl_denormalized_measured_values.value_33_0 :: integer < upper 
inner join tbl_physical_samples on tbl_physical_samples."physical_sample_id" = metainformation.tbl_denormalized_measured_values."physical_sample_id" 
left join tbl_analysis_entities on tbl_analysis_entities."physical_sample_id" = tbl_physical_samples."physical_sample_id" 
  and ((
      metainformation.tbl_denormalized_measured_values.value_33_0 >= 110 
      and metainformation.tbl_denormalized_measured_values.value_33_0 <= 2904
  ))
where lower = 110 and upper = 133
group by lower, upper, tbl_analysis_entities.analysis_entity_id,  tbl_physical_samples."physical_sample_id"
order by count_column
        
WITH categories(category, lower, upper) AS ((
    SELECT n::text || ' => ' || (n + 23)::text, n, n + 23
    FROM generate_series(110, 2904+23, 23) as a(n)
    WHERE n <= 2904)
)
    SELECT category, lower, upper, COUNT(DISTINCT tbl_analysis_entities.analysis_entity_id) AS count_column
    FROM categories
    LEFT JOIN metainformation.tbl_denormalized_measured_values 
      ON metainformation.tbl_denormalized_measured_values.value_33_0::integer >= lower
     AND metainformation.tbl_denormalized_measured_values.value_33_0::integer < upper
     INNER JOIN tbl_physical_samples  ON tbl_physical_samples."physical_sample_id" = metainformation.tbl_denormalized_measured_values."physical_sample_id"
     LEFT JOIN tbl_analysis_entities  ON tbl_analysis_entities."physical_sample_id" = tbl_physical_samples."physical_sample_id" 
     AND (( (metainformation.tbl_denormalized_measured_values.value_33_0 >= 110
             AND metainformation.tbl_denormalized_measured_values.value_33_0 <= 2904)))
    WHERE lower = 110 and upper = 133
    GROUP BY category, lower, upper
    ORDER BY lower;

select *
from metainformation.tbl_denormalized_measured_values m
left join tbl_analysis_entities ae
  on ae.physical_sample_id = m.physical_sample_id
where value_33_0 is not NULL
  and ae.analysis_entity_id is null
             
