using Microsoft.EntityFrameworkCore;
using Payroll.Application.Interfaces;
using Payroll.Domain.Entities;
using Payroll.Infrastructure.Data;

namespace Payroll.Infrastructure.Repository
{
    public class AttendanceRepository : Repository<Attendence>, IAttendenceRepository
    {
        private readonly AppDbContext context;

        public AttendanceRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Attendence>> getAll()
        {
           return await context.Attendences.Include(x=>x.Employee).ThenInclude(x=>x.ApplicationUser).ToListAsync();
        }

        public async Task<Attendence> GetLastestAttendenceEmployee(int Empid)
        {
           return await context.Attendences.Where(a => a.EmployeeId == Empid).OrderByDescending(a => a.inTime).FirstOrDefaultAsync();
        }

        public void Update(Attendence attendence)
        {
            context.Attendences.Update(attendence);
        }
    }
}
