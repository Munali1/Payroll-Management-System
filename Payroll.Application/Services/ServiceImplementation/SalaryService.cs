using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;

namespace Payroll.Application.Services.ServiceImplementation
{
    public class SalaryService : ISalaryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailServiceInterface emailService;
        private readonly IPdfGenerator pdfGenerator;

        public SalaryService(IUnitOfWork unitOfWork,IEmailServiceInterface emailService,IPdfGenerator pdfGenerator)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.pdfGenerator = pdfGenerator;
        }
        public async Task Create(Salary salary)
        {
            unitOfWork.salaryRepository.Add(salary);
            await unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var sal = await unitOfWork.salaryRepository.GetAsync(x => x.Id == id, "Employee");
            unitOfWork.salaryRepository.Remove(sal);
            await unitOfWork.SaveAsync();
        }

        public IEnumerable<Salary> GetAll()
        {
            var sal = unitOfWork.salaryRepository.GetSalary();
            return sal;
        }

        public async Task<Salary> GetById(int id)
        {
            var sal = await unitOfWork.salaryRepository.GetAsync(x => x.Id == id, "Employee");
            return sal;
        }

        public async Task<Salary> getEmployeeSalaryDetails(int id)
        {
            var sal = await unitOfWork.salaryRepository.GetAsync(x => x.EmployeeId == id, "Employee");
            return sal;
        }

        public async Task<List<Salary>> GetSalaryList()
        {
            return await unitOfWork.salaryRepository.GetAllAsync(null, "Employee");
        }

        public async Task Update(Salary salary)
        {
            unitOfWork.salaryRepository.Update(salary);
            await unitOfWork.SaveAsync();
        }
        public async Task ProcessMonthlySalaries()
        {
            var employees = await unitOfWork.empRepository.GetAllAsync();

            foreach (var employee in employees)
            {
                var exists = await unitOfWork.salaryRepository.GetAsync(s => s.EmployeeId == employee.Id &&
                    s.PaymentDate.Month == DateTime.Now.Month &&
                    s.PaymentDate.Year == DateTime.Now.Year,"Employee");

                if (exists == null)
                {
                    decimal baseSalary = 50000;  
                    decimal bonus = 5;    
                    decimal totalSalary = baseSalary + (bonus/100)*baseSalary;

                    var salary = new Salary
                    {
                        EmployeeId = employee.Id,
                        SalaryAmount = baseSalary,
                        Bonus = bonus,
                        TotalSalary = totalSalary,
                        PaymentDate = DateTime.Now
                    };

                    unitOfWork.salaryRepository.Add(salary);
                    await unitOfWork.SaveAsync();
              
                    var salarySlipHtml = GenerateSalarySlipHtml(salary, employee);
                    var pdfBytes = pdfGenerator.GeneratePdfFromHtml(salarySlipHtml);
                    await emailService.sendEmail(
                   unitOfWork.empRepository.getEmpEmail(employee.UserId),
                   salarySlipHtml,
                   "Your Salary Slip for " + DateTime.Now.ToString("MMMM yyyy"),
                   pdfBytes,
                   unitOfWork.empRepository.getFullName(employee.UserId)+" SalarySlip.pdf"
               );
                }
            }
        }
        public string GenerateSalarySlipHtml(Salary salary, Employee employee)
        {
            var empName = unitOfWork.empRepository.getFullName(employee.UserId);
            var EmpDep = unitOfWork.empRepository.GetDepartmentName(employee.Id);
            var htmlContent = $@"
<html>
    <body style='font-family:Arial,sans-serif;'>
        <div style='text-align:center; margin-bottom:20px;'>
            <img src='https://example.com/logo.png' alt='Company Logo' style='height:80px;' />
            <h2>Salary Slip</h2>
        </div>
        <table style='width:60%; margin:0 auto; border-collapse:collapse;'>
            <tr>
                <th style='text-align:left; padding:8px; border-bottom:1px solid #ccc;'>Employee Name</th>
                <td style='padding:8px; border-bottom:1px solid #ccc;'>{empName}</td>
            </tr>
            <tr>
                <th style='text-align:left; padding:8px; border-bottom:1px solid #ccc;'>Employee ID</th>
                <td style='padding:8px; border-bottom:1px solid #ccc;'>{employee.Id}</td>
            </tr>
            <tr>
                <th style='text-align:left; padding:8px; border-bottom:1px solid #ccc;'>Department</th>
                <td style='padding:8px; border-bottom:1px solid #ccc;'>{EmpDep}</td>
            </tr>
            <tr>
                <th style='text-align:left; padding:8px; border-bottom:1px solid #ccc;'>Base Salary</th>
                <td style='padding:8px; border-bottom:1px solid #ccc;'>{salary.SalaryAmount:C}</td>
            </tr>
            <tr>
                <th style='text-align:left; padding:8px; border-bottom:1px solid #ccc;'>Bonus</th>
                <td style='padding:8px; border-bottom:1px solid #ccc;'>{salary.Bonus}%</td>
            </tr>
            <tr>
                <th style='text-align:left; padding:8px; border-bottom:1px solid #ccc;'>Total Salary</th>
                <td style='padding:8px; border-bottom:1px solid #ccc;'>{salary.TotalSalary:C}</td>
            </tr>
            <tr>
                <th style='text-align:left; padding:8px; border-bottom:1px solid #ccc;'>Payment Date</th>
                <td style='padding:8px; border-bottom:1px solid #ccc;'>{salary.PaymentDate.ToShortDateString()}</td>
            </tr>
        </table>
       
    </body>
</html>";

      
            return htmlContent;
        }
    }
}