
using Payroll.Domain.Entities;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;


namespace Payroll.Application.Interfaces
{
    public interface ISalaryRepository:IRepository<Salary>
    {
      
        IEnumerable<Salary> GetSalary();
        void Update(Salary salary);
    }
}
