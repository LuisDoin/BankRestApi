using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public interface ITransactionServices
    {
        public Task<Account> withdraw(string accountNumber, decimal amount);

        public Task<IEnumerable<StatementEntry>> getStatement(string accountNumber);

        public Task deposit(string accountNumber, decimal amount);

        public Task transfer(string fromAccount, string toAccount, decimal amount);
    }
}
