using System.Data.SqlClient;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace DemoAPIDataAccess
{
    public class BaseDA
    {
        private static int connectionCount;

        public static int ConnectionCount { get => connectionCount; set => connectionCount = value; }


        private IConfiguration _config;

        public BaseDA(IConfiguration config)
        {
            _config = config;
        }

        private void CloseConnection()
        {
            Interlocked.Decrement(ref connectionCount);
        }

        private SqlConnection CreateConnection(string con)
        {
            if (ConnectionCount >= 10)
            {
                throw new Exception("Too many database connections.");
            }

            Interlocked.Increment(ref connectionCount);

            return new SqlConnection(con);
        }


        protected async Task<T> ExecuteStoredProcedureQuerySingleOrDefaultAsync<T>(string name, DynamicParameters parameters)
        {
            using SqlConnection connection = CreateConnection(_config.GetConnectionString("dbCon"));

            T? result = await connection.QuerySingleOrDefaultAsync<T?>(name, parameters, commandType: CommandType.StoredProcedure);

            CloseConnection();

            return result;
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProcedureQueryAsync<T>(string name)
        {
            using SqlConnection connection = CreateConnection(_config.GetConnectionString("dbCon"));

            // Execute the stored procedure
            IEnumerable<T> result = await connection.QueryAsync<T>(name, commandType: CommandType.StoredProcedure);

            CloseConnection();

            return result;
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProcedureQueryAsync<T>(string name, DynamicParameters parameters)
        {
            using SqlConnection connection = CreateConnection(_config.GetConnectionString("dbCon"));

            // Execute the stored procedure
            IEnumerable<T> result = await connection.QueryAsync<T>(name, parameters, commandType: CommandType.StoredProcedure);

            CloseConnection();

            return result;
        }

        protected async Task<int> ExecuteStoredProcedureAsync(string name, DynamicParameters parameters)
        {
            using SqlConnection connection = CreateConnection(_config.GetConnectionString("dbCon"));

            // Execute the stored procedure
            int result = await connection.ExecuteAsync(name, parameters, commandType: CommandType.StoredProcedure);

            CloseConnection();

            return result;
        }
    }
}
