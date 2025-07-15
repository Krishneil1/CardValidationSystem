// -------------------------------------------------------------------------------------------------
//
// ValidateCardEndpointTests.cs -- The ValidateCardEndpointTests.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.Endpoints.Tests;

using System.Net.Http.Json;
using card.validator.api.v1.CQRS.Commands;
using card.validator.api.v1.Models.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

[TestClass()]
public class ValidateCardEndpointTests
{
    private static HttpClient client = null!;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        var factory = new WebApplicationFactory<Program>(); // Assuming Program.cs is the entry point
        client = factory.CreateClient();
    }

    [TestMethod]
    public async Task ValidateCardValidVisaReturnsOk()
    {
        // Arrange
        var command = new ValidateCardCommand("4111111111111111");

        // Act
        var response = await client.PostAsJsonAsync("/api/cards/validate", command);

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);

        var result = await response.Content.ReadFromJsonAsync<CardValidationResultDto>();
        Assert.IsNotNull(result);
        Assert.AreEqual("VISA", result.CardType);
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public async Task ValidateCardInvalidNumberReturnsValidFalse()
    {
        var command = new ValidateCardCommand("4111111111111");

        var response = await client.PostAsJsonAsync("/api/cards/validate", command);

        Assert.IsTrue(response.IsSuccessStatusCode);

        var result = await response.Content.ReadFromJsonAsync<CardValidationResultDto>();
        Assert.IsNotNull(result);
        Assert.AreEqual("VISA", result.CardType);
        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public async Task ValidateCardUnknownCardTypeReturnsUnknown()
    {
        var command = new ValidateCardCommand("9111111111111111");

        var response = await client.PostAsJsonAsync("/api/cards/validate", command);

        Assert.IsTrue(response.IsSuccessStatusCode);

        var result = await response.Content.ReadFromJsonAsync<CardValidationResultDto>();
        Assert.IsNotNull(result);
        Assert.AreEqual("Unknown", result.CardType);
        Assert.IsFalse(result.IsValid);
    }
}
