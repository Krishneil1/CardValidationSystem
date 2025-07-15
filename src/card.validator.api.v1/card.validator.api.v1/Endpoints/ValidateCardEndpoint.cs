// -------------------------------------------------------------------------------------------------
//
// ValidateCardEndpoint.cs -- The ValidateCardEndpoint.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.Endpoints;

using card.validator.api.v1.CQRS.Commands;
using card.validator.api.v1.Models.Dtos;
using MediatR;

public static class ValidateCardEndpoint
{
    public static void MapValidateCardEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/cards/validate", async (
            ValidateCardCommand command,
            ISender mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("ValidateCard")
        .WithTags("Card Validation")
        .Produces<CardValidationResultDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();
    }
}
