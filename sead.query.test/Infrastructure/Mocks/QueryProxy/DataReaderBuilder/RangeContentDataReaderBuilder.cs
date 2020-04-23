using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SQT.Infrastructure
{
    public class RangeContentDataReaderBuilder : RangeCountDataReaderBuilder
    {
        public RangeContentDataReaderBuilder(decimal start = 0, decimal step = 1) : base(start, step)
        {
        }
    }
}
