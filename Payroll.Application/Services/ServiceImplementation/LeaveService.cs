using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;

namespace Payroll.Application.Services.ServiceImplementation
{
    public class LeaveService : ILeaveService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailServiceInterface emailService;

        public LeaveService(IUnitOfWork unitOfWork,IEmailServiceInterface emailService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }
        public async Task applyLeave(Leave leave)
        {
            unitOfWork.leaveRepository.Add(leave);
            await unitOfWork.SaveAsync();
        }

        public async Task approveLeave(Leave leave)
        {
            unitOfWork.leaveRepository.UpdateLeave(leave);
            await unitOfWork.SaveAsync();
        }

        public string GenerateMail(Employee employee, Leave leave)
        {
            string mailBody = $@"
        <html>
        <body style='font-family: Arial, sans-serif;'>
            <h2 style='color: #2C3E50;'>Leave Notification</h2>
            <p>Dear {unitOfWork.empRepository.getFullName(employee.UserId)},</p>
            <p>We would like to inform you about the status of your leave request.</p>

            <table style='border: 1px solid #ccc; border-collapse: collapse; width: 100%;'>
                <tr>
                    <th style='padding: 8px; background-color: #f2f2f2; text-align: left;'>Leave Type</th>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{leave.LeaveType}</td>
                </tr>
                <tr>
                    <th style='padding: 8px; background-color: #f2f2f2; text-align: left;'>Leave Dates</th>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{leave.LeaveDate.ToShortDateString()} </td>
                </tr>
                <tr>
                    <th style='padding: 8px; background-color: #f2f2f2; text-align: left;'>Leave Duration</th>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{leave.LeaveDuration} days</td>
                </tr>
                <tr>
                    <th style='padding: 8px; background-color: #f2f2f2; text-align: left;'>Leave Status</th>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{leave.Status}</td>
                </tr>
            </table>

            <p>Leave Reason: {leave.LeaveReason}</p>

            <p>If you have any questions, feel free to contact HR.</p>

            <p>Best regards,</p>
            <p>HR Team</p>
        </body>
        </html>
    ";

            return mailBody;
        }

        public async Task<List<Leave>> getAllLeaves()
        {
            return await unitOfWork.leaveRepository.GetAllAsync();
        }

        public async Task<Leave> getIndividuaLeave(int id)
        {
            return await unitOfWork.leaveRepository.GetAsync(x => x.Id == id);
        }

        public async Task<List<Leave>> getIndividualEmployeeLeave(int id)
        {
            return await unitOfWork.leaveRepository.GetAllAsync(x => x.EmployeeId == id);
        }
        


        public async Task LeaveApproveMail()
        {
            var employees = await unitOfWork.empRepository.GetAllAsync();
            foreach (var employee in employees)
            {
                var sendUpdates = await unitOfWork.leaveRepository.GetAllAsync(
                    a => a.EmployeeId == employee.Id &&
                    a.Status == "Approved"||a.Status=="Rejected"
                   );
                if (sendUpdates.Any())
                {
                    foreach(var leave in sendUpdates)
                    {
                            var reportHtml = GenerateMail(employee, leave);
                            await emailService.sendEmail(unitOfWork.empRepository.getEmpEmail(employee.UserId),
                            reportHtml,
                           "Leave Approval for "+leave.LeaveDate);
                   

                    }
                }

            }
        }

    }
}
