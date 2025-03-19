using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
   public interface ISalaryService
    {
        Task Create(Salary salary);
        Task Delete(int id);
        Task<Salary> GetById(int id);
        Task<List<Salary>> GetSalaryList();
        Task Update(Salary salary);

        IEnumerable<Salary> GetAll();
    }
}
