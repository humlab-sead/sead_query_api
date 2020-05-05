
using SeadQueryCore.Model;
using SeadQueryCore.Model.Ext;
using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.Services.Result
{
    public class DefaultResultService : QueryServiceBase, IResultService
    {
        public IResultSqlCompilerLocator SqlCompilerLocator { get; }
        public string ResultFacetCode { get; protected set; }
        public IDynamicQueryProxy QueryProxy { get; }

        public DefaultResultService(
            IRepositoryRegistry repositoryRegistry,
            IDynamicQueryProxy queryProxy,
            IQuerySetupBuilder builder,
            IResultSqlCompilerLocator sqlCompilerLocator
        ) : base(repositoryRegistry, builder)
        {
            SqlCompilerLocator = sqlCompilerLocator;
            ResultFacetCode = "result_facet";
            QueryProxy = queryProxy;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var resultFacet = Facets.GetByCode(ResultFacetCode);
            var queryFields = Results.GetFieldsByKeys(resultConfig.AggregateKeys);

            var querySetup = QuerySetupBuilder
                .Build(facetsConfig, resultFacet, queryFields);

            var sqlQuery = SqlCompilerLocator
                .Locate(resultConfig.ViewTypeId)
                    .Compile(querySetup, resultFacet, queryFields);

            return new TabularResultContentSet(
                resultConfig: resultConfig,
                resultFields: queryFields.GetResultValueFields().ToList(),
                reader: QueryProxy.Query(sqlQuery) /* This is (for now) only call to generic QueryProxy.Query */
            ) {
                Payload = GetExtraPayload(facetsConfig),
                Query = sqlQuery
            };
        }

        //public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        //{
        //    string sql = Compile(facetsConfig, resultConfig, ResultCode);

        //    if (sql.IsEmpty())
        //        return null;
            
        //    var valueFields = Results.GetFieldsByKeys(resultConfig.AggregateKeys).GetResultValueFields().ToList();
            
        //    /* This is (for now) only call to generic QueryProxy.Query */
        //    var reader = QueryProxy.Query(sql);

        //    var resultSet = new TabularResultContentSet(resultConfig, valueFields, reader) {
        //        Payload = GetExtraPayload(facetsConfig),
        //        Query = sql
        //    };

        //    return resultSet;
        //}

        //public string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string resultFacetCode)
        //{
        //    var resultFacet = Facets.GetByCode(resultFacetCode);

        //    /* Get the result fields for the requested aggregate keys */
        //    var fields = Results
        //        .GetFieldsByKeys(resultConfig.AggregateKeys);

        //    /* Setup query */
        //    QuerySetup querySetup = QuerySetupBuilder
        //        .Build(facetsConfig, resultFacet, fields);

        //    /* Compile query */
        //    return SqlCompilerLocator
        //        .Locate(resultConfig.ViewTypeId)
        //            .Compile(querySetup, resultFacet, fields);
        //}

        //protected virtual List<ResultAggregateField> GetResultValueFields(ResultConfig resultConfig)
        //{
        //    return Results.GetFieldsByKeys(resultConfig.AggregateKeys).GetResultValueFields().ToList();
        //}

        protected virtual dynamic GetExtraPayload(FacetsConfig2 facetsConfig)
        {
            // TODO Check if this really always should be null for tabular results
            return null;
        }
    }
}