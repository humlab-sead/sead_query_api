using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoFixture;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Model.Ext;

namespace SQT.Infrastructure
{

    public class TabularResultDataReaderBuilder : DataReaderBuilder
    {
        private readonly ResultSpecification ResultSpecification;

        public TabularResultDataReaderBuilder(ResultSpecification resultSpecification) : base("TabuleResult")
        {
            ResultSpecification = resultSpecification;
        }

        public override DataReaderBuilder CreateNewTable()
        {
            DataTable = new DataTable("CategoryCount");

            GetDataColumns().ForEach(column => DataTable.Columns.Add(column));

            return this;
        }

        private IEnumerable<(string Alias, ResultSpecificationField Field)> GetAliasFieldPairs()
            => ResultSpecification
                .GetSortedFields()
                .GetResultAliasedFields()
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
