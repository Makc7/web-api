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
            CreateMap<Pilor, PilorDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<PilorForCreatonDto, Pilor>();
            CreateMap<PlaneForCreationDto, Plane>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<PlaneForUpdateDto, Plane>();
            CreateMap<PilorForUpdateDto, Pilor>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<PlaneForUpdateDto, Plane>().ReverseMap();
        }
    }
}
