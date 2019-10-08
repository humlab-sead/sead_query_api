using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Npgsql;
using SeadQueryCore;

namespace SeadQueryTest
{
    internal class MockIndex<T, T1> : Dictionary<T, T1>, IIndex<T, T1>
    {
    }

    /// <summary>
    /// IOptionsBuilder
    /// </summary>
    public class MockOptionBuilder
    {

        private readonly QueryBuilderSetting defaultOptions;

        public MockOptionBuilder(Dictionary<string, string> memorySettings=null)
        {
            defaultOptions = GetSettings(memorySettings);
        }
        public FacetSetting  DefaultFacetSettings()
        {
            return new FacetSetting() {
                CategoryNameFilter = true,
                DirectCountColumn = "tbl_analysis_entities.analysis_entity_id",
                DirectCountTable = "tbl_analysis_entities",
                IndirectCountColumn = "tbl_dating_periods.dating_period_id",
                IndirectCountTable = "tbl_dating_periods",
                ResultQueryLimit = 10000
            };
        }

        public IQueryBuilderSetting DefaultQueryBuilderSettings()
        {
            return new QueryBuilderSetting() {
                Facet = DefaultFacetSettings(),
                Store = GetSettings().Store
            };
        }

        public QueryBuilderSetting GetSettings(Dictionary<string, string> memorySettings = null)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(memorySettings ?? new Dictionary<string, string>())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build()
                .GetSection("QueryBuilderSetting")
                .Get<QueryBuilderSetting>();
            return config;
        }

        public IOptions<QueryBuilderSetting> Build()
        {
            var options = new Mock<IOptions<QueryBuilderSetting>>();
            options.Setup(o => o.Value).Returns(defaultOptions);
            return options.Object;
        }

        public MockOptionBuilder WithQueryLimit(int queryLimit)
        {
            defaultOptions.Facet.ResultQueryLimit = queryLimit;
            return this;
        }

        public MockOptionBuilder WithRedisCache()
        {
            defaultOptions.Store.UseRedisCache = true;
            return this;
        }

        public MockOptionBuilder WithDataServer(string host, int port=5432)
        {
            defaultOptions.Store.Host = host;
            defaultOptions.Store.Port = port.ToString();
            return this;
        }

        public MockOptionBuilder WithCredentials(string userName, string password)
        {
            defaultOptions.Store.Username = userName;
            defaultOptions.Store.Password = password;
            return this;
        }

        //public MockOptionBuilder WithEnvCredentials(string userNameEnvVar, string passwordEnvVar)
        //{
        //    var userName = "";
        //    var password = "";
        //    return this.WithCredentials(userName, password);
        //}
    }
}
