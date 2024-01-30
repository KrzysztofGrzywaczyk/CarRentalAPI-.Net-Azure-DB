using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;

namespace CarRentalAPI
{
    public class RentalOfficeMappingProfile : Profile
    {
        public RentalOfficeMappingProfile()
        {
            CreateMap<RentalOffice, PresentRentalOfficeDto>()
                .ForMember(m => m.City, conf => conf.MapFrom(src => src.Address!.City))
                .ForMember(m => m.Street, conf => conf.MapFrom(src => src.Address!.Street))
                .ForMember(m => m.PostalCode, conf => conf.MapFrom(src => src.Address!.PostalCode));

            CreateMap<Car, PresentCarDto>();

            CreateMap<Car, PresentCarAllCarsDto>();

            CreateMap<CreateCarDto, Car>();

            CreateMap<CreateRentalOfficeDto, RentalOffice>()
                .ForMember(r => r.Address, conf => conf.MapFrom(dto => new Address()
                {City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street}
                ));

            CreateMap<User, PresentUserDto>()
                .ForMember(u => u.RoleName, conf => conf.MapFrom(src => src.Role!.Name));
        }
    }
}
