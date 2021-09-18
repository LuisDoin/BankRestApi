using BankRestApi.Data;
using BankRestApi.Data.Repositories;
using BankRestApi.Models;
using BankRestApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankRestApi.UnitTests.Services
{
    [TestFixture]
    class TransactionService_GetStatementTests
    {
        private Mock<IAccountsRepository> _accountsRepository;
        private Mock<IStatementsRepository> _statementsRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _config;
        private TransactionService _transactionService;

        [SetUp]
        public void SetUp()
        {
            _accountsRepository = new Mock<IAccountsRepository>();
            _statementsRepository = new Mock<IStatementsRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _config = new Mock<IConfiguration>();
            _transactionService = new TransactionService(_accountsRepository.Object, _statementsRepository.Object,
                                                         _unitOfWork.Object, _config.Object);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void GetStatement_InvalidParameter_ThrowArgumentException(string accountNumber)
        {
            Assert.That(() => _transactionService.GetStatement(accountNumber), Throws.ArgumentException);
        }

        [Test]
        public void GetStatement_InexistentAccount_ThrowInvalidOperationException()
        {
            _statementsRepository.Setup(rep => rep.Get("a")).ReturnsAsync(Enumerable.Empty<StatementEntry>);

            Assert.That(() => _transactionService.GetStatement("a"), Throws.InvalidOperationException);
        }

        [Test]
        public void GetStatement_AccountExists_ReturnSatement()
        {
            var expected = new List<StatementEntry>() { new StatementEntry() };
            _statementsRepository.Setup(rep => rep.Get("a")).ReturnsAsync(expected);

            var result = _transactionService.GetStatement("a").Result;

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
