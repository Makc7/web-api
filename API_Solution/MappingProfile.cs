using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace API_Solution
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>().ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Plane, PlaneDto>();
            CreateMap<Piot, PiotDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<PiotForCreatonDto, Piot>();
            CreateMap<PlaneForCreationDto, Plane>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<PlaneForUpdateDto, Plane>();
            CreateMap<PiotForUpdateDto, Piot>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<PlaneForUpdateDto, Plane>().ReverseMap();
        }
    }
}
