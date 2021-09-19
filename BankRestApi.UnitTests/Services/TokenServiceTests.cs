using BankRestApi.Data.Repositories;
using BankRestApi.Models;
using BankRestApi.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRestApi.UnitTests.Services
{
    [TestFixture]
    class TokenServiceTests
    {
        private Mock<IUsersRepository> _usersRepository;
        private TokenService _tokenServices;

        [SetUp]
        public void SetUp()
        {
            _usersRepository = new Mock<IUsersRepository>();
            _tokenServices = new TokenService(_usersRepository.Object);
        }

        [Test]
        public void GenerateToken_InvalidUser_ThrowInvalidOperationException()
        {
            _usersRepository.Setup(urep => urep.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(value: null);

            Assert.That(() => _tokenServices.GenerateToken(new User()), Throws.InvalidOperationException);
        }

        [Test]
        public void GenerateToken_ValidUser_ThrowInvalidOperationException()
        {
            _usersRepository.Setup(urep => urep.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User { Login = "login", Password = "password", Role = "Role"});

            var result = _tokenServices.GenerateToken(new User()).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
    }
}
