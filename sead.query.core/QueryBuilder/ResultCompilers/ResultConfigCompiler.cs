using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryBuilder.Ext;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{

    public class ResultConfigCompiler : QueryServiceBase, IResultConfigCompiler
    {
        protected IResultSqlCompilerLocator SqlCompilerLocator;

        public ResultConfigCompiler(
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            IResultSqlCompilerLocator sqlCompilerLocator
        ) : base(context, builder)
        {
            SqlCompilerLocator = sqlCompilerLocator;
        }

        public string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string resultFacetCode)
        {
            var resultFacet = Registry.Facets.GetByCode(resultFacetCode);

            var fields = GetResultAggregateFields(resultConfig);

            if (!fields.Any()) {
                return "";
            }

            QuerySetup querySetup = QuerySetupBuilder
                .Build(facetsConfig, resultFacet, fields.GetAggregateTableNames().ToList());

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