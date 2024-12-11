using System;
using AutoMapper;
using AscendionAPI.Models.Domain;
using AscendionAPI.Models.DTO;

namespace AscendionAPI.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

        CreateMap<Walk, WalkDto>().ReverseMap();
        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
        CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

        CreateMap<Difficulty, DifficultyDto>().ReverseMap();


        CreateMap<Workshop, WorkshopDto>().ReverseMap();
        CreateMap<Location, LocationDto>().ReverseMap();
        CreateMap<Modes, ModesDto>().ReverseMap();

        CreateMap<AddWorkshopRequestDto, Workshop>().ReverseMap();
        CreateMap<AddWorkshopRequestLocationDto, Location>().ReverseMap();
        CreateMap<AddWorkshopRequestModesDto, Modes>().ReverseMap();

        CreateMap<UpdateWorkshopRequestDto, Workshop>().ReverseMap();
        CreateMap<UpdateWorkshopRequestLocationDto, Location>().ReverseMap();
        CreateMap<UpdateWorkshopRequestModesDto, Modes>().ReverseMap();

        CreateMap<Session, SessionDto>().ReverseMap();
    }
}