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

        public Account get(string accountNumber)
        {
            return _session.Connection.QueryFirstOrDefault(SqlQueries.getBalance, new { accountNumber }, _session.Transaction);
        }

        public void update(string accountNumber, double balance)
        {
            _session.Connection.Execute(SqlQueries.updateBalance, new { accountNumber, balance }, _session.Transaction);
        }
    }
}
