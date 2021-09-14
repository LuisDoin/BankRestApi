using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Models
{
    public class Account
    {
        public Account(string accountNumber, double balance)
        {
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public String AccountNumber { get; set; }

        public double Balance { get; set; }
    }
}
