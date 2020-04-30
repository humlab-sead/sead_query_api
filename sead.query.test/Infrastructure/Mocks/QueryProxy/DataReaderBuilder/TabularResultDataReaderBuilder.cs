using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoFixture;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder.Ext;

namespace SQT.Infrastructure
{

    public class TabularResultDataReaderBuilder : DataReaderBuilder
    {
        private readonly ResultAggregate ResultAggregate;

        public TabularResultDataReaderBuilder(ResultAggregate resultAggregate) : base("TabuleResult")
        {
            ResultAggregate = resultAggregate;
        }

        public override DataReaderBuilder CreateNewTable()
        {
            DataTable = new DataTable("CategoryCount");

            GetDataColumns().ForEach(column => DataTable.Columns.Add(column));

            return this;
        }

        private IEnumerable<(string Alias, ResultAggregateField Field)> GetAliasFieldPairs()
            => ResultAggregate
                .GetSortedFields()
                .GetAggregateAliasedFields()
                .Where(z => z.Field.FieldType.IsResultValue);

        public virtual IEnumerable<DataColumn> GetDataColumns()
            => GetAliasFieldPairs()
                .Select(z => new DataColumn(z.Alias, z.Field.ResultField.GetDataType()));

        /// <summary>
        /// Used when mocking ResultContentSet.Meta.Columns:
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ResultContentSet.ResultColumn> GetResultColumns()
            => GetAliasFieldPairs().Select(
                z => new ResultContentSet.ResultColumn
                {
                    DisplayText = z.Field.ResultField.DisplayText,
                    FieldKey = z.Field.ResultField.ResultFieldKey,
                    Type = z.Field.ResultField.GetDataTypeName()
                }
                ).ToList();
    }
}
