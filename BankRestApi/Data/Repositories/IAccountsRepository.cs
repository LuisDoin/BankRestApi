using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    interface IAccountsRepository
    {
        public void update(String accountNumber, double amount); 
        
        public Account get(String accountNumber);
    }
}
