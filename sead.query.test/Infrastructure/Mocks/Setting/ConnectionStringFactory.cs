using System;
using Npgsql;

namespace SQT
{
    public static class ConnectionStringFactory
    {
        public static string Create(string hostName=null, string databaseName=null, string port = null)
        {
            var settings = SettingFactory.DefaultSettings.Store;
            return new NpgsqlConnectionStringBuilder
            {
                Host = hostName ?? settings.Host,
                Database = databaseName ?? settings.Database,
                Username = settings.Username,
                Password = settings.Password,
                Port = Convert.ToInt32(port ?? settings.Port),
                Pooling = false
            }.ConnectionString;
        }
    }
}
