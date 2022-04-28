using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using SeadQueryCore;
using SeadQueryInfra;

namespace SQT
{
    internal class MockIndex<T, T1> : Dictionary<T, T1>, IIndex<T, T1>
    {
    }

    /// <summary>
    /// IOptionsBuilder
    /// </summary>
    internal class SettingFactory
    {
        private readonly Setting defaultOptions;

        public SettingFactory(Dictionary<string, string> memorySettings = null)
        {
            defaultOptions = GetSettings(memorySettings);
        }
        public FacetSetting DefaultFacetSettings()
        {
            return new FacetSetting()
            {
                CountColumn = "tbl_analysis_entities.analysis_entity_id",
                CountTable = "tbl_analysis_entities"
            };
        }

        public ISetting DefaultQueryBuilderSettings()
        {
            return new Setting()
            {
                Facet = DefaultFacetSettings(),
                Store = GetSettings().Store
            };
        }

        public static Setting GetSettings(Dictionary<string, string> memorySettings = null)
        {
            DotEnv.Load(".env", "conf/.env");
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddInMemoryCollection(memorySettings ?? new Dictionary<string, string>())
                .Build()
                .GetSection("QueryBuilderSetting")
                .Get<Setting>();
            return config;
        }

        public IOptions<Setting> Create()
        {
            var options = new Mock<IOptions<Setting>>();
            options.Setup(o => o.Value).Returns(defaultOptions);
            return options.Object;
        }
    }
}
