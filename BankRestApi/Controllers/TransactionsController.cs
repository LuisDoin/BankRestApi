using BankRestApi.Models;
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
        public Account Put(String accountNumber, int amount)
        {
            return _transactionServices.withdraw(accountNumber, amount);

        }
    }
}
