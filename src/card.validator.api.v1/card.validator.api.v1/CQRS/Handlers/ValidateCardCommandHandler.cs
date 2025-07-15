// -------------------------------------------------------------------------------------------------
//
// ValidateCardCommandHandler.cs -- The ValidateCardCommandHandler.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.CQRS.Handlers;

using AutoMapper;
using card.validator.api.v1.CQRS.Commands;
using card.validator.api.v1.Interfaces;
using card.validator.api.v1.Models.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

public class ValidateCardCommandHandler : IRequestHandler<ValidateCardCommand, CardValidationResultDto>
{
    private readonly ICardValidationService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<ValidateCardCommandHandler> _logger;

    private const int CardNumberLastDigitsToLog = 4;

    public ValidateCardCommandHandler(
        ICardValidationService service,
        IMapper mapper,
        ILogger<ValidateCardCommandHandler> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    public Task<CardValidationResultDto> Handle(ValidateCardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.CardNumber))
            {
                _logger.LogWarning("Card number is null or empty.");
                return Task.FromResult(new CardValidationResultDto
                {
                    CardType = "Unknown",
                    IsValid = false,
                    FormattedNumber = string.Empty
                });
            }

            string maskedCardNumber = request.CardNumber.Length >= CardNumberLastDigitsToLog
                ? request.CardNumber[^CardNumberLastDigitsToLog..]
                : "Unknown";

            _logger.LogInformation("Received card validation request for card number ending in {Last4Digits}", maskedCardNumber);

            var result = _service.Validate(request.CardNumber);
            var dto = _mapper.Map<CardValidationResultDto>(result);

            _logger.LogInformation("Card validation completed: Type={CardType}, Valid={IsValid}", dto.CardType, dto.IsValid);
            return Task.FromResult(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while validating card number: {CardNumber}", request.CardNumber);
            return Task.FromResult(new CardValidationResultDto
            {
                CardType = "Unknown",
                IsValid = false,
                FormattedNumber = string.Empty
            });
        }
    }
}