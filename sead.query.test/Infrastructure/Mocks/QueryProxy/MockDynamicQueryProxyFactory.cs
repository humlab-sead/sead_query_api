using Moq;
using SeadQueryCore;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure
{
    public class MockDynamicQueryProxyFactory
    {
        public Mock<IDynamicQueryProxy> Create<B, T>(int numberOfRows)
            where B: DataReaderBuilder, new()
            where T: class, new()
        {
            var queryProxy = new Mock<IDynamicQueryProxy>();

            var dataReaderBuilder = new B().GenerateBogusRows(numberOfRows);

            queryProxy.Setup(foo => foo.Query(It.IsAny<string>())).Returns(
                dataReaderBuilder.ToDataReader()
            );

            return queryProxy;
        }

        public Mock<IDynamicQueryProxy> Create<T>(IEnumerable<T> items)
            where T: class, new()
        {
            var queryProxy = new Mock<IDynamicQueryProxy>();

            queryProxy.Setup(foo => foo.Query(It.IsAny<string>())).Returns(
                items.ToDataReader<T>()
            );

            return queryProxy;
        }
    }
}
