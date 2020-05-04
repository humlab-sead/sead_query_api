using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class PicksFilterCompiler : IPicksFilterCompiler
    {
        public IPickFilterCompilerLocator PickCompilers { get; set; }

        public PicksFilterCompiler(IPickFilterCompilerLocator pickCompilers)
        {
            PickCompilers = pickCompilers;
        }

        /// <summary>
        /// Returns where-clauses based on user picks for all involved facets
        /// </summary>
        /// <param name="targetFacet"></param>
        /// <param name="involvedConfigs"></param>
        /// <returns></returns>
        public IEnumerable<string> Compile(Facet targetFacet, List<FacetConfig2> involvedConfigs)
        {
            var criterias = involvedConfigs
                .Select(
                    config => PickCompiler(config).Compile(targetFacet, config.Facet, config)
                 )
                .Where(
                    x => x.IsNotEmpty()
                );
            return criterias;
        }

        protected IPickFilterCompiler PickCompiler(FacetConfig2 c)
        {
            return PickCompilers.Locate(c.Facet.FacetTypeId);
        }
    }
}