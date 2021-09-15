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
            var result = _transactionServices.withdraw(accountNumber, amount);
            return result != null ? Ok(result) : BadRequest("Account inexistent or with insufficient funds.");
        }

        [HttpGet("statement")]
        public IActionResult GetStatement(String accountNumber)
        {
            var result = _transactionServices.getStatement(accountNumber);
            return result.Any() ? Ok(result) : BadRequest("Inexistent account.");
        }
    }
}
