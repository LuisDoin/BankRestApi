using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data
{
    public static class Settings
    {
        public static string ConnectionString = "Server = bankdb.cyjqwbjszuvs.us-east-1.rds.amazonaws.com; Port = 5432; Database = postgres; User Id = postgres; Password = bankdb1234;";
        //public static string ConnectionString = Environment.GetEnvironmentVariable("dbConnection");
    }
}
