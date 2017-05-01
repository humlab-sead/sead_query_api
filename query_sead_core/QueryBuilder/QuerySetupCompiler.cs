using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryFacetDomain.QueryBuilder;

namespace QueryFacetDomain {

    public class QueryConfig {
        public List<string> GroupByFields = new List<string>();
        public List<string> InnerGroupByFields = new List<string>();
        public List<string> DataFieldAliases = new List<string>();
        public List<string> DataTables = new List<string>();
        public List<string> SortFields = new List<string>();
        public List<string> DataFields = new List<string>();
    }

    public interface IQuerySetupCompilers {
        IQuerySetupCompiler DefaultQuerySetupCompiler { get; }
        IQuerySetupCompiler MapQuerySetupCompiler { get; }
    }

    public interface IQuerySetupCompiler {

        string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode = null);

    }

    public abstract class QuerySetupCompiler : QueryServiceBase, IQuerySetupCompiler {

        public QuerySetupCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        public abstract string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode = null);

    }

    public class DefaultQuerySetupCompiler : QuerySetupCompiler { // OLD NAME: ResultSqlQueryCompiler

        public DefaultQuerySetupCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        public override string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode = "result_facet")
        {
            QueryConfig queryConfig = createResultQueryConfig(resultConfig);

            if (queryConfig == null || queryConfig.DataFields.Count == 0) {
                return "";
            }
            QueryBuilder.QuerySetup query = QueryBuilder.Build(facetsConfig, facetCode, queryConfig.DataTables);
            string sql = ResultSqlQueryBuilder.compile(query, null, queryConfig);
            return sql;
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

                    result.DataFieldAliases.Add($"{resultField.ColumnName} AS {alias_name}");
                    result.DataTables.Add(resultField.TableName);
                    result.InnerGroupByFields.Add(alias_name);

                    if (SqlFieldCompiler.isFieldType(resultField.ResultType))
                        result.DataFields.Add(fieldCompiler.compile(alias_name));

                    if (SqlFieldCompiler.isGroupByType(resultField.ResultType))
                        result.GroupByFields.Add(alias_name);

                    if (SqlFieldCompiler.isSortType(resultField.ResultType))
                        result.SortFields.Add(alias_name);
                }
            }
            return result;
        }

    }

    public class MapQuerySetupCompiler : QuerySetupCompiler { // OLD NAME: MapResultSqlQueryCompiler
        public MapQuerySetupCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
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