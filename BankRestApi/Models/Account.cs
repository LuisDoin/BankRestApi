using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Models
{
    public class Account
    {
        public Account(string accountNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public String AccountNumber { get; set; }

        public decimal Balance { get; set; }
    }
}
