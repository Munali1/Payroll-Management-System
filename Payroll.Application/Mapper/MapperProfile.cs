using AutoMapper;
using Payroll.Application.ViewModels;
using Payroll.Domain.Entities;


namespace Payroll.Application.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Attendence, AttendenceViewModel>();
            CreateMap<BankDetails, BankViewModel>();
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<Salary, SalaryViewModel>();
        }
    }
}
