using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using SQT.Infrastructure;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class TestHostFixture<T> : IDisposable where T: class, new()
    {
        public IHostBuilder Builder;
        public Task<IHost> Server;
        public HttpClient Client;
        public TestHostFixture()
        {
            Builder = new TestHostBuilder().Create<T>("apa");
            Server = Builder.StartAsync();
            Client = Server.Result.GetTestClient();
        }

        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }
    }
}
