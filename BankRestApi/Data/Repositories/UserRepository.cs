using BankRestApi.Data.Utils;
using BankRestApi.Models;
using Dapper;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private DbSession _session;

        public UserRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<User> Get(string login, string password)
        {
            return await _session.Connection.QueryFirstOrDefaultAsync<User>(SqlQueries.getUser, new { login, password });
        }
    }
}
