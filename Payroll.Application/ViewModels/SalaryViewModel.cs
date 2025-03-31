using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.ViewModels
{
    public class SalaryViewModel
    {
        public int EmployeeId { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal Bonus { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
