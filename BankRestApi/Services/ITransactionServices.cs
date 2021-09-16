using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public interface ITransactionServices
    {
        public Account withdraw(string accountNumber, decimal amount);

        public IEnumerable<StatementEntry> getStatement(string accountNumber);

        public void deposit(string accountNumber, decimal amount);

        public void transfer(string fromAccount, string toAccount, decimal amount);
    }
}
