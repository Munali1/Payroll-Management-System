using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Payroll.Application.Services.ServiceInterface;

namespace Payroll.Web.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendenceService attendenceService;
        private readonly IEmployeeService employeeService;

        public AttendanceController(IAttendenceService attendenceService,IEmployeeService employeeService)
        {
            this.attendenceService = attendenceService;
            this.employeeService = employeeService;
        }
        [Authorize(Roles ="HR")]
        public async Task<IActionResult> Index()
        {
            var AttendanceList = await attendenceService.getAttendenceList();
            return View(AttendanceList);
        }

    }
}
