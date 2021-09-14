using Npgsql;
using System;
using System.Data;

namespace BankRestApi.Data
{
    public sealed class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession()
        {
            Connection = new NpgsqlConnection(Settings.ConnectionString);
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
