
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

        public async Task< BankDetails> GetBankDetails(int id)
        {
            var bank = await context.Banks.FirstOrDefaultAsync(x => x.EmployeeId == id);
            return bank;
        }

        public string GetDepartmentName(int id)
        {
            var employee = context.Employees
                                        .Include(e => e.department)
                                        .FirstOrDefault(e => e.Id ==id);

            if (employee == null || employee.department == null)
            {
                return "Department not found";
            }

            return employee.department.DepartmentName;
        }

        public string getEmpEmail(string id)
        {
            var employee = context.Employees.Include(e => e.ApplicationUser).FirstOrDefault(x => x.UserId == id);
            return employee.ApplicationUser.Email;
        }

        public int GetEmployeeIdFromUserId(string id)
        {
            var employee=context.Employees.FirstOrDefault(x => x.UserId == id);
            return (employee.Id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartment(int departmentId)
        {
            var departmentEmployees = await context.Employees.Where(e => e.DepartmentId == departmentId).ToListAsync();
            return departmentEmployees;
        }

        public  string getFullName(string id)
        {
            var employee =  context.Employees.Include(e => e.ApplicationUser).FirstOrDefault(x => x.UserId == id);
            return $"{employee.ApplicationUser.FirstName} {employee.ApplicationUser.LastName}";

        }
        public async Task<Salary> getSalaryDetails(int id)
        {
            var sal = await context.Salaries.FirstOrDefaultAsync(x => x.EmployeeId == id);
            return sal;
        }

        public async Task Update(Employee employee)
        {
           context.Update(employee);
        }

      
    }
}
