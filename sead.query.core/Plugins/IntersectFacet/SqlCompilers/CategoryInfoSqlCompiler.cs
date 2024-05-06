using System.Data;

namespace SeadQueryCore.Plugin.Intersect;

public class IntersectCategoryInfoSqlCompiler : IIntersectCategoryInfoSqlCompiler
{
    public virtual string Compile(QueryBuilder.QuerySetup query, Facet facet, dynamic payload)
    {
        TickerInfo tickerInfo = (TickerInfo)payload;
        var categoryType = facet?.CategoryIdType ?? "int4range";
        string sql = $@"
        (
            SELECT n::text || ' to ' || (n + {tickerInfo.Interval})::{categoryType}::text, {categoryType}(n, (n + {tickerInfo.Interval}))
            FROM generate_series({tickerInfo.OuterLow}::{categoryType}, {tickerInfo.OuterHigh}::{categoryType}, {tickerInfo.Interval}::{categoryType}) as a(n)
            WHERE n < {tickerInfo.OuterHigh}::{categoryType}
        )";

        return sql;
    }

    public CategoryItem ToItem(IDataReader dr)
    {
        var range = GetRange(dr, 1);

        return new CategoryItem()
        {
            Category = dr.GetString(0),
            Count = null,
            Extent = [range.Item1, range.Item2],
            Name = dr.GetString(0),
        };
    }

    private (decimal, decimal) GetRange(IDataReader dr, int index)
    {
        // FIXME Read using dr.GetFieldValue<NpgsqlRange<T>>(0);
        // FIXME Add dependency to Npgsql in core.
        string value = dr.GetString(index);
        if (value.Equals("empty"))
            return (0, 0);
        value = value.Trim('[', ')');
        var bounds = value.Split(',');
        var lower = decimal.Parse(bounds[0]);
        var upper = decimal.Parse(bounds[1]) - 1;
        return (lower, upper);
    }

}
