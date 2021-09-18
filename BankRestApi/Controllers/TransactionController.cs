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
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionServices;
        private readonly ITokenService _tokenServices;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionServices,
                                      ILogger<TransactionController> logger,
                                      ITokenService tokenServices)
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

                return Ok(new { user, token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return StatusCode(500);
            }            
        }

        [HttpPut("withdraw")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier2")]
        public async Task<IActionResult> Withdraw(string accountNumber, decimal amount)
        {
            try
            {
                return Ok(await _transactionServices.Withdraw(accountNumber, amount));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return StatusCode(500);
            } 
        }

        [HttpPut("deposit")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier2")]
        public async Task<IActionResult> Deposit(string accountNumber, decimal amount)
        {
            try
            {
                await _transactionServices.Deposit(accountNumber, amount);
                return Ok();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpPut("transfer")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier2")]
        public async Task<IActionResult> Transfer(string fromAccount, string toAccount, decimal amount)
        {
            try
            {
                await _transactionServices.Transfer(fromAccount, toAccount, amount);
                return Ok();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpGet("statement")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier1,tier2")]
        public async Task<IActionResult> GetStatement(string accountNumber)
        {
            try
            {
                return Ok(await _transactionServices.GetStatement(accountNumber));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpGet("accounts")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "tier1,tier2")]
        public async Task<IActionResult> GetAccount()
        {
            try
            {
                return Ok(await _transactionServices.GetAccounts());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return StatusCode(500);
            }
        }
    }
}
