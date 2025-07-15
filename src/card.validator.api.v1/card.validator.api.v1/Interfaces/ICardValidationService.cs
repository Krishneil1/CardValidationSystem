// -------------------------------------------------------------------------------------------------
//
// ICardValidationService.cs -- The ICardValidationService.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.Interfaces;

using card.validator.api.v1.Models;

public interface ICardValidationService
{
    CardValidationResult Validate(string cardNumber);
}
