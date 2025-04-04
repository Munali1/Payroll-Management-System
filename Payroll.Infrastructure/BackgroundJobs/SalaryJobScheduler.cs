using Hangfire;
using Payroll.Application.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Infrastructure.BackgroundJobs
{
    public class SalaryJobScheduler
    {
        private readonly ISalaryService salaryService;

        public SalaryJobScheduler(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }
        public void ScheduleMonthlyJob()
        {
            RecurringJob.AddOrUpdate(
                "monthly-salary-processor",
                () => salaryService.ProcessMonthlySalaries(),
                Cron.Minutely);
        }
    }
}
