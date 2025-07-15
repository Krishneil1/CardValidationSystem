// -------------------------------------------------------------------------------------------------
//
// ValidateCardCommandHandlerTests.cs -- The ValidateCardCommandHandlerTests.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------
namespace card.validator.api.v1.CQRS.Handlers.Tests;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using card.validator.api.v1.CQRS.Commands;
using card.validator.api.v1.Interfaces;
using card.validator.api.v1.Models;
using card.validator.api.v1.Models.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class ValidateCardCommandHandlerTests
{
    private Mock<ICardValidationService> validationServiceMock = null!;
    private Mock<IMapper> mapperMock = null!;
    private Mock<ILogger<ValidateCardCommandHandler>> loggerMock = null!;
    private ValidateCardCommandHandler handler = null!;

    [TestInitialize]
    public void Setup()
    {
        validationServiceMock = new Mock<ICardValidationService>();
        mapperMock = new Mock<IMapper>();
        loggerMock = new Mock<ILogger<ValidateCardCommandHandler>>();

        handler = new ValidateCardCommandHandler(
            validationServiceMock.Object,
            mapperMock.Object,
            loggerMock.Object
        );
    }

    [TestMethod]
    public async Task HandleValidCardReturnsMappedDto()
    {
        // Arrange
        var command = new ValidateCardCommand("4111111111111111");

        var serviceResult = new CardValidationResult
        {
            CardType = "VISA",
            IsValid = true,
            FormattedNumber = "4111111111111111"
        };

        var dto = new CardValidationResultDto
        {
            CardType = "VISA",
            IsValid = true,
            FormattedNumber = "4111111111111111"
        };

        validationServiceMock.Setup(s => s.Validate(command.CardNumber)).Returns(serviceResult);
        mapperMock.Setup(m => m.Map<CardValidationResultDto>(serviceResult)).Returns(dto);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("VISA", result.CardType);
        Assert.IsTrue(result.IsValid);
        Assert.AreEqual("4111111111111111", result.FormattedNumber);
    }

    [TestMethod]
    public async Task HandleEmptyCardNumberReturnsUnknownDto()
    {
        // Arrange
        var command = new ValidateCardCommand("");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Unknown", result.CardType);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(string.Empty, result.FormattedNumber);
    }

    [TestMethod]
    public async Task HandleValidationThrowsExceptionReturnsFallbackDto()
    {
        // Arrange
        var command = new ValidateCardCommand("1234567890123456");

        validationServiceMock
            .Setup(s => s.Validate(It.IsAny<string>()))
            .Throws(new Exception("Simulated failure"));

        // Even though exception will happen, mapper won't be called — setup not needed

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Unknown", result.CardType);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(string.Empty, result.FormattedNumber);
    }
}
