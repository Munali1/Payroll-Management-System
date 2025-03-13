
using Payroll.Application.Interfaces;
using Payroll.Domain.Entities;
using Payroll.Infrastructure.Data;
using Payroll.Infrastructure.Repository;


namespace FinalProject.Infrastructure.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly AppDbContext context;

        public DepartmentRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(Department department)
        {
            var dep=context.Departments.Update(department);
          
        }
    }
}
