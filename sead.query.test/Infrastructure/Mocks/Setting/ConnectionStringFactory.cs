using System;
using System.Collections.Generic;
using Npgsql;

namespace SQT
{
    public static class ConnectionStringFactory
    {
        public static string Create(Dictionary<string, string> defaultSettings = null)
        {
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
