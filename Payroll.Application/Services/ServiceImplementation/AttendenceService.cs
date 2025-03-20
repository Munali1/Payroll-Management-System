using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;


namespace Payroll.Application.Services.ServiceImplementation
{
    public class AttendenceService : IAttendenceService
    {
        private readonly IUnitOfWork unitOfWork;

        public AttendenceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Create(Attendence attendence)
        {
            unitOfWork.attendanceRepository.Add(attendence);
            await unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var attendence = await unitOfWork.attendanceRepository.GetAsync(x=>x.AttendenceId==id);
            unitOfWork.attendanceRepository.Remove(attendence);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<Attendence>> getAttendenceList()
        {
            return await unitOfWork.attendanceRepository.GetAllAsync(null,"Employee");
        }

        public async Task<Attendence> getIndividualAttendence(int id)
        {
           return await unitOfWork.attendanceRepository.GetAsync(x=>x.AttendenceId==id,"Employee");
        }

        public async Task<string> GetWorkingHoursAsync(int id)
        {
            var attendance = await unitOfWork.attendanceRepository.GetAsync(x => x.AttendenceId == id, "Employee");

            if (attendance == null || attendance.inTime == null || attendance.outTime == null)
            {
                return "N/A"; 
            }
            TimeSpan workedDuration = attendance.outTime.Value - attendance.inTime.Value;
            return $"{workedDuration.Hours} hours {workedDuration.Minutes} mins";
        }

        public async Task Update(Attendence attendence)
        {
            unitOfWork.attendanceRepository.Update(attendence);
            await unitOfWork.SaveAsync();
        }
    }
}
