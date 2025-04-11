using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.ViewModels
{
    public class LeaveCreateDTO
    {
        public string LeaveType { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LeaveDate { get; set; }
        public float LeaveDuration { get; set; }
     

        public string LeaveReason { get; set; }
        public string Status { get; set; }
    }
}
