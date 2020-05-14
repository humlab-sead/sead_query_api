-- Deploy sead_api:20190101_DML_FACET_SCHEMA to pg

/****************************************************************************************************************
  Author        Roger Mähler
  Date          2019-01-01
  Description
  Reviewer
  Approver
  Rollback
  Idempotent    Yes
  Notes
*****************************************************************************************************************/



begin;
do $$
begin

    begin

        if current_database() != 'study_model' then
            raise exception 'This script must be run in fitness!';
        end if;

        set client_encoding = 'UTF8';
        set standard_conforming_strings = on;
        set check_function_bodies = false;
        set client_min_messages = warning;

		-- drop schema if exists facet cascade;

		create schema if not exists facet;

-- 		select '(''' || table_name || ''', ''' || table_name || '_id'', ' || (row_number() over(order by table_name))::text || ', FALSE), '
-- 		from information_schema.tables
-- 		where table_catalog = 'fitness'
-- 		  and table_schema = 'public'
-- 		  and table_type = 'BASE TABLE'
-- 		order by table_name

		delete from facet.table;

		with new_tables(table_or_udf_name, primary_key_name, table_id, is_udf) as (values
			('residence', 'residence_id', 1, FALSE),
			('residence_type', 'residence_type_id', 2, FALSE),
			('experiment', 'experiment_id', 3, FALSE),
			('experiment_type', 'experiment_type_id', 4, FALSE),
			('observed_span', 'observed_span_id', 5, FALSE),
			('project', 'project_id', 6, FALSE),
			('project_residence', 'project_residence_id', 7, FALSE),
			('project_property', 'project_property_id', 8, FALSE),
			('project_report', 'project_report_id', 9, FALSE),
			('project_type', 'project_type_id', 10, FALSE),
			('method', 'method_id', 11, FALSE),
			('method_type', 'method_type_id', 12, FALSE),
			('subject', 'subject_id', 13, FALSE),
			('subject_note', 'subject_note_id', 14, FALSE),
			('subject_type', 'subject_type_id', 15, FALSE),
			('observed_count', 'observed_count_id', 16, FALSE),
			('report', 'report_id', 17, FALSE),
			('observed_magnitude', 'observed_magnitude_id', 18, FALSE),
			('cohort', 'cohort_id', 19, FALSE),
			('cohort_description', 'cohort_description_id', 20, FALSE),
			('study', 'study_id', 21, FALSE),
			('study_type', 'study_type_id', 24, FALSE),
			('academic', 'study_id', 22, FALSE),
			('academic_type', 'study_id', 23, FALSE)
		)
		insert into facet.table (table_or_udf_name, primary_key_name, table_id, is_udf)
			select table_or_udf_name, primary_key_name, table_id, is_udf
			from new_tables
            on conflict (table_or_udf_name) do nothing;

		/* Add relations */
        insert into facet.table_relation (source_table_id, target_table_id, weight, source_column_name, target_column_name)
			WITH foreign_key_edges(source_table, target_table, column_name) AS (
				SELECT tc.table_name, ccu.table_name AS foreign_table_name, ccu.column_name AS foreign_column_name
				FROM information_schema.table_constraints AS tc
				JOIN information_schema.key_column_usage AS kcu USING (table_schema, constraint_name)
				JOIN information_schema.constraint_column_usage AS ccu USING (table_schema, constraint_name)
				WHERE tc.constraint_type = 'FOREIGN KEY'
				  AND tc.table_schema = 'public'
            ) select t1.table_id as source_table_id,
                    t2.table_id as target_table_id,
                    20 as weight,
                    column_name as source_column_name,
                    column_name as target_column_name
            from foreign_key_edges
            join facet.table t1 on t1.table_or_udf_name = foreign_key_edges.source_table
            join facet.table t2 on t2.table_or_udf_name = foreign_key_edges.target_table
            left join facet.table_relation cx
                on cx.source_table_id = t1.table_id
            and cx.target_table_id = t2.table_id
            where cx.table_relation_id is null;

        insert into facet.facet_group (facet_group_id, facet_group_key, display_title, description, is_applicable, is_default) (values
            (99, 'ROOT', 'ROOT', 'ROOT', false, false),
            (1, 'others', 'Others', 'Others', true, false),
            (999, 'DOMAIN', 'DOMAIN', 'DOMAIN', false, false)
        ) on conflict (facet_group_id) do update
            set facet_group_key = excluded.facet_group_key,
                display_title = excluded.display_title,
                description = excluded.description,
                is_applicable = excluded.is_applicable,
                is_default = excluded.is_default;

       insert into facet.facet_type (facet_type_id, facet_type_name, reload_as_target) (values
            (9, 'undefined', false),
            (1, 'discrete', false),
            (2, 'range', true)
        ) on conflict (facet_type_id) do update
            set facet_type_name = excluded.facet_type_name,
                reload_as_target = excluded.reload_as_target;

       insert into facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) (values
            ('sum_item', true, 'TemplateFieldCompiler', true, false, false, 'SUM({0}::double precision) AS sum_of_{0}'),
            ('count_item', true, 'TemplateFieldCompiler', true, false, false, 'COUNT({0}) AS count_of_{0}'),
            ('avg_item', true, 'TemplateFieldCompiler', true, false, false, 'AVG({0}) AS avg_of_{0}'),
            ('text_agg_item', true, 'TemplateFieldCompiler', true, false, false, 'ARRAY_TO_STRING(ARRAY_AGG(DISTINCT {0}),'','') AS text_agg_of_{0}'),
            ('single_item', true, 'TemplateFieldCompiler', false, false, true, '{0}'),
            ('sort_item', false, 'TemplateFieldCompiler', false, true, false, '{0}')
        ) on conflict (field_type_id) do update
            set is_result_value = excluded.is_result_value,
                sql_field_compiler = excluded.sql_field_compiler,
                is_aggregate_field = excluded.is_aggregate_field,
                is_sort_field = excluded.is_sort_field,
                is_item_field = excluded.is_item_field,
                sql_template = excluded.sql_template;

       insert into facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) (values
            ( 1, 'project', 'project', 'project.name', 'Project', 'single_item', true, NULL, NULL),
            ( 2, 'experiment_type', 'experiment_type', 'experiment_type.name', 'Data type', 'text_agg_item', true, NULL, NULL),
            ( 3, 'experiment', 'experiment', 'experiment.experiment_id', 'Experiment', 'single_item', true, NULL, NULL),
            ( 4, 'observed_count', 'observed_count', 'observed_count.count', 'Number of observed_counts', 'single_item', true, NULL, NULL),
            ( 5, 'observed_magnitude', 'observed_count', 'observed_count.count', 'Number of observed_counts', 'single_item', true, NULL, NULL),
            ( 6, 'study', 'study', 'study.study_name', 'Study', 'single_item', true, NULL, NULL),
            ( 7, 'cohort', 'cohort', 'cohort.name', 'Team', 'single_item', true, NULL, NULL),
            ( 8, 'method', 'method', 'method.method_name', 'Method', 'single_item', true, NULL, NULL),
            ( 9, 'category_id', 'project', 'category_id', 'Project ID', 'single_item', true, NULL, NULL),
            (10, 'category_name', 'project', 'category_name', 'Project Name', 'single_item', true, NULL, NULL),
            (11, 'latitude_dd', 'project', 'latitude_dd', 'Latitude (dd)', 'single_item', true, NULL, NULL),
            (12, 'longitude_dd', 'project', 'longitude_dd', 'Longitude (dd)', 'single_item', true, NULL, NULL)
        ) on conflict (result_field_id) do update
            set result_field_key = excluded.result_field_key,
                table_name = excluded.table_name,
                column_name = excluded.column_name,
                display_text = excluded.display_text,
                field_type_id = excluded.field_type_id,
                activated = excluded.activated,
                link_url = excluded.link_url,
                link_label = excluded.link_label;

        insert into facet.result_specification (specification_id, specification_key, display_text, is_applicable, is_activated) (values
            (1, 'tabular_default', 'Tabular default', false, true),
            (2, 'map_default', 'Map default', false, true)
        ) on conflict (specification_id) do update
            set specification_key = excluded.specification_key,
                display_text = excluded.display_text,
                is_applicable = excluded.is_applicable,
                is_activated = excluded.is_activated;

       with result_specification_data(specification_field_id, specification_key, result_field_key, field_type_id, sequence_id) as (
		 (values
				( 1, 'tabular_default', 'project',         'single_item',    1),
				( 2, 'tabular_default', 'experiment_type', 'text_agg_item',  2),
				( 3, 'tabular_default', 'experiment',      'count_item',     3),
				( 4, 'tabular_default', 'project',         'sort_item',     99),
				( 5, 'map_default',     'category_id',     'single_item',    1),
				( 6, 'map_default',     'category_name',   'single_item',    2),
				( 7, 'map_default',     'latitude_dd',     'single_item',    3),
				( 8, 'map_default',     'longitude_dd',    'single_item',    4)
	   ))
	    insert into facet.result_specification_field (specification_field_id, specification_id, result_field_id, field_type_id, sequence_id)
			select d.specification_field_id, s.specification_id, rf.result_field_id, d.field_type_id, d.sequence_id
			from result_specification_data d
			join facet.result_specification s using (specification_key)
			join facet.result_field rf using (result_field_key);

       insert into facet.result_view_type (view_type_id, view_name, is_cachable) (values
            ('tabular', 'Tabular', true),
            ('map', 'Map', false)
        ) on conflict (view_type_id) do update
            set view_name = excluded.view_name,
                is_cachable = excluded.is_cachable;

        --insert into facet.view_state (view_state_key, view_state_data, create_time) (values
        --) on conflict do nothing;

   exception when sqlstate 'GUARD' then
        raise notice 'ALREADY EXECUTED';
    end;

end $$;
commit;
