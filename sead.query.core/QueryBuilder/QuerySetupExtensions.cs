using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public static class QuerySetupExtensions
    {
        public static IEnumerable<Facet> Facets(this IEnumerable<FacetConfig2> configs)
            => configs.Select(c => c.Facet);

        public static IEnumerable<Facet> AddUnion(this IEnumerable<Facet> facets, Facet facet)
            => facets.Union(new List<Facet> { facet });

        public static IEnumerable<string> Criterias(this IEnumerable<Facet> facets)
            => facets.SelectMany(z => z.Criterias);


        public static IEnumerable<string> Criterias(this IEnumerable<FacetConfig2> configs)
            => configs.Facets().Criterias();

        public static IEnumerable<TableRelation> Edges(this IEnumerable<GraphRoute> routes)
            => routes.SelectMany(route => route.Items).OrderByDescending(z => z.TargetTable.IsUdf);

        public static IEnumerable<FacetTable> Tables(this IEnumerable<Facet> facets)
            => facets.SelectMany(z => z.Tables);

        public static IEnumerable<string> Names(this IEnumerable<FacetTable> facetTables)
           => facetTables.Select(z => z.ResolvedAliasOrTableOrUdfName);

        public static IEnumerable<string> TableNames(this IEnumerable<Facet> facets)
            => facets.Tables().Names();

    }
}