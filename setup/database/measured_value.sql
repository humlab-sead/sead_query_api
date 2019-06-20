
DO $$
DECLARE v_methods_sql text;
DECLARE v_method RECORD;
DECLARE v_joins text;
DECLARE v_table_name text;
DECLARE v_column_name text;
DECLARE v_column_names text;
DECLARE v_sql text;
BEGIN
    v_methods_sql = '
        SELECT ds.method_id as dataset_method_id, COALESCE(pm.method_id,0::int) as prep_method_id,  count(*)
        FROM tbl_analysis_entities ae
        LEFT JOIN tbl_analysis_entity_prep_methods pm
          ON pm.analysis_entity_id = ae.analysis_entity_id
        JOIN tbl_datasets ds
          ON ae.dataset_id = ds.dataset_id
        WHERE ds.method_id is not null
        GROUP BY 1, 2
        ORDER BY 1
    ';
    
    v_joins = '';
    v_column_names = '';
    
    FOR v_method in EXECUTE v_methods_sql
    LOOP
    
        -- RAISE NOTICE '% ', v_method;
        
        IF v_column_names <> '' THEN
            v_column_names = v_column_names || ', ';
        END IF;
        
        v_table_name   = 'values_' || v_method.dataset_method_id::text || '_' || v_method.prep_method_id::text;
        v_column_name  = 'value_' || v_method.dataset_method_id::text || '_' || v_method.prep_method_id::text;
     
        v_column_names = v_column_names || 'max(' || v_table_name || '.measured_value'::text || ') as ' || v_column_name;
        
        v_joins = v_joins || '
            LEFT JOIN measured_values AS ' || v_table_name || '
              ON ' || v_table_name || '.method_id = ' || v_method.dataset_method_id::text || '
             AND COALESCE(' || v_table_name || '.method_id, 0) = ' || v_method.prep_method_id::text || '
             AND ' || v_table_name || '.analysis_entity_id = tbl_analysis_entities.analysis_entity_id';
             
    END LOOP;

    v_sql = '
        WITH measured_values AS (
            SELECT ds.method_id, COALESCE(pm.method_id, 0) AS prep_method_id, mv.measured_value, mv.analysis_entity_id 
            FROM tbl_measured_values mv
            JOIN tbl_analysis_entities ae
              ON mv.analysis_entity_id = ae.analysis_entity_id 
            JOIN tbl_datasets ds
              ON ds.dataset_id = ae.dataset_id 
            LEFT JOIN tbl_analysis_entity_prep_methods pm
              ON pm.analysis_entity_id = ae.analysis_entity_id 
        ) 
            SELECT ps.physical_sample_id, ' || v_column_names || '
            FROM tbl_analysis_entities ae
            JOIN tbl_physical_samples ps
              ON ps.physical_sample_id = ae.physical_sample_id
            ' || v_joins || '
            GROUP BY ps.physical_sample_id             
            ORDER BY ps.physical_sample_id
    ';
    
    RAISE NOTICE '% ', v_sql;
    
END $$
LANGUAGE plpgsql;

