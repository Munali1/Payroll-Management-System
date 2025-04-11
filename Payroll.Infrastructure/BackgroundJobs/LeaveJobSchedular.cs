using Hangfire;
using Payroll.Application.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Infrastructure.BackgroundJobs
{
    public class LeaveJobSchedular
    {
        private readonly ILeaveService leaveService;

        public LeaveJobSchedular(ILeaveService leaveService)
        {
            this.leaveService = leaveService;
        }
        public void ScheduleMonthlyJob()
        {
            RecurringJob.AddOrUpdate(
                "leave-status-mail",
                () => leaveService.LeaveApproveMail(),
                Cron.Minutely);
        }
    }
}
