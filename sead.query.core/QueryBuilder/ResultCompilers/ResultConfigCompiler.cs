using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

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

            var resultQuerySetup = CreateResultSetup(resultConfig);

            if (!resultQuerySetup.IsEmpty) {

                QuerySetup querySetup = QuerySetupBuilder
                    .Build(facetsConfig, resultFacet, resultQuerySetup.TableNames);

                return SqlCompilerLocator
                    .Locate(resultConfig.ViewTypeId)
                    .Compile(querySetup, resultFacet, resultQuerySetup);
            }
            return "";
        }

        private ResultQuerySetup CreateResultSetup(ResultConfig resultConfig)
        {
            var resultFields = Registry.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
            var resultQuerySetup = new ResultQuerySetup(resultFields);
            return resultQuerySetup;
        }
    }
}