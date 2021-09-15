using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IStatementsRepository
    {
        public IEnumerable<StatementEntry> get(String accountNumber);

        public void save(String accountNumber, DateTime date, String description, double balanceVariation, double balance);
    }
}
