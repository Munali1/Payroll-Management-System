using Payroll.Domain.Entities;


namespace Payroll.Application.Interfaces
{
    public interface IAttendenceRepository:IRepository<Attendence>
    {
        void Update(Attendence attendence);
    }
}
