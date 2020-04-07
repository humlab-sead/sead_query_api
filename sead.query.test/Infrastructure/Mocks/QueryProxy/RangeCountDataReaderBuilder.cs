using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoFixture;

namespace SeadQueryTest.Infrastructure
{
    public class RangeCountDataReaderBuilder : DataReaderBuilder
    {
        private readonly decimal step;
        private readonly decimal start;

        public RangeCountDataReaderBuilder(decimal start = 0, decimal step = 1) : base("CategoryCount")
        {
            this.start = start;
            this.step = step;
        }

        public override DataReaderBuilder CreateNewTable()
        {
            dataTable = new DataTable("CategoryCount")
            {
                Columns =
                    {
                        new DataColumn("Category", typeof(string)),
                        new DataColumn("Lower", typeof(decimal)),
                        new DataColumn("Upper", typeof(decimal)),
                        new DataColumn("Count", typeof(int))
                    }
            };
            return this;
        }

        public override DataReaderBuilder GenerateBogusRows(int numberOfRows = 3)
        {
            var lower = start;
            var upper = step;
            for (var i = 0; i < numberOfRows; i++, lower += step, upper += step) {
                AddRow(new object[] {
                    $"{lower}-{upper}",
                    lower,
                    upper,
                    fixture.Create<UInt16>()
                }) ;
            }
            return this;
        }
    }
}
