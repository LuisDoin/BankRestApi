using BankRestApi.Data.Utils;
using BankRestApi.Models;
using Dapper;
using System;
using System.Collections.Generic;

namespace BankRestApi.Data.Repositories
{
    public class StatementsRepository : IStatementsRepository
    {
        private DbSession _session;

        public StatementsRepository(DbSession session)
        {
            _session = session;
        }

        public IEnumerable<StatementEntry> get(String accountNumber)
        {
            return _session.Connection.Query<StatementEntry>(SqlQueries.getStatements, new { accountNumber });
        }

        public void save(string accountNumber, DateTime date, string description, decimal balanceVariation, decimal balance)
        {
            _session.Connection.Execute(SqlQueries.saveStatement, new { accountNumber, date, description, balanceVariation, balance }, _session.Transaction);
        }
    }
}
