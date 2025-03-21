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
            return await unitOfWork.attendanceRepository.getAll();
        }

        public async Task<Attendence> getIndividualAttendence(int id)
        {
           return await unitOfWork.attendanceRepository.GetAsync(x=>x.AttendenceId==id,"Employee");
        }

        public async Task<Attendence> getLatest(int id)
        {
            var attendence = await unitOfWork.attendanceRepository.GetLastestAttendenceEmployee(id);
            return attendence;
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

        public async Task PunchIn(int employeeId)
        {
            var existingRecord = await unitOfWork.attendanceRepository.
                GetAsync(x => x.EmployeeId == employeeId && x.inTime != null && x.outTime == null);

            if (existingRecord != null)
            {
                throw new Exception("You have already punched in. Please punch out first.");
            }

            var newAttendance = new Attendence
            {
                EmployeeId = employeeId,
                inTime = DateTime.Now
            };

            await Create(newAttendance);
        }

        public async Task PunchOut(int employeeId)
        {
            var attendance = await unitOfWork.attendanceRepository
        .GetAsync(x => x.EmployeeId == employeeId && x.outTime == null);

            if (attendance == null)
            {
                throw new Exception("No active punch-in record found.");
            }
            attendance.outTime = DateTime.Now;
            TimeSpan workedDuration = attendance.outTime.Value - attendance.inTime.Value;
            attendance.workingHours = new DateTime().Add(workedDuration);

            await Update(attendance);
        }

        public async Task Update(Attendence attendence)
        {
            unitOfWork.attendanceRepository.Update(attendence);
            await unitOfWork.SaveAsync();
        }
    }
}
