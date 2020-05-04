using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Model.Ext;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class ResultConfigCompiler : QueryServiceBase, IResultConfigCompiler
    {
        protected IResultSqlCompilerLocator SqlCompilerLocator;

        public ResultConfigCompiler(
            IRepositoryRegistry repositoryRegistry,
            IQuerySetupBuilder builder,
            IResultSqlCompilerLocator sqlCompilerLocator
        ) : base(repositoryRegistry, builder)
        {
            SqlCompilerLocator = sqlCompilerLocator;
        }

        public string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string resultFacetCode)
        {
            if (resultConfig?.IsEmpty ?? true)
                throw new System.ArgumentNullException($"ResultConfig is null or is missing aggregate keys!");

            var resultFacet = Facets.GetByCode(resultFacetCode);

            /* Get the result fields for the requested aggregate keys */
            var fields = Results.GetFieldsByKeys(resultConfig.AggregateKeys);

            if (!fields.Any()) {
                throw new System.ArgumentException($"No result fields found for given {resultConfig.AggregateKeys[0]}!");
            }

            /* Setup query */
            QuerySetup querySetup = QuerySetupBuilder.Build(facetsConfig, resultFacet, fields);

            /* Compile query */
            return SqlCompilerLocator
                .Locate(resultConfig.ViewTypeId)
                    .Compile(querySetup, resultFacet, fields);
        }

        private IEnumerable<ResultAggregateField> GetResultAggregateFields(ResultConfig resultConfig)
        {
            var fields = Registry.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
            return fields;
        }
    }
}