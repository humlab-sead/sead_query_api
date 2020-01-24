using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SeadQueryTest.Mocks
{
    internal static class FakeConnectionFactory
    {
        public static DbConnection Create()
        { 
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return connection;
        }
    }
}
