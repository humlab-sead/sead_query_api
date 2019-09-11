using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class ResultQuerySetup
    {
        public List<ResultAggregateField> Fields { get; set; }
        public List<ResultField> ResultFields => Fields.Select(z => z.ResultField).ToList();
        public List<string> DataTables => Fields.Select(z => z.ResultField.TableName).Where(t => t != null).ToList();

        public List<(string, string)> AliasPairs { get; set; }
        public List<string> DataFields { get; set; }
        public List<string> GroupByFields { get; set; }
        public List<string> InnerGroupByFields { get; set; }
        public List<string> SortFields { get; set; }

        public ResultQuerySetup(List<ResultAggregateField> fields)
        {
            var aliases = fields.Select((field, i) => new { Field = field, Alias = "alias_" + (i+1).ToString() });
            Fields = fields;
            InnerGroupByFields = aliases.Select(p => p.Alias).ToList();
            GroupByFields = aliases.Where(z => z.Field.FieldType.IsGroupByField).Select(z => z.Alias).ToList();
            AliasPairs = aliases.Select(z => ((z.Field.ResultField.ColumnName, z.Alias))).ToList();
            SortFields = aliases.Where(z => z.Field.FieldType.IsSortField).Select(z => z.Alias).ToList();
            DataFields = aliases.Where(z => z.Field.FieldType.IsResultValue).Select(z => z.Field.FieldType.Compiler.Compile(z.Alias)).ToList();
        }

        public bool IsEmpty => (Fields?.Count ?? 0) == 0;
    }
}
