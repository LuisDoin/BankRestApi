using BankRestApi.Controllers;
using BankRestApi.Models;
using BankRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankRestApi.UnitTests.Controllers
{
    [TestFixture]
    class AuthenticationControllerTests
    {
        private Mock<ITokenService> _tokenService;
        private Mock<ILogger<AuthenticationController>> _logger;
        private AuthenticationController _transactionController;

        [SetUp]
        public void SetUp()
        {
            _tokenService = new Mock<ITokenService>();
            _logger = new Mock<ILogger<AuthenticationController>>();
            _transactionController = new AuthenticationController(_tokenService.Object, _logger.Object);
        }

        [Test]
        public void Authenticate_InvalidUser_ReturnBadRequest()
        {
            _tokenService.Setup(ts => ts.GenerateToken(It.IsAny<User>())).Throws<InvalidOperationException>();

            var result = _transactionController.Authenticate(new User()).Result.Result;
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public void Authenticate_ValidUser_ReturnOk()
        {
            var user = new User();

            var result = _transactionController.Authenticate(user).Result.Result;
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            _tokenService.Verify(ts => ts.GenerateToken(user));
        }
    }
}
