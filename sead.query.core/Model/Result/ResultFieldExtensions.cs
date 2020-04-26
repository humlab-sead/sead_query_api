using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder.Ext
{
     public static class ResultFieldExtensions
    {
        public static IEnumerable<ResultField> GetAggregateResultFields(this IEnumerable<ResultAggregateField> fields)
             => fields.Select(z => z.ResultField);

        public static IEnumerable<string> GetAggregateTableNames(this IEnumerable<ResultAggregateField> fields)
            => fields.Select(z => z.ResultField.TableName).Where(t => t != null);

        public static IEnumerable<(string Alias, ResultAggregateField Field)> GetAggregateAliasedFields(this IEnumerable<ResultAggregateField> fields)
            => fields.Select((field, i) => ($"alias_{i + 1}", field));

        public static IEnumerable<string> GetAggregateInnerGroupByFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetAggregateAliasedFields().Select(p => p.Alias);

        public static IEnumerable<string> GetAggregateGroupByFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetAggregateAliasedFields().Where(z => z.Field.FieldType.IsGroupByField).Select(z => z.Alias);

        public static IEnumerable<(string ColumnName, string Alias)> GetAggregateColumnNameAliasPairs(this IEnumerable<ResultAggregateField> fields)
            => fields.GetAggregateAliasedFields().Select(z => ((z.Field.ResultField.ColumnName, z.Alias)));

        public static IEnumerable<string> GetAggregateSortFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetAggregateAliasedFields().Where(z => z.Field.FieldType.IsSortField).Select(z => z.Alias);

        public static IEnumerable<string> GetAggregateCompiledDataFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetAggregateAliasedFields()
                .Where(z => z.Field.FieldType.IsResultValue)
                    .Select(z => z.Field.FieldType.Compiler.Compile(z.Alias));
    }
}
