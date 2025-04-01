using AutoMapper;
using Payroll.Application.ViewModels;
using Payroll.Domain.Entities;


namespace Payroll.Application.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Attendence, AttendenceViewModel>().ReverseMap();
            CreateMap<BankDetails, BankViewModel>().ReverseMap();
            CreateMap<Department, DepartmentViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            CreateMap<Salary, SalaryViewModel>().ReverseMap ();
        }
    }
}
