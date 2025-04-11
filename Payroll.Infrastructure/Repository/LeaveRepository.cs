using Payroll.Application.Interfaces;
using Payroll.Domain.Entities;
using Payroll.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Infrastructure.Repository
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private readonly AppDbContext context;

        public LeaveRepository(AppDbContext context):base(context) 
        {
            this.context = context;
        }
 
        public async Task ApproveLeave(Leave leave)
        {
            context.Leaves.Update(leave);
        }

    }
}
