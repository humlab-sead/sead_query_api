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

select *
from facet.facet
where facet_id = 3


        SELECT x.facet_id, x.aggregate_facet_id, x.aggregate_title, x.aggregate_type, x.category_id_expr, x.category_name_expr, x.display_title, x.facet_code, x.facet_group_id, x.facet_type_id, x.icon_id_expr, x.is_applicable, x.is_default, x.sort_expr, "x.FacetType".facet_type_id, "x.FacetType".facet_type_name, "x.FacetType".reload_as_target, "x.FacetGroup".facet_group_id, "x.FacetGroup".display_title, "x.FacetGroup".facet_group_key, "x.FacetGroup".is_applicable, "x.FacetGroup".is_default
FROM facet.facet AS x
INNER JOIN facet.facet_type AS "x.FacetType" ON x.facet_type_id = "x.FacetType".facet_type_id
INNER JOIN facet.facet_group AS "x.FacetGroup" ON x.facet_group_id = "x.FacetGroup".facet_group_id
ORDER BY x.facet_id
TRACE [11766] Cleaning up reader
TRACE [11766] End user action
TRACE [11766] Start user action
DEBUG [11766] Executing statement(s):
        SELECT "x.Tables".facet_table_id, "x.Tables".alias, "x.Tables".facet_id, "x.Tables".udf_call_arguments, "x.Tables".table_or_udf_name, "x.Tables".schema_name, "x.Tables".sequence_id
FROM facet.facet_table AS "x.Tables"
INNER JOIN (
    SELECT DISTINCT x0.facet_id
    FROM facet.facet AS x0
    INNER JOIN facet.facet_type AS "x.FacetType0" ON x0.facet_type_id = "x.FacetType0".facet_type_id
    INNER JOIN facet.facet_group AS "x.FacetGroup0" ON x0.facet_group_id = "x.FacetGroup0".facet_group_id
) AS t ON "x.Tables".facet_id = t.facet_id
ORDER BY t.facet_id
TRACE [11766] Cleaning up reader
TRACE [11766] End user action
TRACE [11766] Start user action
DEBUG [11766] Executing statement(s):
        SELECT "x.Clauses".facet_source_table_id, "x.Clauses".clause, "x.Clauses".facet_id
FROM facet.facet_clause AS "x.Clauses"
INNER JOIN (
    SELECT DISTINCT x1.facet_id
    FROM facet.facet AS x1
    INNER JOIN facet.facet_type AS "x.FacetType1" ON x1.facet_type_id = "x.FacetType1".facet_type_id
    INNER JOIN facet.facet_group AS "x.FacetGroup1" ON x1.facet_group_id = "x.FacetGroup1".facet_group_id
) AS t0 ON "x.Clauses".facet_id = t0.facet_id
ORDER BY t0.facet_id
TRACE [11766] Cleaning up reader
TRACE [11766] End user action
TRACE [11766] Closing connection...
TRACE [11766] Closing connector
TRACE [11766] Cleaning up connector
DEBUG [11766] Connection closed
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SeadQueryAPI.Controllers.FacetsController.Load (sead.query.api) with arguments () - Validation state: Invalid
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SeadQueryAPI.Controllers.FacetsController.Load (sead.query.api) in 49.7488ms
fail: Microsoft.AspNetCore.Server.Kestrel[13]
      Connection id "0HLQM6GDV9IS5", Request id "0HLQM6GDV9IS5:00000001": An unhandled exception was thrown by the application.
System.NullReferenceException: Object reference not set to an instance of an object.
   at SeadQueryAPI.Serializers.FacetConfigReconstituteService.Reconstitute(FacetsConfig2 facetsConfig) in /src/sead.query.api/Serializers/FacetConfigReconstituteService.cs:line 39
   at SeadQueryAPI.Controllers.FacetsController.Load(FacetsConfig2 facetsConfig) in /src/sead.query.api/Controllers/FacetsController.cs:line 79
   at lambda_method(Closure , Object , Object[] )
   at Microsoft.AspNetCore.Mvc.Internal.ActionMethodExecutor.SyncObjectResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeActionMethodAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeNextActionFilterAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Rethrow(ActionExecutedContext context)
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeInnerFilterAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeNextResourceFilter()
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Rethrow(ResourceExecutedContext context)
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeFilterPipelineAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeAsync()
   at Microsoft.AspNetCore.Builder.RouterMiddleware.Invoke(HttpContext httpContext)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpProtocol.ProcessRequests[TContext](IHttpApplication`1 application)
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 51.3241ms 500

