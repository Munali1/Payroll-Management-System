using Hangfire;
using Payroll.Application.Services.ServiceInterface;


namespace Payroll.Infrastructure.BackgroundJobs
{
    public class AttendenceJobScheduler
    {
        private readonly IAttendenceService attendenceService;

        public AttendenceJobScheduler(IAttendenceService attendenceService)
        {
            this.attendenceService = attendenceService;
        }
        public void ScheduleMonthlyJob()
        {
            RecurringJob.AddOrUpdate(
                "monthly-attendence-processor",
                () => attendenceService.ProcessMonthlyAttendence(),
                Cron.Monthly);
        }
    }
}
