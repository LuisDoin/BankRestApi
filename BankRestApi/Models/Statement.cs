using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Models
{
    public class Statement
    {
        public DateTime Date { get; set; }

        public String Description { get; set; }

        public double BalanceVariation { get; set; }

        public double Balance { get; set; }
    }
}
