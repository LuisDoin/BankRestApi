
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
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionServices,
                                      ILogger<TransactionController> logger)
        {
            _transactionServices = transactionServices;
            _logger = logger;
        }

        /// <summary>
        /// Withdraws from a given existent account and returns the state of the account after the transaction.  
        /// </summary>
        /// <returns> The state of the account after the transaction</returns>
        /// <remarks>
        /// 
        /// A llist of the current existent accounts can be queried through the Transactions/accounts endpoint.      
        /// 
        /// </remarks>
        /// <response code="200">The state of the account after the transaction</response>
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

        /// <summary>
        /// Deposits to a given existent account.  
        /// </summary>
        /// <returns> </returns>
        /// <remarks>
        /// 
        /// A list of the current existent accounts can be queried through the Transactions/accounts endpoint.     
        /// 
        /// </remarks>
        /// <response code="200"></response>
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

        /// <summary>
        /// Transfer from a given existent account to another given existent account.  
        /// </summary>
        /// <returns> </returns>
        /// <remarks>
        /// 
        /// A list of the current existent accounts can be queried through the Transactions/accounts endpoint.       
        /// 
        /// </remarks>
        /// <response code="200"></response>
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

        /// <summary>
        /// Return the statement of a given existent account.  
        /// </summary>
        /// <returns> </returns>
        /// <remarks>
        /// 
        /// A list of the current existent accounts can be queried through the Transactions/accounts endpoint.      
        /// 
        /// </remarks>
        /// <response code="200"></response>
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


        /// <summary>
        /// Returns a list of the current existent accounts.
        /// </summary>
        /// <returns> </returns>
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
