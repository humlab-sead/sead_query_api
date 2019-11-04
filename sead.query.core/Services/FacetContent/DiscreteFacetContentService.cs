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
            IDiscreteContentSqlQueryCompiler sqlCompiler
            ) : base(config, context, builder)
        {
            CountService = countServices[EFacetType.Discrete];
            SqlCompiler = sqlCompiler;
        }

        public IDiscreteContentSqlQueryCompiler SqlCompiler { get; }

        protected override FacetContent.IntervalQueryInfo CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int count = 0)
        {
            QuerySetup query = QuerySetupBuilder.Build(facetsConfig, facetsConfig.TargetFacet, null, facetsConfig.GetFacetCodes());
            string sql = SqlCompiler.Compile(query, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter());
            Debug.Print($"{facetCode}: {sql}");
            return new FacetContent.IntervalQueryInfo {
                Count = 1,
                Query = sql
            };
        }
    }
}