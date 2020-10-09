using AutoMapper;
using Festivals.API.Infrastructure.Entities;
using Festivals.API.Models;

namespace Festivals.API.Mapping_Profiles
{
    public class FestivalsProfile : Profile
    {
        public FestivalsProfile()
        {
            CreateMap<FestivalDto, Festival>()
                .ForMember(x => x.CreatedOn, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, 
                    opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy,
                    opt => opt.Ignore())
                .ForMember(x => x.Address, 
                    opt => opt.Ignore())
                .ForMember(x => x.AddressId, 
                    opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Festival, FestivalFullDto>();

            CreateMap<FestivalForCreationDto, Festival>()
                .ForMember(x => x.Id, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, 
                    opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy,
                    opt => opt.Ignore())
                .ForMember(x => x.AddressId, 
                    opt => opt.Ignore());

            CreateMap<FestivalForUpdateDto, Festival>()
                .ForMember(x => x.Id, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, 
                    opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy,
                    opt => opt.Ignore())
                .ForMember(x => x.AddressId, 
                    opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
