using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public interface ITransactionServices
    {
        public Account withdraw(String accountNumber, double amount);

        public IEnumerable<StatementEntry> getStatement(String accountNumber);

        public void deposit(String accountNumber, double amount);
    }
}
