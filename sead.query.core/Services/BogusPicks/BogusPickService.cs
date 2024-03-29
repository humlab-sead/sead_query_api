using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class BogusPickService : QueryServiceBase, IBogusPickService
    {
        public BogusPickService(
            IRepositoryRegistry registry,
            IQuerySetupBuilder builder,
            IValidPicksSqlCompiler picksCompiler,
            ITypedQueryProxy queryProxy
        ) : base(registry, builder)
        {
            PicksCompiler = picksCompiler;
            QueryProxy = queryProxy;
        }

        public IValidPicksSqlCompiler PicksCompiler { get; }
        public ITypedQueryProxy QueryProxy { get; }

        /// <summary>
        /// Removes invalid selections e.g. hidden selections still being sent from the client.
        /// The client keep them since they can be visible when the filters changes.
        /// This is only applicable for discrete facets (range facet selection are always visible)
        /// </summary>
        /// <param name="facetsConfig"></param>
        /// <returns></returns>
        public FacetsConfig2 Update(FacetsConfig2 facetsConfig)
        {
            foreach (string facetCode in facetsConfig.GetFacetCodes())
            {
                var config = facetsConfig.GetConfig(facetCode);

                if (config.Facet.FacetTypeId != EFacetType.Discrete || config.Picks.Count == 0)
                {
                    continue;
                }

                if (!config.HasPicks())
                {
                    continue;
                }

                config.Picks = QueryProxy.QueryRows(
                    PicksCompiler.Compile(
                        QuerySetupBuilder.Build(facetsConfig, config.Facet, null, null),
                        config.GetIntegerPickValues()
                    ),
                    x => new FacetConfigPick(x.GetString(0), x.GetString(1))
                );
            }
            return facetsConfig;
        }
    }
}
