// -------------------------------------------------------------------------------------------------
//
// ValidateCardEndpointTests.cs -- The ValidateCardEndpointTests.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

using Moq;
using AutoMapper;
using card.validator.api.v1.CQRS.Handlers;
using card.validator.api.v1.CQRS.Commands;
using card.validator.api.v1.Models;
using card.validator.api.v1.Models.Dtos;
using card.validator.api.v1.Interfaces;
using Microsoft.Extensions.Logging;

namespace card.validator.api.v1.Tests.Handlers
{
    [TestClass]
    public class ValidateCardCommandHandlerTests
    {
        private Mock<ICardValidationService> serviceMock = null!;
        private Mock<IMapper> mapperMock = null!;
        private Mock<ILogger<ValidateCardCommandHandler>> loggerMock = null!;
        private ValidateCardCommandHandler handler = null!;

        [TestInitialize]
        public void Setup()
        {
            serviceMock = new Mock<ICardValidationService>();
            mapperMock = new Mock<IMapper>();
            loggerMock = new Mock<ILogger<ValidateCardCommandHandler>>();

            handler = new ValidateCardCommandHandler(
                serviceMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );
        }

        [TestMethod]
        public async Task HandleValidVisaReturnsExpectedResult()
        {
            // Arrange
            var command = new ValidateCardCommand("4111111111111111");
            var serviceResult = new CardValidationResult
            {
                CardType = "VISA",
                IsValid = true,
                FormattedNumber = "4111111111111111"
            };

            var dtoResult = new CardValidationResultDto
            {
                CardType = "VISA",
                IsValid = true,
                FormattedNumber = "4111111111111111"
            };

            serviceMock.Setup(s => s.Validate(command.CardNumber)).Returns(serviceResult);
            mapperMock.Setup(m => m.Map<CardValidationResultDto>(serviceResult)).Returns(dtoResult);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("VISA", result.CardType);
            Assert.IsTrue(result.IsValid);
        }
        [TestMethod]
        public async Task HandleNullOrWhitespaceCardNumberReturnsUnknownResult()
        {
            // Arrange
            var command = new ValidateCardCommand("   "); // or string.Empty
            var expected = new CardValidationResultDto
            {
                CardType = "Unknown",
                IsValid = false,
                FormattedNumber = string.Empty
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected.CardType, result.CardType);
            Assert.AreEqual(expected.IsValid, result.IsValid);
            Assert.AreEqual(expected.FormattedNumber, result.FormattedNumber);
        }

        [TestMethod]
        public async Task HandleExceptionThrownInServiceReturnsFallbackResult()
        {
            // Arrange
            var command = new ValidateCardCommand("1234567890123456");

            serviceMock
                .Setup(s => s.Validate(It.IsAny<string>()))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Unknown", result.CardType);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Empty, result.FormattedNumber);
        }

        [TestMethod]
        public async Task HandleMapsResultCorrectly()
        {
            // Arrange
            var command = new ValidateCardCommand("6011000990139424");

            var validationResult = new CardValidationResult
            {
                CardType = "Discover",
                IsValid = true,
                FormattedNumber = "6011000990139424"
            };

            var dto = new CardValidationResultDto
            {
                CardType = "Discover",
                IsValid = true,
                FormattedNumber = "6011000990139424"
            };

            serviceMock.Setup(x => x.Validate(command.CardNumber)).Returns(validationResult);
            mapperMock.Setup(m => m.Map<CardValidationResultDto>(validationResult)).Returns(dto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Discover", result.CardType);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual("6011000990139424", result.FormattedNumber);
        }
    }
}
