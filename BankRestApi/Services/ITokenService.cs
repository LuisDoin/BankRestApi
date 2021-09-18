using BankRestApi.Models;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}
