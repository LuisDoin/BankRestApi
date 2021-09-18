using BankRestApi.Data.Utils;
using BankRestApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public class StatementsRepository : IStatementsRepository
    {
        private DbSession _session;

        public StatementsRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<StatementEntry>> Get(string accountNumber)
        {
            return await _session.Connection.QueryAsync<StatementEntry>(SqlQueries.getStatements, new { accountNumber });
        }

        public async Task Save(string accountNumber, DateTime date, string description, decimal balanceVariation, decimal balance)
        {
            await _session.Connection.ExecuteAsync(SqlQueries.saveStatement, new { accountNumber, date, description, balanceVariation, balance }, _session.Transaction);
        }
    }
}
