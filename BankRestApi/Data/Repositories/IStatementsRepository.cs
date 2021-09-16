using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IStatementsRepository
    {
        public IEnumerable<StatementEntry> get(string accountNumber);

        public void save(string accountNumber, DateTime date, string description, decimal balanceVariation, decimal balance);
    }
}
