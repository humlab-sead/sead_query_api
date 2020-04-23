using AutoFixture;
using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace SQT.Infrastructure
{
    public static class FakeDataUtility
    {
        public static object[,] GenerateRows(List<(string Name, Type Type)> fields, int nRows)
        {
            AutoFixture.Fixture fixture = new AutoFixture.Fixture();
            var ctx = new AutoFixture.Kernel.SpecimenContext(fixture);

            var values = new object[nRows, fields.Count];
            for (var i = 0; i < nRows; i++)
                for (var j = 0; j < fields.Count; j++)
                    values[i, j] = ctx.Resolve(fields[j].Type);
            return values;
        }

        public static object[] GenerateRow(List<(string Name, Type Type)> fields)
        {
            var ctx = GetSpecimenContext();
            var values = new object[fields.Count];
            for (var j = 0; j < fields.Count; j++)
                values[j] = ctx.Resolve(fields[j].Type);
            return values;
        }

        private static AutoFixture.Kernel.SpecimenContext GetSpecimenContext()
        {
            AutoFixture.Fixture fixture = new AutoFixture.Fixture();
            var ctx = new AutoFixture.Kernel.SpecimenContext(fixture);
            return ctx;
        }
    }
}
