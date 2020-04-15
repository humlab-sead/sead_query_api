using Autofac.Features.Indexed;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public class ResultQueryCompiler : QueryServiceBase, IResultQueryCompiler
    {
        protected IIndex<string, IResultQuerySetupSqlCompiler> QueryCompilers;

        public ResultQueryCompiler(
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            IIndex<string, IResultQuerySetupSqlCompiler> queryCompilers
        ) : base(context, builder)
        {
            QueryCompilers = queryCompilers;
        }

        public string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode)
        {
            Facet facet = Registry.Facets.GetByCode(facetCode);
            ResultQuerySetup resultQuerySetup = CreateResultSetup(resultConfig);
            if (!resultQuerySetup.IsEmpty) {
                QuerySetup querySetup = QuerySetupBuilder.Build(facetsConfig, facet, resultQuerySetup.TableNames);
                return QueryCompilers[resultConfig.ViewTypeId].Compile(querySetup, facet, resultQuerySetup);
            }
            return "";
        }

        private ResultQuerySetup CreateResultSetup(ResultConfig resultConfig)
        {
            var resultFields = Registry.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
            ResultQuerySetup resultQuerySetup = new ResultQuerySetup(resultFields);
            return resultQuerySetup;
        }
    }
}