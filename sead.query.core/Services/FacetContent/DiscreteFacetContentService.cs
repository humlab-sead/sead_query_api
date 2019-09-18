using Autofac.Features.Indexed;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace SeadQueryCore
{
    public class DiscreteFacetContentService : FacetContentService {
        public DiscreteFacetContentService(
            IQueryBuilderSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            IIndex<EFacetType, ICategoryCountService> countServices,
            IDiscreteContentSqlQueryCompiler sqlBuilder
            ) : base(config, context, builder)
        {
            CountService = countServices[EFacetType.Discrete];
            SqlBuilder = sqlBuilder;
        }

        public IDiscreteContentSqlQueryCompiler SqlBuilder { get; }

        protected override (int, string) CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int count=0)
        {
            QuerySetup query = QuerySetupBuilder.Build(facetsConfig, facetsConfig.TargetCode, null, facetsConfig.GetFacetCodes());
            string sql = SqlBuilder.Compile(query, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter());
            Debug.Print($"{facetCode}: {sql}");
            return ( 1, sql );
        }
    }
}