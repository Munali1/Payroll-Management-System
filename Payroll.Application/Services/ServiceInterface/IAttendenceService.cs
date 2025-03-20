using Payroll.Domain.Entities;


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

    }
}
