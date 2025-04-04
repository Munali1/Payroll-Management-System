using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;

namespace Payroll.Application.Services.ServiceImplementation
{
    public class SalaryService : ISalaryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailServiceInterface emailService;

        public SalaryService(IUnitOfWork unitOfWork,IEmailServiceInterface emailService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
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

                   await emailService.sendEmail(unitOfWork.empRepository.getEmpEmail(employee.UserId), salarySlipHtml, "Your Salary Slip for " + DateTime.Now.ToString("MMMM yyyy"));
                }
            }
        }
        public string GenerateSalarySlipHtml(Salary salary, Employee employee)
        {
            var empName = unitOfWork.empRepository.getFullName(employee.UserId);
            var EmpDep = unitOfWork.empRepository.GetDepartmentName(employee.Id);

            var htmlContent = $@"
            <html>
                <body>
                    <h1>Salary Slip for {empName}</h1>
                    <p>Employee ID: {employee.Id}</p>
                    <p>Department: {EmpDep}</p>
                    <p>Base Salary: {salary.SalaryAmount:C}</p>
                    <p>Bonus: {salary.Bonus}%</p>
                    <p>Total Salary: {salary.TotalSalary:C}</p>
                    <p>Payment Date: {salary.PaymentDate.ToShortDateString()}</p>
                    <p>Thank you for your hard work!</p>
                </body>
            </html>";
            return htmlContent;
        }
    }
}