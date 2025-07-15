// -------------------------------------------------------------------------------------------------
//
// ValidateCardResultDto.cs -- The ValidateCardResultDto.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.Models.Dtos;

public class CardValidationResultDto
{
    public string CardType { get; set; } = "Unknown";
    public bool IsValid { get; set; }
    public string FormattedNumber { get; set; } = string.Empty;

    // TODO: Add CVV validation support
    // public bool IsCvvValid { get; set; }

    // TODO: Include expiry date validation result
    // public bool IsExpiryDateValid { get; set; }

    // TODO: Add a property for expiry date input (optional)
    // public string? ExpiryMonth { get; set; }
    // public string? ExpiryYear { get; set; }

    // TODO: Add a validation message or summary for feedback to the client
    // public string? ValidationMessage { get; set; }
}
