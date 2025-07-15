// -------------------------------------------------------------------------------------------------
//
// CardValidationServiceTests.cs -- The CardValidationServiceTests.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------


using card.validator.api.v1.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace card.validator.api.v1.Tests.Services
{
    [TestClass]
    public class CardValidationServiceTests
    {
        private CardValidationService? service;

        [TestInitialize]
        public void Setup()
        {
            var loggerMock = new Mock<ILogger<CardValidationService>>();
            service = new CardValidationService(loggerMock.Object);
        }

        [DataTestMethod]
        [DataRow("4111111111111111", "VISA", true)]
        [DataRow("4111111111111", "VISA", false)]
        [DataRow("4012888888881881", "VISA", true)]
        [DataRow("378282246310005", "AMEX", true)]
        [DataRow("6011111111111117", "Discover", true)]
        [DataRow("5105105105105100", "MasterCard", true)]
        [DataRow("5105 1051 0510 5106", "MasterCard", false)]
        [DataRow("9111111111111111", "Unknown", false)]
        public void ValidateReturnsExpectedResults(string cardNumber, string expectedType, bool expectedValidity)
        {
            // Act
            var result = service!.Validate(cardNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedType, result.CardType);
            Assert.AreEqual(expectedValidity, result.IsValid);
        }

        [TestMethod]
        public void ValidateEmptyInputReturnsInvalid()
        {
            var result = service!.Validate("");

            Assert.AreEqual("Unknown", result.CardType);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Empty, result.FormattedNumber);
        }

        [TestMethod]
        public void ValidateNullInputReturnsInvalid()
        {
            var result = service!.Validate("");

            Assert.AreEqual("Unknown", result.CardType);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Empty, result.FormattedNumber);
        }
    }
}
