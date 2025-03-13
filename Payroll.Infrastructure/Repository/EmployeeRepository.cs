
using Payroll.Domain.Entities;
using Payroll.Application.Interfaces;
using Payroll.Infrastructure.Data;


namespace Payroll.Infrastructure.Repository
{
    internal class EmployeeRepository :  Repository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext context;

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<Employee> GetEmployeesByDepartment(int departmentId)
        {
            var departmentEmployees = context.Employees.Where(e => e.DepartmentId == departmentId).ToList();
            return departmentEmployees;
        }

        public void Update(Employee employee)
        {
            context.Update(employee);
        }
    }
}
