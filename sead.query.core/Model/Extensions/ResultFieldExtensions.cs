﻿using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.Model.Ext
{
    public static class ResultFieldExtensions
    {
        public static IEnumerable<ResultField> GetResultFields(this IEnumerable<ResultSpecificationField> fields)
             => fields.Select(z => z.ResultField);

        public static IEnumerable<string> GetResultFieldTableNames(this IEnumerable<ResultSpecificationField> fields)
            => fields.Select(z => z.ResultField.TableName).Where(t => t != null).Distinct();

        public static IEnumerable<(string Alias, ResultSpecificationField Field)> GetResultAliasedFields(this IEnumerable<ResultSpecificationField> fields)
            => fields.Select((field, i) => ($"alias_{i + 1}", field));

        public static IEnumerable<string> GetResultInnerGroupByFields(this IEnumerable<ResultSpecificationField> fields)
            => fields.GetResultAliasedFields().Select(p => p.Alias);

        public static IEnumerable<string> GetResultGroupByFields(this IEnumerable<ResultSpecificationField> fields)
            => fields.GetResultAliasedFields().Where(z => z.Field.FieldType.IsGroupByField).Select(z => z.Alias);

        public static IEnumerable<(string ColumnName, string Alias)> GetResultColumnNameAliasPairs(this IEnumerable<ResultSpecificationField> fields)
            => fields.GetResultAliasedFields().Select(z => ((z.Field.ResultField.ColumnName, z.Alias)));

        public static IEnumerable<string> GetResultSortFields(this IEnumerable<ResultSpecificationField> fields)
            => fields.GetResultAliasedFields().Where(z => z.Field.FieldType.IsSortField).Select(z => z.Alias);

        public static IEnumerable<ResultSpecificationField> GetResultValueFields(this IEnumerable<ResultSpecificationField> fields)
            => fields.Where(z => z.FieldType.IsResultValue);

        public static IEnumerable<string> GetResultCompiledValueFields(this IEnumerable<ResultSpecificationField> fields)
            => fields.GetResultAliasedFields()
                .Where(z => z.Field.FieldType.IsResultValue)
                    .Select(z => z.Field.FieldType.Compiler.Compile(z.Alias));
    }
}
