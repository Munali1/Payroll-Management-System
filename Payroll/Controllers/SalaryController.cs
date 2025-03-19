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
        public IActionResult Index()
        {
            var salList= salaryService.GetAll();
            return View(salList);
        }
        public async Task<IActionResult> Create()
        {
            var employees = await employeeService.GetEmployees();

            ViewBag.Employees = employees.Select(e => new
            {
                Id = e.Id,
                Name = e.ApplicationUser.FirstName + " " + e.ApplicationUser.LastName
            }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Salary salary)
        {
            var employees = await employeeService.GetEmployees();
            ViewBag.Employees = employees.Select(e => new
            {
                Id = e.Id,
                Name = e.ApplicationUser.FirstName + " " + e.ApplicationUser.LastName
            }).ToList();
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
            var employees = await employeeService.GetEmployees();
            ViewBag.Employees = employees.Select(e => new
            {
                Id = e.Id,
                Name = e.ApplicationUser.FirstName + " " + e.ApplicationUser.LastName
            }).ToList();
            var sal = await salaryService.GetById(id);
            return View(sal);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Salary salary)
        {
            var employees = await employeeService.GetEmployees();
            ViewBag.Employees = employees.Select(e => new
            {
                Id = e.Id,
                Name = e.ApplicationUser.FirstName + " " + e.ApplicationUser.LastName
            }).ToList();
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
