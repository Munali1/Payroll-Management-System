using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.ViewModels
{
    public class EmployeeViewModel
    {
        public string UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Designation { get; set; }
        public string? EmployeeImage { get; set; }
    }
}
