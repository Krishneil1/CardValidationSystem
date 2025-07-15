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
}