using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IAccountsRepository
    {
        public Task<IEnumerable<Account>> GetAccounts(); 

        public Task<decimal?> GetBalance(string accountNumber);

        public Task UpdateBalance(string accountNumber, decimal amount); 
        
        
    }
}
