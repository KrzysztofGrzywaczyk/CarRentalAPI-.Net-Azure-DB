using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;

namespace CarRentalAPI
{
    public class RentalOfficeMappingProfile : Profile
    {
        public RentalOfficeMappingProfile()
        {
            CreateMap<RentalOffice, RentalOfficeDto>()
                .ForMember(m => m.City, conf => conf.MapFrom(src => src.Address.City))
                .ForMember(m => m.Street, conf => conf.MapFrom(src => src.Address.Street))
                .ForMember(m => m.PostalCode, conf => conf.MapFrom(src => src.Address.PostalCode));

            CreateMap<Car, CarDto>();

            CreateMap<CreateRentalOfficeDto, RentalOffice>()
                .ForMember(r => r.Address, conf => conf.MapFrom(dto => new Address()
                { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }
                ));
        }
    }
}
