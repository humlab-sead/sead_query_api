using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SeadQueryTest.Infrastructure
{

    public class MockTypedQueryProxyFactory
    {
        public Mock<ITypedQueryProxy> Create<B, T>(int numberOfRows)
            where B : DataReaderBuilder, new()
            where T : class, new()
        {
            var queryProxy = new Mock<ITypedQueryProxy>();

            var dataReaderBuilder = new B().GenerateBogusRows(numberOfRows);

            queryProxy.Setup(foo => foo.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, T>>())).Returns(
                dataReaderBuilder.ToItems<T>().ToList()
            );

            return queryProxy;
        }

        public Mock<ITypedQueryProxy> Create<T>(IEnumerable<T> items)
            where T : class, new()
        {
            var queryProxy = new Mock<ITypedQueryProxy>();

            queryProxy.Setup(foo => foo.QueryRows(It.IsAny<string>(), It.IsAny<Func<IDataReader, T>>())).Returns(
                items.ToList()
            );

            return queryProxy;
        }
    }
}
