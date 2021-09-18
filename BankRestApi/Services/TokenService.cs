using BankRestApi.Data.Repositories;
using BankRestApi.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public class TokenService : ITokenService
    {

        private readonly IUsersRepository _usersRepository;

        public TokenService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<string> GenerateToken(User candidateUser) 
        {
            var user = await _usersRepository.Get(candidateUser.Login, candidateUser.Password);

            if (user == null)
                throw new InvalidOperationException("Invalid user or password.");

            candidateUser.Role = user.Role;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
