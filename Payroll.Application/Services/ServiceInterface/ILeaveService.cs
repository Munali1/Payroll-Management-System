using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
    public interface ILeaveService
    {
        Task applyLeave(Leave leave);
        Task approveLeave(Leave leave);
        Task<List<Leave>> getAllLeaves();
        Task<List<Leave>> getIndividualEmployeeLeave(int id);
        Task<Leave> getIndividuaLeave(int id);
        Task LeaveApproveMail();
        string GenerateMail(Employee employee,Leave leave);

    }
}
