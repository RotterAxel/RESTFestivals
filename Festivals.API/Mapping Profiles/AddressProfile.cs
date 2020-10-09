using AutoMapper;
using Festivals.API.Infrastructure.Entities;
using Festivals.API.Models;

namespace Festivals.API.Mapping_Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>()
                .ForMember(x => x.CreatedOn, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, 
                    opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy, 
                    opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AddressForCreationDto, Address>()
                .ForMember(x => x.Id, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, 
                    opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy,
                    opt => opt.Ignore());

            CreateMap<AddressForUpdateDto, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, 
                    opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, 
                    opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy,
                    opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
