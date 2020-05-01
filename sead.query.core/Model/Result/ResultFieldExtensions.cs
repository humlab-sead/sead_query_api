using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder.Ext
{
     public static class ResultFieldExtensions
    {
        public static IEnumerable<ResultField> GetResultFields(this IEnumerable<ResultAggregateField> fields)
             => fields.Select(z => z.ResultField);

        public static IEnumerable<string> GetResultFieldsTableNames(this IEnumerable<ResultAggregateField> fields)
            => fields.Select(z => z.ResultField.TableName).Where(t => t != null);

        public static IEnumerable<(string Alias, ResultAggregateField Field)> GetResultAliasedFields(this IEnumerable<ResultAggregateField> fields)
            => fields.Select((field, i) => ($"alias_{i + 1}", field));

        public static IEnumerable<string> GetResultInnerGroupByFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetResultAliasedFields().Select(p => p.Alias);

        public static IEnumerable<string> GetResultGroupByFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetResultAliasedFields().Where(z => z.Field.FieldType.IsGroupByField).Select(z => z.Alias);

        public static IEnumerable<(string ColumnName, string Alias)> GetResultColumnNameAliasPairs(this IEnumerable<ResultAggregateField> fields)
            => fields.GetResultAliasedFields().Select(z => ((z.Field.ResultField.ColumnName, z.Alias)));

        public static IEnumerable<string> GetResultSortFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetResultAliasedFields().Where(z => z.Field.FieldType.IsSortField).Select(z => z.Alias);

        public static IEnumerable<string> GetResultCompiledDataFields(this IEnumerable<ResultAggregateField> fields)
            => fields.GetResultAliasedFields()
                .Where(z => z.Field.FieldType.IsResultValue)
                    .Select(z => z.Field.FieldType.Compiler.Compile(z.Alias));
    }
}
