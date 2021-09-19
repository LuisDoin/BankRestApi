using BankRestApi.Controllers;
using BankRestApi.Models;
using BankRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace BankRestApi.UnitTests.Controllers
{
    [TestFixture]
    class TransactionControllerTests
    {
        private Mock<ITransactionService> _transactionService;
        private Mock<ITokenService> _tokenService;
        private Mock<ILogger<TransactionController>> _logger;
        private TransactionController _transactionController;

        [SetUp]
        public void SetUp()
        {
            _transactionService = new Mock<ITransactionService>();
            _tokenService = new Mock<ITokenService>();
            _logger = new Mock<ILogger<TransactionController>>();
            _transactionController = new TransactionController(_transactionService.Object, _logger.Object);
        }
        
        [Test]
        public void Withdraw_InvalidParameters_ReturnBadRequest()
        {
            _transactionService.Setup(ts => ts.Withdraw(It.IsAny<string>(), It.IsAny<decimal>())).Throws<ArgumentException>();

            var result = _transactionController.Withdraw("a", 1).Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void Withdraw_InvalidOperation_ReturnBadRequest()
        {
            _transactionService.Setup(ts => ts.Withdraw(It.IsAny<string>(), It.IsAny<decimal>())).Throws<InvalidOperationException>();

            var result = _transactionController.Withdraw("a", 1).Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void Withdraw_ValidOperation_ReturnOk()
        {
            var result = _transactionController.Withdraw("a", 1).Result;
            var okResult = result as OkObjectResult;
            
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            _transactionService.Verify(ts => ts.Withdraw("a", 1));
        }

        [Test]
        public void Deposit_InvalidParameters_ReturnBadRequest()
        {
            _transactionService.Setup(ts => ts.Deposit(It.IsAny<string>(), It.IsAny<decimal>())).Throws<ArgumentException>();

            var result = _transactionController.Deposit("a", 1).Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void Deposit_InvalidOperation_ReturnBadRequest()
        {
            _transactionService.Setup(ts => ts.Deposit(It.IsAny<string>(), It.IsAny<decimal>())).Throws<InvalidOperationException>();

            var result = _transactionController.Deposit("a", 1).Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void Deposit_ValidOperation_ReturnOk()
        {
            var actionResult = _transactionController.Deposit("a", 1).Result;
            var statusCodeResult = (IStatusCodeActionResult)actionResult;

            Assert.AreEqual(200, statusCodeResult.StatusCode);
            _transactionService.Verify(ts => ts.Deposit("a", 1));
        }

        [Test]
        public void Transfer_InvalidParameters_ReturnBadRequest()
        {
            _transactionService.Setup(ts => 
                ts.Transfer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Throws<ArgumentException>();

            var result = _transactionController.Transfer("a", "b", 1).Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void Transfer_InvalidOperation_ReturnBadRequest()
        {
            _transactionService.Setup(ts => 
                ts.Transfer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Throws<InvalidOperationException>();

            var result = _transactionController.Transfer("a", "b", 1).Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void Transfer_ValidOperation_ReturnOk()
        {
            var actionResult = _transactionController.Transfer("a", "b", 1).Result;
            var statusCodeResult = (IStatusCodeActionResult)actionResult;

            Assert.AreEqual(200, statusCodeResult.StatusCode);
            _transactionService.Verify(ts => ts.Transfer("a", "b", 1));
        }

        [Test]
        public void GetStatement_InvalidParameters_ReturnBadRequest()
        {
            _transactionService.Setup(ts => ts.GetStatement(It.IsAny<string>())).Throws<ArgumentException>();

            var result = _transactionController.GetStatement("a").Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void GetStatement_InvalidOperation_ReturnBadRequest()
        {
            _transactionService.Setup(ts => ts.GetStatement(It.IsAny<string>())).Throws<InvalidOperationException>();

            var result = _transactionController.GetStatement("a").Result;
            var BadRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(BadRequestResult);
            Assert.AreEqual(400, BadRequestResult.StatusCode);
        }

        [Test]
        public void GetStatement_ValidOperation_ReturnOk()
        {
            var result = _transactionController.GetStatement("a").Result;
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            _transactionService.Verify(ts => ts.GetStatement("a"));
        }

        [Test]
        public void GetAccounts_ValidOperation_ReturnOk()
        {
            _transactionService.Setup(ts => ts.GetAccounts()).ReturnsAsync(new List<Account> { new Account() });

            var result = _transactionController.GetAccount().Result;
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
