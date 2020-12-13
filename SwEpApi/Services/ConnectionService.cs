using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;

namespace SwEpApi.Services
{
    public class ConnectionService : IConnectionService
    {

        private readonly IConfiguration Configuration;

        public ConnectionService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDbConnection NewConnection(string ConnectionKey) => new SqlConnection(Configuration.GetConnectionString(ConnectionKey));

        #region Connection Scopes

        public TResult Scope<TResult>(string ConnectionKey, Func<IDbConnection, TResult> func)
        {
            using (var connection = NewConnection(ConnectionKey))
            {                
                connection.Open();
                return func(connection);
            }
        }

        public TResult TransactionScope<TResult>(string ConnectionKey, Func<IDbConnection, TResult> func)
        {
            var result = default(TResult);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    result = Scope(ConnectionKey, func);
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public async Task<TResult> ScopeAsync<TResult>(string ConnectionKey, Func<IDbConnection, Task<TResult>> funcAsync)
        {
            using (var connection = NewConnection(ConnectionKey))
            {
                connection.Open();
                return await funcAsync(connection);
            }
        }

        public void Scope(string ConnectionKey, Action<IDbConnection> func)
        {
            using (var connection = NewConnection(ConnectionKey))
            {
                connection.Open();
                func(connection);
            }
        }

        public async Task ScopeAsync<TResult>(string ConnectionKey, Func<IDbConnection, Task> funcAsync)
        {
            using (var connection = NewConnection(ConnectionKey))
            {
                connection.Open();
                await funcAsync(connection);
            }
        }

        #endregion Connection Scopes
    }
}
