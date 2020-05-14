-- Deploy sead_api:20200429_DML_FACET_UPDATES to pg
/****************************************************************************************************************
  Author        Roger Mähler
  Date          2020
  Description
  Prerequisites
  Reviewer
  Approver
  Idempotent    Yes
  Notes
*****************************************************************************************************************/

begin;
do $$
declare s_facets text;
declare j_facets jsonb;
begin

    begin

        set search_path = facet, pg_catalog;
        set client_encoding = 'UTF8';

		s_facets = $facets$
        [
            {
                "facet_id": 1,
                "facet_code": "result_facet",
                "display_title": "Experiments",
                "description": "Experiments",
                "facet_group_id":"99",
                "facet_type_id": 1,
                "category_id_expr": "experiment.experiment_id",
                "category_name_expr": "subject.name||' '||study.study_name",
                "sort_expr": "study.study_name",
                "is_applicable": false,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of subjects",
                "aggregate_facet_code": null,
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "experiment",
                        "udf_call_arguments": null,
                        "alias":  null
                    },
                    {
                        "sequence_id": 2,
                        "table_name": "subject",
                        "udf_call_arguments": null,
                        "alias":  null
                    },
                    {
                        "sequence_id": 3,
                        "table_name": "study",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [  ]
            },
            {
                "facet_id": 2,
                "facet_code": "map_result",
                "display_title": "Project",
                "description": "Project",
                "facet_group_id":"99",
                "facet_type_id": 1,
                "category_id_expr": "project.project_id",
                "category_name_expr": "project.project_name",
                "sort_expr": "project.project_id",
                "is_applicable": false,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of projects",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "project",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [  ]
            },
            {
                "facet_id": 3,
                "facet_code": "study_type",
                "display_title": "Study type",
                "description": "Study type",
                "facet_group_id":"1",
                "facet_type_id": 1,
                "category_id_expr": "study_type.study_type_id",
                "category_name_expr": "study_type.study_type_name",
                "sort_expr": "study_type.study_type_name",
                "is_applicable": true,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of studies",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "study_type",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [  ]
            },
            {
                "facet_id": 4,
                "facet_code": "magnitude",
                "display_title": "Magnitude.",
                "description": "Magnitude",
                "facet_group_id":"1",
                "facet_type_id": 2,
                "category_id_expr": "observed_magnitude.value",
                "category_name_expr": "observed_magnitude.value",
                "sort_expr": "observed_magnitude.value",
                "is_applicable": true,
                "is_default": false,
                "aggregate_type": "",
                "aggregate_title": "Number of experiments",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "observed_magnitude",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [  ]
            },

            {
                "facet_id": 5,
                "facet_code": "cohort",
                "display_title": "Cohort",
                "description": "A collection of subjects",
                "facet_group_id":"1",
                "facet_type_id": 1,
                "category_id_expr": "cohort.cohort_id",
                "category_name_expr": "cohort.name",
                "sort_expr": "cohort.name",
                "is_applicable": true,
                "is_default": true,
                "aggregate_type": "count",
                "aggregate_title": "Number of samples",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "cohort",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [  ]
            },
            {
                "facet_id": 6,
                "facet_code": "project",
                "display_title": "Project",
                "description": "Project",
                "facet_group_id":"1",
                "facet_type_id": 1,
                "category_id_expr": "project.project_id",
                "category_name_expr": "project.project_name",
                "sort_expr": "project.project_name",
                "is_applicable": true,
                "is_default": true,
                "aggregate_type": "count",
                "aggregate_title": "Projects",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "project",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [  ]
            },
            {
                "facet_id": 7,
                "facet_code": "academic_project",
                "display_title": "Academic projects",
                "description": "",
                "facet_group_id":"1",
                "facet_type_id": 1,
                "category_id_expr": "countries.location_id",
                "category_name_expr": "countries.location_name ",
                "sort_expr": "countries.location_name",
                "is_applicable": true,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of samples",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "project",
                        "udf_call_arguments": null,
                        "alias":  "academic"
                    },
                    {
                        "sequence_id": 2,
                        "table_name": "project_type",
                        "udf_call_arguments": null,
                        "alias": "academic_type"
                    } ],
                "clauses": [
                    {
                        "clause": "academic_type.type_name='Academic'",
                        "enforce_constraint": true
                    }
                ]
            },
            {
                "facet_id": 8,
                "facet_code": "observed_count",
                "display_title": "Counts",
                "description": "Counts",
                "facet_group_id":"1",
                "facet_type_id": 2,
                "category_id_expr": "observed_count.count",
                "category_name_expr": "observed_count.count",
                "sort_expr": "observed_count.count",
                "is_applicable": true,
                "is_default": false,
                "aggregate_type": "",
                "aggregate_title": "Number of samples",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "observed_count",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [
                    {
                        "clause": "observed_count.count is not null",
                        "enforce_constraint": true
                    } ]
            },
            {
                "facet_id": 9,
                "facet_code": "study_method",
                "display_title": "Study methods",
                "description": "Study methods",
                "facet_group_id":"1",
                "facet_type_id": 1,
                "category_id_expr": "method.method_id ",
                "category_name_expr": "method.method_name",
                "sort_expr": "method.method_name",
                "is_applicable": true,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of studies",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "method",
                        "udf_call_arguments": null,
                        "alias":  null
                    },
                    {
                        "sequence_id": 2,
                        "table_name": "study",
                        "udf_call_arguments": null,
                        "alias":  null
                    }],
                "clauses": [  ]
            },
            {
                "facet_id": 10,
                "facet_code": "study",
                "display_title": "Datasets",
                "description": "Datasets",
                "facet_group_id":"1",
                "facet_type_id": 1,
                "category_id_expr": "study.study_id",
                "category_name_expr": "study.study_name",
                "sort_expr": "study.study_name",
                "is_applicable": true,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of studies",
                "aggregate_facet_code": null,
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "study",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [  ]
            },
            {
                "facet_id": 1001,
                "facet_code": "odd_method",
                "display_title": "Odd method",
                "description": "Odd method domain facet",
                "facet_group_id":"999",
                "facet_type_id": 1,
                "category_id_expr": "study.study_id",
                "category_name_expr": "study.study_name ",
                "sort_expr": "study.study_name",
                "is_applicable": false,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of studies",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "study",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [
                    {
                        "clause": "bool_and((study.method_id % 2) = 1) ",
                        "enforce_constraint": true
                    } ]
            },
            {
                "facet_id": 1002,
                "facet_code": "even_method",
                "display_title": "Even method",
                "description": "Even method domain facet",
                "facet_group_id":"999",
                "facet_type_id": 1,
                "category_id_expr": "study.study_id",
                "category_name_expr": "study.study_name ",
                "sort_expr": "study.study_name",
                "is_applicable": false,
                "is_default": false,
                "aggregate_type": "count",
                "aggregate_title": "Number of studies",
                "aggregate_facet_code": "result_facet",
                "tables": [
                    {
                        "sequence_id": 1,
                        "table_name": "study",
                        "udf_call_arguments": null,
                        "alias":  null
                    } ],
                "clauses": [
                    {
                        "clause": "bool_and((study.method_id % 2) = 0) ",
                        "enforce_constraint": true
                    } ]
            }
        ]

$facets$;

        j_facets = s_facets::jsonb;

        perform facet.create_or_update_facet(v.facet::jsonb)
        from jsonb_array_elements(j_facets) as v(facet);

    exception when sqlstate 'GUARD' then
        raise notice 'ALREADY EXECUTED';
    end;

end $$;
commit;
