
using Payroll.Domain.Entities;


namespace Payroll.Application.Interfaces
{
    public interface IDepartmentRepository:IRepository<Department>
    {
        List<Employee> getEmployeesInDepartment(int id);
        void Update(Department department);
    }
}
