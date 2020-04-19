using System;
using System.Collections.Generic;
using Npgsql;

namespace SQT
{
    public static class ConnectionStringFactory
    {
        public static Dictionary<string, string> DefaultSettings()
        {
            var defaultSettings = new Dictionary<string, string>
            {
                { "QueryBuilderSetting:Store:Host",                 "seadserv.humlab.umu.se"                    },
                { "QueryBuilderSetting:Store:Port",                 "5432"                                      },
                { "QueryBuilderSetting:Store:Database",             "sead_staging"                              },
                { "QueryBuilderSetting:Store:Username",             "humlab_admin"                              },
                { "QueryBuilderSetting:Store:UseRedisCache",        "false"                                     }
            };
            return defaultSettings;
        }

        public static string Create(Dictionary<string, string> defaultSettings = null)
        {
            defaultSettings ??= DefaultSettings();
            var settings = SettingFactory.GetSettings(defaultSettings).Store;
            return new NpgsqlConnectionStringBuilder
            {
                Host = settings.Host,
                Database = settings.Database,
                Username = settings.Username,
                Password = settings.Password,
                Port = Convert.ToInt32(settings.Port),
                Pooling = false
            }.ConnectionString;
        }


    }
}
