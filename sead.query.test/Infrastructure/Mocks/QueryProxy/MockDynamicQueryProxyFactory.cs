using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace SQT.Infrastructure
{
    public class MockDynamicQueryProxyFactory
    {
        //public Mock<IDynamicQueryProxy> Create<B, T>(int numberOfRows)
        //    where B: DataReaderBuilder, new()
        //    where T: class, new()
        //{
        //    var queryProxy = new Mock<IDynamicQueryProxy>();

        //    var dataReaderBuilder = new B().GenerateBogusRows(numberOfRows);

        //    queryProxy.Setup(foo => foo.Query(It.IsAny<string>())).Returns(
        //        dataReaderBuilder.ToDataReader()
        //    );

        //    return queryProxy;
        //}

        public Mock<IDynamicQueryProxy> Create<T>(IEnumerable<T> items)
            where T: class, new()
        {
            var queryProxy = new Mock<IDynamicQueryProxy>();

            queryProxy.Setup(foo => foo.Query(It.IsAny<string>())).Returns(
                items.ToDataReader<T>()
            );

            return queryProxy;
        }

        public Mock<IDynamicQueryProxy> Create(DataTable dt)
        {
            var queryProxy = new Mock<IDynamicQueryProxy>();

            queryProxy.Setup(foo => foo.Query(It.IsAny<string>())).Returns(
                dt.CreateDataReader()
            );

            return queryProxy;
        }
        public Mock<IDynamicQueryProxy> CreateWithData(List<(string Name, Type Type)> fields, object[,] values)
        {
            var dt = DataTableUtility.CreateDataTable(fields, values);
            return Create(dt);
        }

        public Mock<IDynamicQueryProxy> CreateWithFakeData(List<(string Name, Type Type)> fields, int rows)
        {
            var values = FakeDataUtility.GenerateRows(fields, rows);
            return CreateWithData(fields, values);
        }
    }
}
