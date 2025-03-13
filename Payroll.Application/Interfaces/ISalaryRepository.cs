
using Payroll.Domain.Entities;


namespace Payroll.Application.Interfaces
{
    public interface ISalaryRepository:IRepository<Salary>
    {
        void Update(Salary salary);
    }
}
