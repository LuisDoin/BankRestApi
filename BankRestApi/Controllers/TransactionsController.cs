using BankRestApi.Data.Repositories;
using BankRestApi.Models;
using BankRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionServices _transactionServices;
        private readonly ITokenServices _tokenServices;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionServices transactionServices,
                                      ILogger<TransactionsController> logger,
                                      ITokenServices tokenServices)
        {
            _transactionServices = transactionServices;
            _logger = logger;
            _tokenServices = tokenServices;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User user)
        {
            try
            {
                var token = await _tokenServices.GenerateToken(user);
                user.Password = "";

                return new { user, token };
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error message: " + e.Message + " StackTrace: " + e.StackTrace);
                return StatusCode(500);
            }            
        }

        [HttpPut("withdraw")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier2")]
        public async Task<IActionResult> Withdraw(string accountNumber, decimal amount)
        {
            try
            {
                if (accountNumber == null || accountNumber.Length == 0 || amount <= 0)
                    return BadRequest("Invalid parameters.");

                return Ok(await _transactionServices.Withdraw(accountNumber, amount));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error message: " + e.Message + " StackTrace: " + e.StackTrace);
                return StatusCode(500);
            } 
        }

        [HttpPut("deposit")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier2")]
        public async Task<IActionResult> Deposit(string accountNumber, decimal amount)
        {
            try
            {
                if (accountNumber == null || accountNumber.Length == 0 || amount <= 0)
                    return BadRequest("Invalid parameters.");

                await _transactionServices.Deposit(accountNumber, amount);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error message: " + e.Message + " StackTrace: " + e.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpPut("transfer")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier2")]
        public async Task<IActionResult> Transfer(string fromAccount, string toAccount, decimal amount)
        {
            try
            {
                if (fromAccount == null || fromAccount.Length == 0 || toAccount == null || toAccount.Length == 0 || amount <= 0)
                    return BadRequest("Invalid parameters.");

                await _transactionServices.Transfer(fromAccount, toAccount, amount);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error message: " + e.Message + " StackTrace: " + e.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpGet("statement")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier1,tier2")]
        public async Task<IActionResult> GetStatement(string accountNumber)
        {
            try
            {
                if (accountNumber == null || accountNumber.Length == 0)
                    return BadRequest("Invalid parameters.");

                return Ok(await _transactionServices.GetStatement(accountNumber));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error message: " + e.Message + " StackTrace: " + e.StackTrace);
                return StatusCode(500);
            }            
        }
    }
}
