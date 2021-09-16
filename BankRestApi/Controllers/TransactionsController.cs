using BankRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BankRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionServices _transactionServices;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionServices transactionServices,
                                      ILogger<TransactionsController> logger)
        { 
            _transactionServices = transactionServices;
            _logger = logger;
        }

        [HttpPut("withdraw")]
        public IActionResult Withdraw(string accountNumber, decimal amount)
        {
            try
            {
                if (accountNumber == null || accountNumber.Length == 0 || amount <= 0)
                    return BadRequest("Invalid parameters.");

                return Ok(_transactionServices.withdraw(accountNumber, amount));
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
        public IActionResult Deposit(string accountNumber, decimal amount)
        {
            try
            {
                if (accountNumber == null || accountNumber.Length == 0 || amount <= 0)
                    return BadRequest("Invalid parameters.");

                _transactionServices.deposit(accountNumber, amount);
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
        public IActionResult Transfer(string fromAccount, string toAccount, decimal amount)
        {
            try
            {
                if (fromAccount == null || fromAccount.Length == 0 || toAccount == null || toAccount.Length == 0 || amount <= 0)
                    return BadRequest("Invalid parameters.");

                _transactionServices.transfer(fromAccount, toAccount, amount);
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
        public IActionResult GetStatement(string accountNumber)
        {
            try
            {
                if (accountNumber == null || accountNumber.Length == 0)
                    return BadRequest("Invalid parameters.");

                return Ok(_transactionServices.getStatement(accountNumber));
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
