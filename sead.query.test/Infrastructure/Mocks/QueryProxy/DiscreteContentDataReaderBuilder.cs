using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SeadQueryTest.Infrastructure
{
    public class DiscreteContentDataReaderBuilder : DataReaderBuilder
    {
        public DiscreteContentDataReaderBuilder() : base("CategoryCount")
        {
        }

        public override DataReaderBuilder CreateNewTable()
        {
            throw new NotImplementedException();
        }

    }
}
