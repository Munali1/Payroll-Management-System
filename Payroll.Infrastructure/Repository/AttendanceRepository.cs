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

        public void Update(Attendence attendence)
        {
            context.Attendences.Update(attendence);
        }
    }
}
