using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IAccountsRepository
    {
        public decimal? getBalance(String accountNumber);

        public void updateBalance(String accountNumber, decimal amount); 
        
        
    }
}
