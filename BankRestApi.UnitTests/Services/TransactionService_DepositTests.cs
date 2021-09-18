using BankRestApi.Data;
using BankRestApi.Data.Repositories;
using BankRestApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;

namespace BankRestApi.UnitTests.Services
{
    [TestFixture]
    class TransactionService_DepositTests
    {
        private Mock<IAccountsRepository> _accountsRepository;
        private Mock<IStatementsRepository> _statementsRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _config;
        private Mock<IConfigurationSection> section;
        private TransactionService _transactionService;

        [SetUp]
        public void SetUp()
        {
            _accountsRepository = new Mock<IAccountsRepository>();
            _statementsRepository = new Mock<IStatementsRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _config = new Mock<IConfiguration>();
            section = new Mock<IConfigurationSection>();
            section.Setup(x => x.Key).Returns("DepositPercentageFee");
            section.Setup(x => x.Value).Returns("0.01");
            _config.Setup(x => x.GetSection("DepositPercentageFee")).Returns(section.Object);
            _transactionService = new TransactionService(_accountsRepository.Object, _statementsRepository.Object,
                                                         _unitOfWork.Object, _config.Object);
        }

        [Test]
        [TestCase("", 1)]
        [TestCase(null, 1)]
        [TestCase("a", 0)]
        [TestCase("a", -1)]
        public void Deposit_InvalidParameters_ThrowArgumentException(string accountNumber, decimal amount)
        {
            Assert.That(() => _transactionService.Deposit(accountNumber, amount), Throws.ArgumentException);
        }

        [Test]
        public void Deposit_InexistentAccount_ThrowInvalidOperationException()
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(value: null);

            Assert.That(() => _transactionService.Deposit("a", 1), Throws.InvalidOperationException);
        }

        public void Deposit_AccountExistsAndAmountIsGreaterThanZero_UpdateAccountAndStatementData()
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(1);

            _transactionService.Withdraw("a", 1).Wait();

            _accountsRepository.Verify(arep => arep.UpdateBalance("a", (decimal)1.9));
            _statementsRepository.Verify(srep => srep.Save("a", It.IsAny<DateTime>(), "Deposit", 1, 2));
            _statementsRepository.Verify(srep => srep.Save("a", It.IsAny<DateTime>(), "Deposit fee", (decimal)-0.1, (decimal)1.9));
        }
    }
}
