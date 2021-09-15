using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Models
{
    public class StatementEntry
    {
        public String AccountNumber { get; set; }

        public DateTime Date { get; set; }

        public String Description { get; set; }

        public double BalanceVariation { get; set; }

        public double Balance { get; set; }
    }
}
