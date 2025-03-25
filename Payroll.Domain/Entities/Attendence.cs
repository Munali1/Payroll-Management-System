using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Entities
{
   public class Attendence
    {
        [Key]
        public int AttendenceId { get; set; }
         
        public int EmployeeId { get; set; }
        public DateTime? inTime { get; set; }
        public DateTime? outTime { get; set; }
        public TimeSpan? workingHours { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}
