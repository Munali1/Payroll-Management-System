using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Domain.Entities
{
    public class BankDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public int? EmployeeId { get; set; }  
            [ForeignKey("EmployeeId")]
            public virtual Employee? Employee { get; set; }
            public string? BankName { get; set; }  
            public string? AccountNumber { get; set; }
            public string? AccountHolderName { get; set; }  
        }

    }

