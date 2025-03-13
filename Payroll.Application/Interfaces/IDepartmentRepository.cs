
using Payroll.Domain.Entities;


namespace Payroll.Application.Interfaces
{
    public interface IDepartmentRepository:IRepository<Department>
    {
        void Update(Department department);
    }
}
