using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;


namespace Payroll.Web.Controllers
{
    public class SalaryController : Controller
    {
        private readonly ISalaryService salaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployeeService employeeService;

        public SalaryController(ISalaryService salaryService,UserManager<ApplicationUser> userManager,IEmployeeService employeeService)
        {
            this.salaryService = salaryService;
            this.userManager = userManager;
            this.employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {
            var salList=await salaryService.GetSalaryList();
            return View(salList);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = await employeeService.GetEmployees();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Salary salary)
        {
            ViewBag.Employees = await employeeService.GetEmployees();
            salary.PaymentDate = DateTime.Now;            
            if (ModelState.IsValid)
            {
              await  salaryService.Create(salary);
                return RedirectToAction("Index");
            }
            return View(salary);
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Employees = await employeeService.GetEmployees();
            var sal = await salaryService.GetById(id);
            return View(sal);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Salary salary)
        {
            ViewBag.Employees = await employeeService.GetEmployees();
            if (ModelState.IsValid)
            {
               salary.PaymentDate=DateTime.Now; 
                await salaryService.Update(salary);
                return RedirectToAction("Index");
            }
            else
            {
                return View(salary);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var sal=await salaryService.GetById(id);
            if (sal == null)
            {
                return NotFound();
            }
            else
            {
               await salaryService.Delete(id);
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            var sal=await salaryService.GetById(id);
            if (sal == null)
            {
                return NotFound();
            }
            else
            {
                return View(sal);
            }
           
        }
    }
}
