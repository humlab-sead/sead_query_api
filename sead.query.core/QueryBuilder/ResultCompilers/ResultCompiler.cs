using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeadQueryCore.QueryBuilder;
using Autofac.Features.Indexed;
using SeadQueryCore.Model;

namespace SeadQueryCore
{

    public class ResultCompiler : QueryServiceBase, IResultCompiler
    {
        protected IIndex<string, IResultSqlQueryCompiler> QueryCompilers;

        public ResultCompiler(
            IRepositoryRegistry context,
            IQuerySetupCompiler builder,
            IIndex<string, IResultSqlQueryCompiler> queryCompilers
        ) : base(context, builder)
        {
            QueryCompilers = queryCompilers;
        }

        public string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode)
        {
            Facet facet = Registry.Facets.GetByCode(facetCode);
            ResultQuerySetup resultQuerySetup = CreateResultSetup(resultConfig);
            if (!resultQuerySetup.IsEmpty) {
                QuerySetup querySetup = QuerySetupBuilder.Build(facetsConfig, facet, resultQuerySetup.DataTables);
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