using System.Collections.Generic;
using System.Text;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using SeadQueryCore;

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

        public SettingFactory(Dictionary<string, string> memorySettings=null)
        {
            defaultOptions = GetSettings(memorySettings);
        }
        public FacetSetting  DefaultFacetSettings()
        {
            return new FacetSetting() {
                CountColumn = "tbl_analysis_entities.analysis_entity_id",
                CountTable = "tbl_analysis_entities"
            };
        }

        public ISetting DefaultQueryBuilderSettings()
        {
            return new Setting() {
                Facet = DefaultFacetSettings(),
                Store = GetSettings().Store
            };
        }

        public static Setting GetSettings(Dictionary<string, string> memorySettings = null)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(memorySettings ?? new Dictionary<string, string>())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
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

        //public static Dictionary<string, string> DefaultSettings()
        //{
        //    var defaultSettings = new Dictionary<string, string>
        //    {
        //        { "QueryBuilderSetting:Facet:CountTable",           "tbl_analysis_entities"                     },
        //        { "QueryBuilderSetting:Facet:CountColumn",          "tbl_analysis_entities.analysis_entity_id"  },
        //        { "QueryBuilderSetting:Store:Host",                 "seadserv.humlab.umu.se"                    },
        //        { "QueryBuilderSetting:Store:Port",                 "5432"                                      },
        //        { "QueryBuilderSetting:Store:Database",             "sead_staging"                              },
        //        { "QueryBuilderSetting:Store:UseRedisCache",        "false"                                     }
        //    };
        //    return defaultSettings;
        //}
    }
}
