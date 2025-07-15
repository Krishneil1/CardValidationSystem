// -------------------------------------------------------------------------------------------------
//
// MappingProfile.cs -- The MappingProfile.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

namespace card.validator.api.v1.Mapping;

using AutoMapper;
using card.validator.api.v1.Models;
using card.validator.api.v1.Models.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CardValidationResult, CardValidationResultDto>().ReverseMap();
    }
}