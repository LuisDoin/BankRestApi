using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Utils
{
    public static class SqlQueries
    {
        public static readonly String getStatements = @"Select * from statements 
                                                        Where account_number = @accountNumber
                                                        Order by date desc 
                                                        Limit 5";

        public static readonly String getBalance = @"Select balance from accounts
                                                     Where account_number = @accountNumber";

        public static readonly String updateBalance = @"Update accounts
                                                        Set balance = @balance
                                                        Where account_number = @accountNumber";
    }
}
