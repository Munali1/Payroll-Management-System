
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.ViewModels
{
    public class AttendenceViewModel
    {
        public int EmployeeId { get; set; }
        public DateTime? inTime { get; set; }
        public DateTime? outTime { get; set; }
        public TimeSpan? workingHours { get; set; }
    }
}
