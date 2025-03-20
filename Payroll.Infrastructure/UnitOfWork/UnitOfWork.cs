
using FinalProject.Infrastructure.Repository;
using Payroll.Application.Interfaces;
using Payroll.Infrastructure.Data;
using Payroll.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Infrastructure.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext context;
        public IEmployeeRepository empRepository { get; private set; }
        public IDepartmentRepository departmentRepository { get; private set; }

        public IBankDetailsRepository bankDetailsRepository { get ; private set; }

        public ISalaryRepository salaryRepository { get; private set; }

      

        public IAttendenceRepository attendanceRepository { get;private set; }
        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            empRepository = new EmployeeRepository(context);
            departmentRepository = new DepartmentRepository(context);
            bankDetailsRepository = new BankDetailsRepository(context);
            salaryRepository = new SalaryRepository(context);
            attendanceRepository = new AttendanceRepository(context);

        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
