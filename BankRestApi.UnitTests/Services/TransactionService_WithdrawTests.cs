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
    class TransactionService_WithdrawTests
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
            section.Setup(x => x.Key).Returns("WithdrawalFee");
            section.Setup(x => x.Value).Returns("4");
            _config.Setup(x => x.GetSection("WithdrawalFee")).Returns(section.Object);
            _transactionService = new TransactionService(_accountsRepository.Object, _statementsRepository.Object,
                                                         _unitOfWork.Object, _config.Object); 
        }

        [Test]
        [TestCase("", 1)]
        [TestCase(null, 1)]
        [TestCase("a", 0)]
        [TestCase("a", -1)]
        public void Withdraw_InvalidParameters_ThrowArgumentException(string accountNumber, decimal amount)
        {
            Assert.That(() => _transactionService.Withdraw(accountNumber, amount), Throws.ArgumentException);
        }

        [Test]
        public void Withdraw_InexistentAccount_ThrowInvalidOperationException()
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(value: null);            

            Assert.That(() => _transactionService.Withdraw("a", 1), Throws.InvalidOperationException);
        }

        [Test]
        public void Withdraw_InsufficientFunds_ThrowInvalidOperationException()
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(1);

            Assert.That(() => _transactionService.Withdraw("a", 1), Throws.InvalidOperationException);
        }

        [Test]
        public void Withdraw_AccountExistsAndHasSufficientFunds_UpdateAccountAndStatementData()
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(5);

            _transactionService.Withdraw("a", 1).Wait();

            _accountsRepository.Verify(arep => arep.UpdateBalance("a", 0));
            _statementsRepository.Verify(srep => srep.Save("a", It.IsAny<DateTime>(), "Withdrawal", -1, 4));
            _statementsRepository.Verify(srep => srep.Save("a", It.IsAny<DateTime>(), "Withdrawal fee", -4, 0));
        }
    }
}
