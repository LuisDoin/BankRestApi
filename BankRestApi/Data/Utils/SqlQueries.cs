using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Utils
{
    public static class SqlQueries
    {
        public static readonly String getStatements = @"Select account_number as AccountNumber,
                                                               date as Date,
                                                               description as Description,
                                                               balance_variation as BalanceVariation,
                                                               balance as Balance
                                                        From statements 
                                                        Where account_number = @accountNumber
                                                        Order by date desc 
                                                        Limit 5";

        public static readonly String saveStatement = @"Insert into statements 
                                                        values (@accountNumber,
                                                                @date,
                                                                @description,
                                                                @balanceVariation,
                                                                @balance)";

        public static readonly String getBalance = @"Select balance from accounts
                                                     Where account_number = @accountNumber";

        public static readonly String updateBalance = @"Update accounts
                                                        Set balance = @balance
                                                        Where account_number = @accountNumber";
    }
}
