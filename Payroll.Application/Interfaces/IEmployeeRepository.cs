

using Payroll.Domain.Entities;
using System.Collections.Specialized;
using System.Globalization;

namespace Payroll.Application.Interfaces
{
   public interface IEmployeeRepository:IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesByDepartment(int departmentId);
        string getFullName(string id);
        Task<Salary> getSalaryDetails(int id);
        Task<BankDetails> GetBankDetails(int id);
        int GetEmployeeIdFromUserId(String id);
        Task Update(Employee employee);
       string GetDepartmentName(int id);

        string getEmpEmail(string id);
    }
}
