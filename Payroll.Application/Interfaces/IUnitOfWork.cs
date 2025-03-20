using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IEmployeeRepository empRepository { get; }
        IDepartmentRepository departmentRepository { get; }
        IBankDetailsRepository bankDetailsRepository { get; }
        ISalaryRepository salaryRepository { get; }
        IAttendenceRepository attendanceRepository { get; }
        Task SaveAsync();
    }
}
