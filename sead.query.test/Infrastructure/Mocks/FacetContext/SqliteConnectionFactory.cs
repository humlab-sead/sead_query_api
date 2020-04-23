using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SQT.Mocks
{
    internal static class SqliteConnectionFactory
    {
        public static DbConnection Create()
        {
            var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys = False");
            connection.Open();
            return connection;
        }

        public static DbConnection CreateAndOpen()
        {
            var connection = Create();
            connection.Open();
            return connection;
        }
    }
}
