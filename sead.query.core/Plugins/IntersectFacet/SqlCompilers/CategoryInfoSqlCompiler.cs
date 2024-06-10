using System.Data;

namespace SeadQueryCore.Plugin.Intersect;

public class IntersectCategoryInfoSqlCompiler : IIntersectCategoryInfoSqlCompiler
{
    // public IntersectCategoryInfoSqlCompiler(ITypedQueryProxy queryProxy) {
    //     QueryProxy = queryProxy;
    // }

    // public ITypedQueryProxy QueryProxy { get; }

    public virtual string Compile(QueryBuilder.QuerySetup query, Facet facet, dynamic payload)
    {
        TickerInfo tickerInfo = (TickerInfo)payload;
        var categoryType = facet?.CategoryIdType ?? "int4range";
        var precision = categoryType.StartsWith("int") ? 0 : tickerInfo.Precision;
        var rangeType = categoryType.StartsWith("int") ? "integer" : $"decimal(20, {precision})";

        // FIXME: return lower, upper as well!
        string sql = $@"
            SELECT n::text || ' to ' || (n + {tickerInfo.Interval}::{rangeType})::text, {categoryType}(n, (n + {tickerInfo.Interval}::{rangeType})), n as lower, (n + {tickerInfo.Interval}::{rangeType}) as upper
            FROM generate_series({tickerInfo.OuterLow}::{rangeType}, {tickerInfo.OuterHigh}::{rangeType}, {tickerInfo.Interval}::{rangeType}) as a(n)
            WHERE n < {tickerInfo.OuterHigh}
        ";

        return sql.Trim();
    }

    public CategoryItem ToItem(IDataReader dr)
    {
        // var range = QueryProxy.GetRange<decimal>(dr, 1);
        var lower = dr.GetDecimal(2);
        var upper = dr.GetDecimal(3);
        return new CategoryItem()
        {
            Category = dr.GetString(0),
            Count = null,
            Extent = [lower, upper],
            Name = dr.GetString(0),
        };
    }

    // private (decimal, decimal) GetRange(IDataReader dr, int index)
    // {
    //     // FIXME Read using dr.GetFieldValue<NpgsqlRange<T>>(0);
    //     // FIXME Add dependency to Npgsql in core.
    //     string value = dr.GetString(index);
    //     if (value.Equals("empty"))
    //         return (0, 0);
    //     value = value.Trim('[', ')');
    //     var bounds = value.Split(',');
    //     var lower = decimal.Parse(bounds[0]);
    //     var upper = decimal.Parse(bounds[1]) - 1;
    //     return (lower, upper);
    // }

}
