namespace SeadQueryCore.QueryBuilder
{
    public class RangeFacetPickFilterCompiler : IPickFilterCompiler {
        public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
        {
            var picks = config.GetPickValues(true);
            return picks.Count != 2 ? ""
                : UtilitySqlCompiler.BetweenExpr(currentFacet.CategoryIdExpr, picks[0], picks[1])
                    .GlueIf(currentFacet.QueryCriteria, " AND ");
        }

        private string CompileSql(string expr, decimal lower, decimal upper)
        {
            return (lower == upper) ? $" (floor({expr}) = {lower})" : $" ({expr} >= {lower} and {expr} <= {upper})";
        }
    }
}
