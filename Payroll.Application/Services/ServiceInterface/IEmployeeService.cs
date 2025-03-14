using Microsoft.AspNetCore.Http;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
    public interface IEmployeeService
    {
        Task Create(Employee employee,IFormFile file);
        Task Delete(int id);
        Task<Employee> GetById(int id);
        Task<List<Employee>> GetEmployees();
        Task Update(Employee employee, IFormFile file);
        string getName(string id);
        int getEmpId(string id);
    }
}
