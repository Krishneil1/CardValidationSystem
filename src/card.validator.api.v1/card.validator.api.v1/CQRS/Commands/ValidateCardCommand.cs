// -------------------------------------------------------------------------------------------------
//
// ValidateCardCommand.cs -- The ValidateCardCommand.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.CQRS.Commands;

using card.validator.api.v1.Models.Dtos;
using MediatR;

public record ValidateCardCommand(string CardNumber) : IRequest<CardValidationResultDto>;
