using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IStatementsRepository
    {
        public Task<IEnumerable<StatementEntry>> Get(string accountNumber);

        public Task Save(string accountNumber, DateTime date, string description, decimal balanceVariation, decimal balance);
    }
}
