using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SeadQueryTest.Infrastructure
{

    public class MockQueryProxyFactory
    {
        public Mock<IDatabaseQueryProxy> Create<B, T>(int numberOfRows)
            where B: DataReaderBuilder, new()
            where T: class, new()
        {
            var queryProxy = new Mock<IDatabaseQueryProxy>();

            var dataReaderBuilder = new B().GenerateBogusRows(numberOfRows);

            queryProxy.Setup(foo => foo.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, T>>())).Returns(
                dataReaderBuilder.ToItems<T>().ToList()
            );
            queryProxy.Setup(foo => foo.Query(It.IsAny<string>())).Returns(
                dataReaderBuilder.ToDataReader()
            );

            return queryProxy;
        }

        public Mock<IDatabaseQueryProxy> Create<T>(IEnumerable<T> items)
            where T: class, new()
        {
            var queryProxy = new Mock<IDatabaseQueryProxy>();

            queryProxy.Setup(foo => foo.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, T>>())).Returns(
                items.ToList()
            );

            queryProxy.Setup(foo => foo.Query(It.IsAny<string>())).Returns(
                items.ToDataReader<T>()
            );

            return queryProxy;
        }
    }
}
