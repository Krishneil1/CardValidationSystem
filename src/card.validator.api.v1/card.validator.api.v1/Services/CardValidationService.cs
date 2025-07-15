// -------------------------------------------------------------------------------------------------
//
// CardValidationService.cs -- The CardValidationService.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.Services;

using System.Text.RegularExpressions;
using card.validator.api.v1.Interfaces;
using card.validator.api.v1.Models;
using Microsoft.Extensions.Logging;

public class CardValidationService : ICardValidationService
{
    private readonly ILogger<CardValidationService> _logger;

    private const int AmexLength = 15;
    private const int DiscoverLength = 16;
    private const int MasterCardLength = 16;
    private const int VisaShortLength = 13;
    private const int VisaLongLength = 16;
    private const int LuhnDoubleFactor = 2;
    private const int LuhnDigitThreshold = 9;
    private const int LuhnModuloBase = 10;

    public CardValidationService(ILogger<CardValidationService> logger)
    {
        _logger = logger;
    }

    public CardValidationResult Validate(string cardNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                _logger.LogWarning("Card number is null or empty.");
                return new CardValidationResult
                {
                    CardType = "Unknown",
                    IsValid = false,
                    FormattedNumber = string.Empty
                };
            }

            string cleaned = Regex.Replace(cardNumber, @"\D", "");

            _logger.LogInformation("Validating card number ending in {Last4}",
                cleaned.Length >= 4 ? cleaned[^4..] : "Unknown");

            string type = GetCardType(cleaned);
            bool isValid = IsValidLuhn(cleaned);

            _logger.LogInformation("Card type detected: {Type}, Valid Luhn: {IsValid}", type, isValid);

            return new CardValidationResult
            {
                CardType = type,
                IsValid = isValid,
                FormattedNumber = cleaned
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while validating card number.");
            return new CardValidationResult
            {
                CardType = "Unknown",
                IsValid = false,
                FormattedNumber = string.Empty
            };
        }
    }

    private string GetCardType(string number)
    {
        if ((number.StartsWith("34") || number.StartsWith("37")) && number.Length == AmexLength)
            return "AMEX";
        if (number.StartsWith("6011") && number.Length == DiscoverLength)
            return "Discover";
        if (Regex.IsMatch(number, @"^5[1-5]") && number.Length == MasterCardLength)
            return "MasterCard";
        if (number.StartsWith("4") && (number.Length == VisaShortLength || number.Length == VisaLongLength))
            return "VISA";

        return "Unknown";
    }

    private bool IsValidLuhn(string number)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(number[i].ToString());

            if (alternate)
            {
                digit *= LuhnDoubleFactor;
                if (digit > LuhnDigitThreshold)
                {
                    digit -= LuhnModuloBase - 1; // e.g., 14 becomes 5
                }
            }

            sum += digit;
            alternate = !alternate;
        }

        return (sum % LuhnModuloBase == 0);
    }
}
