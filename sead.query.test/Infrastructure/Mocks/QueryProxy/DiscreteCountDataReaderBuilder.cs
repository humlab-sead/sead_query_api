using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoFixture;

namespace SeadQueryTest.Infrastructure
{
    public class DiscreteCountDataReaderBuilder : DataReaderBuilder
    {
        public DiscreteCountDataReaderBuilder() : base("CategoryCount")
        {
        }

        public override  DataReaderBuilder CreateNewTable()
        {
            dataTable = new DataTable("CategoryCount")
            {
                Columns =
                    {
                        new DataColumn("Category", typeof(string)),
                        new DataColumn("Count", typeof(int))
                    }
            };
            return this;
        }

        public override DataReaderBuilder GenerateBogusRows(int numberOfRows = 3)
        {
            for (var i = 0; i < numberOfRows; i++) {
                AddRow(new object[] {
                    $"Category #{i}",
                    fixture.Create<UInt16>()
                });
            }
            return this;
        }
    }
}
