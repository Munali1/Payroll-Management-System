using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Entities
{
    public class Leave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LeaveDate { get; set; }
        public double LeaveDuration {  get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public string LeaveReason {  get; set; }    
        public string? Status {  get; set; }
        public string? Remarks { get; set; }

    }
}
