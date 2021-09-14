using BankRestApi.Data.Utils;
using BankRestApi.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Statement> get(String accountNumber)
        {
            return _session.Connection.Query<Statement>(SqlQueries.getStatements, new { accountNumber }, _session.Transaction);
        }
    }
}
