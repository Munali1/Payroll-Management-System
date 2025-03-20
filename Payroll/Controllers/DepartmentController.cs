using Microsoft.AspNetCore.Mvc;

using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;

namespace Payroll.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await departmentService.GetDepartments());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await departmentService.Create(department);
                return RedirectToAction("Index");
            }
            return View(department);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var dep = await departmentService.GetById(id);
            if (dep != null)
            {
                return View(dep);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                await departmentService.Update(department);
                return RedirectToAction("Index");
            }
            return View(department);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var dep = await departmentService.GetById(id);
            if (dep != null)
            {
                await departmentService.Delete(dep.DepartmentId);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        public IActionResult EmployeeList(int id)
        {
            var empList = departmentService.getEmployeeInDepartment(id);
            return View(empList);
        }
    }
}