using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IAccountsRepository
    {
        public Task<decimal?> getBalance(string accountNumber);

        public Task updateBalance(string accountNumber, decimal amount); 
        
        
    }
}
