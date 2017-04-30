using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryFacetDomain.QueryBuilder;

namespace QueryFacetDomain {

    public class QueryConfig {
        public List<string> group_by_fields = new List<string>();
        public List<string> group_by_inner_fields = new List<string>();
        public List<string> data_fields_alias = new List<string>();
        public List<string> data_tables = new List<string>();
        public List<string> sort_fields = new List<string>();
        public List<string> data_fields = new List<string>();
    }

    public abstract class QuerySetupCompiler : QueryServiceBase {

        public QuerySetupCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        public abstract string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode = null);

    }

    // FIXME: Name to DefaultQuerySetupCompiler
    public class ResultSqlQueryCompiler : QuerySetupCompiler { // OLD NAME: ResultSqlQueryCompiler

        public ResultSqlQueryCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        private QueryConfig createResultQueryConfig(ResultConfig resultConfig)
        {
            if (resultConfig.Items.Count == 0) {
                return null;
            }

            QueryConfig result = new QueryConfig();
            int alias_counter = 1;

            foreach (var aggregateKey in resultConfig.Items.Where(z => z != "")) {

                foreach (var item in Context.Results.GetByKey(aggregateKey).Fields) {

                    var resultField = item.ResultField;

                    SqlFieldCompiler fieldCompiler = SqlFieldCompiler.getCompiler(resultField.ResultType);

                    string alias_name = "alias_" + (alias_counter++).ToString();

                    result.data_fields_alias.Add($"{resultField.ColumnName} AS {alias_name}");
                    result.data_tables.Add(resultField.TableName);
                    result.group_by_inner_fields.Add(alias_name);

                    if (SqlFieldCompiler.isFieldType(resultField.ResultType))
                        result.data_fields.Add(fieldCompiler.compile(alias_name));

                    if (SqlFieldCompiler.isGroupByType(resultField.ResultType))
                        result.group_by_fields.Add(alias_name);

                    if (SqlFieldCompiler.isSortType(resultField.ResultType))
                        result.sort_fields.Add(alias_name);
                }
            }
            return result;
        }

        public override string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode = "result_facet")
        {
            QueryConfig queryConfig = createResultQueryConfig(resultConfig);

            if (queryConfig == null || queryConfig.data_fields.Count == 0) {
                return "";
            }
            QueryBuilder.QuerySetup query = QueryBuilder.Build(facetsConfig, facetCode, queryConfig.data_tables);
            string sql = ResultSqlQueryBuilder.compile(query, null, queryConfig);
            return sql;
        }
    }

    // FIXME: Name to MapQuerySetupCompiler
    class MapResultSqlQueryCompiler : QuerySetupCompiler { // OLD NAME: MapResultSqlQueryCompiler
        public MapResultSqlQueryCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        public override string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode)
        {
            QueryBuilder.QuerySetup query = QueryBuilder.Build(facetsConfig, facetCode);
            string sql = MapResultSqlQueryBuilder.compile(query, Context.Facets.GetByCode(facetCode));
            return sql;
        }
    }

}