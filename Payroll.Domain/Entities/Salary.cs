using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Entities
{
    public class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }  
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal Bonus { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
