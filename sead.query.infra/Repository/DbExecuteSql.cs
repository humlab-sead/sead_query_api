using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Microsoft.EntityFrameworkCore
{
    public static class RDFacadeExtensions
    {
        public static RelationalDataReader ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql,
            params object[] parameters)
        {
            try
            {
                var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();
                using (concurrencyDetector.EnterCriticalSection())
                {
                    var rawSqlCommand = databaseFacade
                        .GetService<IRawSqlCommandBuilder>()
                        .Build(sql, parameters);

                    return rawSqlCommand
                        .RelationalCommand
                        .ExecuteReader(
                            new RelationalCommandParameterObject(databaseFacade.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues, null,
                                null, null)
                        );
                }
            }
            catch (System.Exception ex)
            {
                Log.Logger.Error(ex, $"Error executing SQL: {sql}");
                throw;
            }
        }

        public static async Task<RelationalDataReader> ExecuteSqlQueryAsync(this DatabaseFacade databaseFacade,
            string sql,
            CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            try
            {
                var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();
                Log.Logger.Information($"Executing SQL: {sql}");

                using (concurrencyDetector.EnterCriticalSection())
                {
                    var rawSqlCommand = databaseFacade
                        .GetService<IRawSqlCommandBuilder>()
                        .Build(sql, parameters);

                    return await rawSqlCommand
                        .RelationalCommand
                        .ExecuteReaderAsync(
                            new RelationalCommandParameterObject(databaseFacade.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues, null,
                                null, null),
                            cancellationToken: cancellationToken);
                }
            }
            catch (System.Exception ex)
            {
                Log.Logger.Error(ex, $"Error executing SQL: {sql}");
                throw;
            }
        }
        public static IEnumerable<T> Select<T>(this DbDataReader reader, System.Func<DbDataReader, T> selector)
        {
            while (reader.Read())
            {
                yield return selector(reader);
            }
        }
    }
}
