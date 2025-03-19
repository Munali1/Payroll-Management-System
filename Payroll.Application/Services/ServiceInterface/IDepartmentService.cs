using Microsoft.AspNetCore.Http;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
    public interface IDepartmentService
    {
        Task Create(Department department);
        Task Delete(int id);
        Task<Department> GetById(int id);
        Task<List<Department>> GetDepartments();
        Task Update(Department department);
        
    }
}
