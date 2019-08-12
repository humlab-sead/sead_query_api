using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeadQueryCore.QueryBuilder;
using Autofac.Features.Indexed;

namespace SeadQueryCore {
    //public interface IQuerySetupCompilers {
    //    TabularQuerySetupCompiler DefaultQuerySetupCompiler { get; }
    //    MapQuerySetupCompiler MapQuerySetupCompiler { get; }
    //}

    public interface IResultQueryCompiler {
        string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode);
    }

    public class ResultQueryCompiler : QueryServiceBase, IResultQueryCompiler
    {
        protected IIndex<string, IResultSqlQueryCompiler> QueryCompilers;

        public ResultQueryCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IIndex<string, IResultSqlQueryCompiler> queryCompilers) : base(config, context, builder)
        {
            QueryCompilers = queryCompilers;
        }

        public string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode)
        {
            var resultFields = Context.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
            ResultQuerySetup resultQuerySetup = new ResultQuerySetup(resultFields);
            if (!resultQuerySetup.IsEmpty) {
                QuerySetup querySetup = QueryBuilder.Build(facetsConfig, facetCode, resultQuerySetup.DataTables);
                return QueryCompilers[resultConfig.ViewTypeId].Compile(querySetup, Context.Facets.GetByCode(facetCode), resultQuerySetup);
            }
            return "";
        }
    }
}