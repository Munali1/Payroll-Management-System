using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Payroll.Application.Services.ServiceImplementation;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;


namespace Payroll.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDepartmentService departmentService;
        private readonly IEmailServiceInterface emailService;

        public EmployeeController(IEmployeeService employeeService, UserManager<ApplicationUser> userManager,
            IDepartmentService departmentService,IEmailServiceInterface emailService)
        {
            _employeeService = employeeService;
            _userManager = userManager;
            this.departmentService = departmentService;
            this.emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetEmployees();
            return View(employees);
        }

 
        public async Task<IActionResult> Create()
        {
            var departments = await departmentService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View();
        }

 
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee, IFormFile file, string firstName, string lastName, string email, string password)
        {
            var departments = await departmentService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    employee.UserId = user.Id;
                    await _employeeService.Create(employee, file);
                    await _userManager.AddToRoleAsync(user, "Employee");
                     string subject = "Welcome to the Company";
                     string body = $"Hello {firstName} {lastName},\n\n" +
                              $"Your employee account has been successfully created.\n" +
                              $"Email: {email}\n" +
                              $"Password: {password}\n\n" +
                              "Please change your password after your first login.\n\n" +
                              "Best regards,\nYour Company";
                emailService.sendEmail(email, body, subject);

                return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            
            return View(employee);
        }

  
        public async Task<IActionResult> Edit(int id)
        {
            var departments = await departmentService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            var employee = await _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(employee.UserId);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.Email = user.Email;

            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee, IFormFile file, string firstName, string lastName, string email)
        {
            var departments = await departmentService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(employee.UserId);
                if (user != null)
                {
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.Email = email;
                    await _userManager.UpdateAsync(user);
                }

                await _employeeService.Update(employee, file);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

   
        public async Task<IActionResult> Details(int id)
        {
            var employee =await  _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(employee.UserId);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.Email = user.Email;

            return View(employee);
        }

    
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetById(id);
            if (employee != null)
            {
               await _employeeService.Delete(id);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        public IActionResult BankDetails(int id)
        { 
            var bankDetails = _employeeService.EmployeeBankDetails(id);
            if (bankDetails == null) {
                return NotFound();
            }
            else
            {
                return RedirectToAction("Details", "Bank", new { id = bankDetails.Id });
            }
        }
        public IActionResult SalaryDetails(int id)
        {
            var sal = _employeeService.EmployeeSalaryDetails(id);
            if (sal == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Salary", new { id = sal.Id });
        }

    }
}
