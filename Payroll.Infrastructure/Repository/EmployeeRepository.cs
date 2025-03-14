
using Payroll.Domain.Entities;
using Payroll.Application.Interfaces;
using Payroll.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Payroll.Infrastructure.Repository
{
    internal class EmployeeRepository :  Repository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext context;

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public int GetEmployeeIdFromUserId(string id)
        {
            var employee=context.Employees.FirstOrDefault(x => x.UserId == id);
            return (employee.Id);
        }

        public IEnumerable<Employee> GetEmployeesByDepartment(int departmentId)
        {
            var departmentEmployees = context.Employees.Where(e => e.DepartmentId == departmentId).ToList();
            return departmentEmployees;
        }

        public string getFullName(string id)
        {
            var employee = context.Employees.Include(e => e.ApplicationUser) .FirstOrDefault(x => x.UserId ==id);
            return $"{employee.ApplicationUser.FirstName} {employee.ApplicationUser.LastName}";

        }

        public void Update(Employee employee)
        {
            context.Update(employee);
        }
    }
}
