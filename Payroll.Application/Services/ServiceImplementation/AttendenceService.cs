using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;
using System.Text;


namespace Payroll.Application.Services.ServiceImplementation
{
    public class AttendenceService : IAttendenceService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailServiceInterface emailService;
        private readonly IPdfGenerator pdfGenerator;

        public AttendenceService(IUnitOfWork unitOfWork,IEmailServiceInterface emailService,IPdfGenerator pdfGenerator)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.pdfGenerator = pdfGenerator;
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

        public string GenerateAttendenceReportHtml(Employee employee,int presentDays, int absentDays, IEnumerable<Attendence> records)
        {
            var sb = new StringBuilder();
            sb.Append($"<h2>Attendance Report for {unitOfWork.empRepository.getFullName(employee.UserId)} - {DateTime.Now.ToString("MMMM yyyy")}</h2>");
            sb.Append($"<p><strong>Present Days:</strong> {presentDays}</p>");
            sb.Append($"<p><strong>Absent Days:</strong> {absentDays}</p>");
            sb.Append("<table border='1' cellpadding='5' cellspacing='0'><tr><th>Date</th><th>In Time</th><th>Out Time</th><th>Hours Worked</th></tr>");

            foreach (var record in records.OrderBy(r => r.inTime))
            {
                sb.Append("<tr>");
                sb.Append($"<td>{record.inTime?.ToShortDateString()}</td>");
                sb.Append($"<td>{record.inTime?.ToShortTimeString()}</td>");
                sb.Append($"<td>{record.outTime?.ToShortTimeString()}</td>");
                sb.Append($"<td>{record.workingHours?.ToString(@"hh\:mm")}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            return sb.ToString();
        }


        public async Task<List<Attendence>> getAttendenceList()
        {
            return await unitOfWork.attendanceRepository.getAll();
        }
        public async Task<List<Attendence>> getIndividualAttendence(int id)
        {
           return await unitOfWork.attendanceRepository.GetAllAsync(x=>x.EmployeeId==id);
        }

        public async Task<Attendence> getLatest(int id)
        {
            var attendence = await unitOfWork.attendanceRepository.GetLastestAttendenceEmployee(id);
            return attendence;
        }

        public async Task<List<Attendence>> getMonthlyAttendence(int EmpId)
        {
            int currentMonth=DateTime.Now.Month;
            int currentYear=DateTime.Now.Year;
            var monthlyattendence = await unitOfWork.attendanceRepository.GetAllAsync(
                a => a.EmployeeId == EmpId &&
                a.inTime.HasValue && a.inTime.Value.Month == currentMonth &&
                a.inTime.Value.Year == currentYear);
            return monthlyattendence;
        }

        public async Task<TimeSpan> getTotalWorkingHours(int EmpId)
        {
            var attendanceRecords = await unitOfWork.attendanceRepository
       .GetAllAsync(x => x.EmployeeId == EmpId && x.inTime.HasValue && x.outTime.HasValue);

            var todaysRecords = attendanceRecords
                .Where(x => x.inTime.Value.Date == DateTime.Today)
                .ToList();
            foreach (var record in todaysRecords)
            {
                if (record.inTime.HasValue && record.outTime.HasValue)
                {
                    record.workingHours = record.outTime.Value - record.inTime.Value;
                }
            }

            var totalWorkingMinutesToday = todaysRecords
                .Where(x => x.workingHours.HasValue)
                .Sum(x => x.workingHours.GetValueOrDefault().TotalMinutes);

            var totalWorkingHoursToday = TimeSpan.FromMinutes(totalWorkingMinutesToday);

            return totalWorkingHoursToday;
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

        public async Task ProcessMonthlyAttendence()
        {
            var employees = await unitOfWork.empRepository.GetAllAsync();
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            foreach (var employee in employees)
            {
                
                var monthlyAttendances = await unitOfWork.attendanceRepository.GetAllAsync(
                    a => a.EmployeeId == employee.Id &&
                         a.inTime.HasValue &&
                         a.inTime.Value.Month == currentMonth &&
                         a.inTime.Value.Year == currentYear
                );
                if (monthlyAttendances != null && monthlyAttendances.Any())
                {
                    int totalWorkingDays = 22; 
                    int presentDays = monthlyAttendances.Count(a =>
                        a.workingHours.HasValue &&
                        a.workingHours.Value.TotalHours >= 4 
                    );
                    int absentDays = totalWorkingDays - presentDays;
                 

                    var reportHtml = GenerateAttendenceReportHtml(employee, presentDays, absentDays, monthlyAttendances);
                    var pdfBytes = pdfGenerator.GeneratePdfFromHtml(reportHtml);

                    await emailService.sendEmail(
                        unitOfWork.empRepository.getEmpEmail(employee.UserId),
                        reportHtml,
                        "Your Attendance Report for " + DateTime.Now.ToString("MMMM yyyy"),
                        pdfBytes,
                        unitOfWork.empRepository.getFullName(employee.UserId) + "_AttendanceReport.pdf"
                    );
                }
            }
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
            attendance.workingHours = workedDuration;
            await Update(attendance);
        }

        public async Task Update(Attendence attendence)
        {
            unitOfWork.attendanceRepository.Update(attendence);
            await unitOfWork.SaveAsync();
        }
    }
}
