using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Entities
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId {  get; set; }  
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
        public string DepartmentEmail { get; set; }
    }
}
