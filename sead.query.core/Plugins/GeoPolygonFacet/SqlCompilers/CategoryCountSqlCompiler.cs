using System.Data;

namespace SeadQueryCore.Plugin;

public class GeoPolygonCategoryCountSqlCompiler : IGeoPolygonCategoryCountSqlCompiler
{
    public string Compile(QueryBuilder.QuerySetup query, Facet facet, CompilePayload payload)
    {
        // Note: Target must have longitude, latitude columns
        var dotName = query.Facet.TargetTable.ResolvedAliasOrTableOrUdfName;

        string sql = $@"
            SELECT {facet.CategoryIdExpr} AS category, 1 as count, {dotName}.longitude_dd, {dotName}.latitude_dd
            FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                    {query.Joins.Combine("\t\t\t\t\t\n")}
            WHERE 1 = 1
            {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
            GROUP BY {facet.CategoryIdExpr}, 3, 4
    ";
        return sql;
    }

    public CategoryItem ToItem(IDataReader dr)
    {
        return new CategoryItem()
        {
            Category = dr.Category2String(0),
            Count = 1,
            Extent = [dr.GetDecimal(2), dr.GetDecimal(3)],
            Name = dr.Category2String(0),
        };
    }

}
