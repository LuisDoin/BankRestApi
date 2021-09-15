using BankRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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
        public IActionResult Withdraw(String accountNumber, int amount)
        {
            if (accountNumber.Length == 0 || amount <= 0)
                return BadRequest();

            try
            {
                return Ok(_transactionServices.withdraw(accountNumber, amount));
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Account inexistent or with insufficient funds.");
            }
            catch (Exception e)
            {
                _logger.LogError("Error message: " + e.Message + " StackTrace: " + e.StackTrace);
                return StatusCode(500);
            } 
        }

        [HttpPut("deposit")]
        public IActionResult Deposit(String accountNumber, double amount)
        {
            if (accountNumber.Length == 0 || amount <= 0)
                return BadRequest();

            try
            {
                _transactionServices.deposit(accountNumber, amount);
                return Ok(); 
            }
            catch(InvalidOperationException e)
            {
                return BadRequest("Inexistent account.");
            }
            catch(Exception e)
            {
                _logger.LogError("Error message: " + e.Message + " StackTrace: " + e.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpGet("statement")]
        public IActionResult GetStatement(String accountNumber)
        {
            var result = _transactionServices.getStatement(accountNumber);
            return result.Any() ? Ok(result) : BadRequest("Inexistent account.");
        }
    }
}
