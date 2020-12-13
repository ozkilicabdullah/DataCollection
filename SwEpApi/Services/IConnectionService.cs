using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Services
{
    public interface IConnectionService
    {
        TResult Scope<TResult>(string ConnectionKey, Func<IDbConnection, TResult> func);
        Task<TResult> ScopeAsync<TResult>(string ConnectionKey, Func<IDbConnection, Task<TResult>> func);
        void Scope(string ConnectionKey, Action<IDbConnection> func);
        Task ScopeAsync<TResult>(string ConnectionKey, Func<IDbConnection, Task> func);
        TResult TransactionScope<TResult>(string ConnectionKey, Func<IDbConnection, TResult> func);
    }
}
