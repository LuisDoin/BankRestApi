using BankRestApi.Models;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public interface ITokenServices
    {
        public Task<string> GenerateToken(User user);
    }
}
