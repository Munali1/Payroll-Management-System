using Payroll.Application.Interfaces;
using Payroll.Domain.Entities;
using Payroll.Infrastructure.Data;


namespace Payroll.Infrastructure.Repository
{
    internal class SalaryRepository : Repository<Salary>, ISalaryRepository
    {
        private readonly AppDbContext context;

        public SalaryRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(Salary salary)
        {
            context.Salaries.Update(salary);
        }
    }
}
