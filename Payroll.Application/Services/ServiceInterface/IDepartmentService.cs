
using Payroll.Application.ViewModels;
using Payroll.Domain.Entities;


namespace Payroll.Application.Services.ServiceInterface
{
    public interface IDepartmentService
    {
        Task Create(Department department);
        Task Delete(int id);
        Task<Department> GetById(int id);
        Task<List<Department>> GetDepartments();
        Task Update(Department department);

        List<Employee> getEmployeeInDepartment(int id); 
        
    }
}
