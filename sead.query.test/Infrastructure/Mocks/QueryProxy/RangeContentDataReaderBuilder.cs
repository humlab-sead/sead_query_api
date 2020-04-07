using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SeadQueryTest.Infrastructure
{
    public class RangeContentDataReaderBuilder : DataReaderBuilder
    {
        public RangeContentDataReaderBuilder() : base("CategoryCount")
        {
        }

        public override DataReaderBuilder CreateNewTable()
        {
            throw new NotImplementedException();
        }

    }
}
