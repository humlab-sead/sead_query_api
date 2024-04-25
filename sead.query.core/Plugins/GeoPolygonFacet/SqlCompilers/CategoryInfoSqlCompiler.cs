namespace SeadQueryCore.Plugin;

public class GeoPolygonCategoryInfoSqlCompiler : GeoPolygonCategoryCountSqlCompiler, IGeoPolygonCategoryInfoSqlCompiler
{
    public virtual string Compile(QueryBuilder.QuerySetup query, Facet facet, dynamic payload)
    {
        return base.Compile(query, facet, null);
    }
}
