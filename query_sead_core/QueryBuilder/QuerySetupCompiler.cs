using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuerySeadDomain.QueryBuilder;
using Autofac.Features.Indexed;

namespace QuerySeadDomain {

    //public interface IQuerySetupCompilers {
    //    TabularQuerySetupCompiler DefaultQuerySetupCompiler { get; }
    //    MapQuerySetupCompiler MapQuerySetupCompiler { get; }
    //}

    public interface IResultQueryCompiler {
        string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode = null);
    }

    public class ResultQueryCompiler : QueryServiceBase, IResultQueryCompiler
    {
        protected IIndex<string, IResultSqlQueryCompiler> QueryCompilers;

        public ResultQueryCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IIndex<string, IResultSqlQueryCompiler> queryCompilers) : base(config, context, builder)
        {
            QueryCompilers = queryCompilers;
        }

        public string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode = "result_facet")
        {
            var resultFields = Context.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
            ResultQuerySetup resultQuerySetup = new ResultQuerySetup(resultFields);
            if (!resultQuerySetup.IsEmpty) {
                QuerySetup querySetup = QueryBuilder.Build(facetsConfig, facetCode, resultQuerySetup.DataTables);
                return QueryCompilers[resultConfig.ViewTypeId].Compile(querySetup, Context.Facets.GetByCode(facetCode), resultQuerySetup);
            }
            return "";
        }

        protected virtual IResultSqlQueryCompiler GetCompiler()
        {
            return new TabularResultSqlQueryBuilder();
        }
    }


    //public class MapQuerySetupCompiler : TabularQuerySetupCompiler
    //{

    //    public MapQuerySetupCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
    //    {
    //    }

    //    protected override IResultSqlQueryBuilder GetCompiler()
    //    {
    //        return new MapResultSqlQueryBuilder();
    //    }

    //}

    //public class MapQuerySetupCompiler : QuerySetupCompiler {

    //    public MapQuerySetupCompiler(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
    //    {
    //    }

    //    public override string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode)
    //    {
    //        // TODO Merge with DefaultQuerySetupCompiler, but use "AggregateKey" = "map_result" instead...?
    //        var resultFields = Context.Results.GetFieldsByKeys(resultConfig.AggregateKeys);
    //        ResultQuerySetup resultQuerySetup = new ResultQuerySetup(resultFields);
    //        QuerySetup query = QueryBuilder.Build(facetsConfig, facetCode);
    //        string sql = new MapResultSqlQueryBuilder().Compile(query, Context.Facets.GetByCode(facetCode), resultQuerySetup);
    //        return sql;
    //    }
    //}

}