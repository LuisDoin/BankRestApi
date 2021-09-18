using BankRestApi.Data.Utils;
using BankRestApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private DbSession _session;

        public AccountsRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _session.Connection.QueryAsync<Account>(SqlQueries.getAccounts);

        }

        public async Task<decimal?> GetBalance(string accountNumber)
        {
            return await _session.Connection.QueryFirstOrDefaultAsync<decimal?>(SqlQueries.getBalance, new { accountNumber }, _session.Transaction);
        }

        public async Task UpdateBalance(string accountNumber, decimal balance)
        {
            await _session.Connection.ExecuteAsync(SqlQueries.updateBalance, new { accountNumber, balance }, _session.Transaction);
        }
    }
}
