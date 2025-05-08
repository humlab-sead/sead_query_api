using System.Collections.Generic;
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

    internal class SettingFactory
    {
        public static Setting defaultOptions { get; set; } = null;

        // Static dictionary for overriding settings
        public static Dictionary<string, string> OverrideSettings { get; set; } = new Dictionary<string, string>();

        public SettingFactory(Dictionary<string, string> overrideSettings = null)
        {
            OverrideSettings = overrideSettings ?? OverrideSettings ?? [];
        }

        // Default Facet Settings
        public static FacetSetting DefaultFacetSettings()
        {
            return new FacetSetting()
            {
                CountColumn = "tbl_analysis_entities.analysis_entity_id",
                CountTable = "tbl_analysis_entities"
            };
        }

        // Default Settings for QueryBuilder
        public static Setting DefaultSettings
        {
            get
            {
                return defaultOptions ??= new Setting()
                {
                    Facet = DefaultFacetSettings(),
                    Store = LoadSettings().Store
                };
            }
        }
        // Main method to get settings with overrides applied
        public static Setting LoadSettings(Dictionary<string, string> memorySettings = null)
        {
            DotEnv.Load(".env", "conf/.env");

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddInMemoryCollection(memorySettings ?? new Dictionary<string, string>())
                .AddInMemoryCollection(OverrideSettings) // <-- Apply Override Settings
                .Build()
                .GetSection("QueryBuilderSetting")
                .Get<Setting>();

            return config;
        }

        public Setting GetSettings(Dictionary<string, string> memorySettings = null)
        {
            if (memorySettings == null)
            {
                return LoadSettings(memorySettings);
            }
            return DefaultSettings;
        }

        // Create Mocked IOptions for Dependency Injection
        public IOptions<Setting> Create()
        {
            var options = new Mock<IOptions<Setting>>();
            options.Setup(o => o.Value).Returns(DefaultSettings);
            return options.Object;
        }
    }
}
