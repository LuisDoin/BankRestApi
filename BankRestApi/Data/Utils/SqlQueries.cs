using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Utils
{
    public static class SqlQueries
    {
        public static readonly string getUser = @"Select *
                                                From users 
                                                Where   
                                                    login = @login and
                                                    password = @password";

        public static readonly string getStatements = @"Select account_number as AccountNumber,
                                                               date as Date,
                                                               description as Description,
                                                               balance_variation as BalanceVariation,
                                                               balance as Balance
                                                        From statements 
                                                        Where account_number = @accountNumber
                                                        Order by date desc 
                                                        Limit 10";

        public static readonly string saveStatement = @"Insert into statements 
                                                        values (@accountNumber,
                                                                @date,
                                                                @description,
                                                                @balanceVariation,
                                                                @balance)";

        public static readonly string getBalance = @"Select balance from accounts
                                                     Where account_number = @accountNumber";

        public static readonly string getAccounts = @"Select account_number as AccountNumber,
                                                             balance as Balance
                                                      From accounts";

        public static readonly string updateBalance = @"Update accounts
                                                        Set balance = @balance
                                                        Where account_number = @accountNumber";
    }
}
