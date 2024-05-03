using System;
using System.Collections.Generic;
using System.Data;

namespace SeadQueryCore
{
    public class RangeCategoryInfoSqlCompiler : IRangeCategoryInfoSqlCompiler
    {
        public virtual string Compile(QueryBuilder.QuerySetup query, Facet facet, dynamic payload)
        {
            TickerInfo tickerInfo = (TickerInfo)payload;
            var categoryType = facet?.CategoryIdType ?? "integer";
            string sql = $@"
            (
                SELECT n::text || ' to ' || (n + {tickerInfo.Interval})::{categoryType}::text, n, (n + {tickerInfo.Interval})::{categoryType}
                FROM generate_series({tickerInfo.OuterLow}::{categoryType}, {tickerInfo.OuterHigh}::{categoryType}, {tickerInfo.Interval}::{categoryType}) as a(n)
                WHERE n < {tickerInfo.OuterHigh}::{categoryType}
            )";

            return sql;
        }

        public CategoryItem ToItem(IDataReader dr)
        {
            return new CategoryItem()
            {
                Category = dr.GetString(0),
                Count = null,
                Extent = [dr.IsDBNull(1) ? 0 : dr.GetDecimal(1), dr.IsDBNull(2) ? 0 : dr.GetDecimal(2)],
                Name = dr.GetString(0),
            };
        }
    }
}
