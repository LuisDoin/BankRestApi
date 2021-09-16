using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data
{
    public static class Settings
    {
        public static string ConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
    }
}
