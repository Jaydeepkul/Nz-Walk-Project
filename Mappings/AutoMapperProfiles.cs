using AutoMapper;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionResquestDTO, Region>().ReverseMap();

            CreateMap<AddWalksRequestDTO,Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();

            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();

            CreateMap<UpdateWalkDtoRequestModel, Walk>().ReverseMap();

        }
    }
}
