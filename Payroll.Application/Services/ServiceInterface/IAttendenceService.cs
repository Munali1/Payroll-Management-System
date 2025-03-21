using Payroll.Domain.Entities;
using System.Threading.Tasks;


namespace Payroll.Application.Services.ServiceInterface
{
    public interface IAttendenceService
    {
        Task Create(Attendence attendence);
        Task Update(Attendence attendence);
        Task Delete(int id);
        Task<List<Attendence>> getAttendenceList();
        Task<Attendence> getIndividualAttendence(int id);
        Task<string> GetWorkingHoursAsync(int id);
        Task PunchIn(int employeeId);
        Task PunchOut(int employeeId);

        Task<Attendence> getLatest(int id);
       

    }
}
