using AutoMapper;
using MainStAds.Application.DTOs;
using MainStAds.Core.Entities;

namespace MainStAds.Application.Mappings
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Business, BusinessDto>()
                .ReverseMap(); // Allows for mapping in both directions
            CreateMap<ErrorViewModel, ErrorViewModelDto>()
                .ReverseMap();
        }
    }
}
