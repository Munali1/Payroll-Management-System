using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Interfaces
{
    public interface ILeaveRepository:IRepository<Leave>
    {
    
        Task UpdateLeave(Leave leave);
    }
}
