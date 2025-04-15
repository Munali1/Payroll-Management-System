using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Payroll.Application.Services.ServiceInterface;
using System.Security.Claims;

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
        [Authorize(Roles ="HR,Admin")]
        public async Task<IActionResult> Index()
        {
            var AttendanceList = await attendenceService.getAttendenceList();
            var TodayList = AttendanceList.Where(a => a.inTime.HasValue &&
                 a.inTime.Value.Month == DateTime.Today.Month &&
                 a.inTime.Value.Year == DateTime.Today.Year)
                 .ToList();
            return View(TodayList);
        }
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetPunchInOutStatus(int employeeId)
        {
            var latestAttendance = await attendenceService.getLatest(employeeId); 
            return PartialView("_PunchInOut", latestAttendance);
        }
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> PunchIn()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int id = employeeService.getEmpId(userId);
            await attendenceService.PunchIn(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [Authorize(Roles = " Employee")]
        public async Task<IActionResult> PunchOut()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int id = employeeService.getEmpId(userId);
            await attendenceService.PunchOut(id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> getTotalWorkingHours(int EmpId)
        {
            var totalWorkingHours = await attendenceService.getTotalWorkingHours(EmpId);
            var hours = totalWorkingHours.Hours;
            var minutes = totalWorkingHours.Minutes;
            return Json(new { hours, minutes });
        }
        public async Task<IActionResult> Details(int id)
        {
            var EmpAttendence=await attendenceService.getMonthlyAttendence(id);
            return View(EmpAttendence);
        }



    }
}
