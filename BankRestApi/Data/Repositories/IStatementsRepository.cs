using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IStatementsRepository
    {
        public IEnumerable<Statement> get(String accountNumber);
    }
}
