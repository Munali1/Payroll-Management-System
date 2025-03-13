

using Payroll.Domain.Entities;

namespace Payroll.Application.Interfaces
{
   public interface IEmployeeRepository:IRepository<Employee>
    {
        IEnumerable<Employee> GetEmployeesByDepartment(int departmentId);
        void Update(Employee employee);
    }
}
