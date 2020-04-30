using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.Kernel;

namespace SQT.Infrastructure
{
    public class DiscreteCountDataReaderBuilder : DataReaderBuilder
    {
        public DiscreteCountDataReaderBuilder() : base("CategoryCount")
        {
        }

        public override  DataReaderBuilder CreateNewTable()
        {
            DataTable = new DataTable("CategoryCount")
            {
                Columns =
                    {
                        new DataColumn("Category", typeof(string)),
                        new DataColumn("Name", typeof(string)),
                        new DataColumn("Count", typeof(int))
                    }
            };
            return this;
        }

        protected override object[] BogusRow(int rowNumber = 0)
        {
            var row = new object[3] {
                $"Category #{rowNumber}",
                $"Dummy #{rowNumber}",
                new SpecimenContext(fixture).Resolve(typeof(Int32))
            };
            return row;
        }
    }
}
