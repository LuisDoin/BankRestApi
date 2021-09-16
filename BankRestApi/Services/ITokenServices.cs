using BankRestApi.Models;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public interface ITokenServices
    {
        public Task<string> generateToken(User user);
    }
}
