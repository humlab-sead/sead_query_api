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
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupCompiler builder,
            IIndex<EFacetType, ICategoryCountService> countServices,
            IDiscreteContentSqlQueryCompiler sqlCompiler,
            ITypedQueryProxy queryProxy
            ) : base(config, context, builder, queryProxy)
        {
            CountService = countServices[EFacetType.Discrete];
            SqlCompiler = sqlCompiler;
        }

        public IDiscreteContentSqlQueryCompiler SqlCompiler { get; }

        protected override FacetContent.IntervalQueryInfo CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int count = 0)
        {
            var querySetup = QuerySetupBuilder.Build(facetsConfig, facetsConfig.TargetFacet, null, facetsConfig.GetFacetCodes());
            var sql = SqlCompiler.Compile(querySetup, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter());
            return new FacetContent.IntervalQueryInfo {
                Count = 1,
                Query = sql
            };
        }
    }
}