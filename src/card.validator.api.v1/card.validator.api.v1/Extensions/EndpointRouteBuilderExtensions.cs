// -------------------------------------------------------------------------------------------------
//
// EndpointRouteBuilderExtensions.cs -- The EndpointRouteBuilderExtensions.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.Extensions;

using card.validator.api.v1.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static void MapCardValidationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapValidateCardEndpoint(); // calls the one in /ValidateCard/
    }
}
