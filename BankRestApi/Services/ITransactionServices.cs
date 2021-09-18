using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public interface ITransactionServices
    {
        public Task<Account> Withdraw(string accountNumber, decimal amount);

        public Task<IEnumerable<StatementEntry>> GetStatement(string accountNumber);

        public Task Deposit(string accountNumber, decimal amount);

        public Task Transfer(string fromAccount, string toAccount, decimal amount);
    }
}
