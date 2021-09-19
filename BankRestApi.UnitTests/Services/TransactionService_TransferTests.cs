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
    class TransactionService_TransferTests
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
            section.Setup(x => x.Key).Returns("TransferFee");
            section.Setup(x => x.Value).Returns("1");
            _config.Setup(x => x.GetSection("TransferFee")).Returns(section.Object);
            _transactionService = new TransactionService(_accountsRepository.Object, _statementsRepository.Object,
                                                         _unitOfWork.Object, _config.Object);
        }

        [Test]
        [TestCase("", "a", 1)]
        [TestCase(null, "a", 1)]
        [TestCase("a", "", 1)]
        [TestCase("a", null, 1)]
        [TestCase("a", "a", 1)]
        [TestCase("a", "b", 0)]
        [TestCase("a", "b", -1)]
        public void Transfer_InvalidParameters_ThrowArgumentException(string fromAccount, string toAccount, decimal amount)
        {
            Assert.That(() => _transactionService.Transfer(fromAccount, toAccount, amount), Throws.ArgumentException);
        }

        [Test]
        [TestCase("a", "b")]
        [TestCase("b", "a")]
        public void Transfer_InexistentSourceOrDestinationAccount_ThrowInvalidOperationException(string fromAccount, string toAccount)
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(value: null);
            _accountsRepository.Setup(rep => rep.GetBalance("b")).ReturnsAsync(2);

            Assert.That(() => _transactionService.Transfer(fromAccount, toAccount, 1), Throws.InvalidOperationException);
        }

        [Test]
        public void Transfer_InsufficientFunds_ThrowInvalidOperationException()
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(1);

            Assert.That(() => _transactionService.Transfer("a", "b", 2), Throws.InvalidOperationException);
        }

        [Test]
        public void Transfer_AccountsExistAndSourceHasSufficientFunds_UpdateAccountAndStatementData()
        {
            _accountsRepository.Setup(rep => rep.GetBalance("a")).ReturnsAsync(2);
            _accountsRepository.Setup(rep => rep.GetBalance("b")).ReturnsAsync(1);

            _transactionService.Transfer("a", "b", 1).Wait();

            _accountsRepository.Verify(arep => arep.UpdateBalance("a", 0));
            _accountsRepository.Verify(arep => arep.UpdateBalance("b", 2));
            _statementsRepository.Verify(srep => srep.Save("a", It.IsAny<DateTime>(), "Transfer (to account b)", -1, 1));
            _statementsRepository.Verify(srep => srep.Save("a", It.IsAny<DateTime>(), "Transfer fee", -1, 0));
            _statementsRepository.Verify(srep => srep.Save("b", It.IsAny<DateTime>(), "Transfer (from account a)", 1, 2));
        }
    }
}
