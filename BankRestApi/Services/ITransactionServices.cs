using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    interface ITransactionServices
    {
        public Account withdraw(String accountNumber, double amount);
    }
}
