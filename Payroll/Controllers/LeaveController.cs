using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Payroll.Application.Services.ServiceImplementation;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;
using Payroll.Web.StaticValues;
using System.Security.Claims;

namespace Payroll.Web.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveService leaveService;
        private readonly IEmployeeService employeeService;
        private readonly IEmailServiceInterface emailService;
        private readonly UserManager<ApplicationUser> userManager;

        public LeaveController(ILeaveService leaveService,IEmployeeService employeeService,IEmailServiceInterface emailService,UserManager<ApplicationUser> userManager)
        {
            this.leaveService = leaveService;
            this.employeeService = employeeService;
            this.emailService = emailService;
            this.userManager = userManager;
         
        }
        [Authorize(Roles ="Admin,HR")]
        public async Task< IActionResult> Index()
        {
            var leaveList=await leaveService.getAllLeaves();
            return View(leaveList);
        }
        [Authorize(Roles ="Employee")]
        public IActionResult RequestLeave()
        {
            ViewBag.LeaveType = StaticValues.StaticValues.LeaveType
    .Select(l => new SelectListItem { Text = l, Value = l })
    .ToList();
            return View();
        }
   
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> RequestLeave(Leave leave)
        {
            var user = await userManager.GetUserAsync(User);
            leave.EmployeeId = employeeService.getEmpId(user.Id);
            ViewBag.LeaveType = StaticValues.StaticValues.LeaveType
                .Select(l => new SelectListItem { Text = l, Value = l })
                .ToList();
            leave.Status = "Pending";
            if (!ModelState.IsValid)
            {
                return View(leave); 
            }
            await leaveService.applyLeave(leave);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles ="HR")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            var leave = await leaveService.getIndividuaLeave(id);
            ViewBag.LeaveStatus = StaticValues.StaticValues.LeaveStatus.Select(l => new SelectListItem { Text=l,Value=l });
            if(leave==null)
            {
                return NotFound();
            }
            else
            {
                return View(leave);
            }
        }
      
        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ApproveLeave(int id, string status,string remarks)
        {
            var leave = await leaveService.getIndividuaLeave(id);
            ViewBag.LeaveStatus = StaticValues.StaticValues.LeaveStatus.Select(l => new SelectListItem { Text = l, Value = l });
            if (leave == null)
            {
                return NotFound(); 
            }
            leave.Status= status;
            leave.Remarks = remarks;
            await leaveService.approveLeave(leave);
            await leaveService.LeaveApproveMail();
            return RedirectToAction("Index"); 
        }
        public async Task<IActionResult> EmployeeLeaves(int id)
        {
            var leaves = await leaveService.getIndividualEmployeeLeave(id);
            if (leaves == null)
            {
                return NotFound();
            }
            else
            {
                return View(leaves);
            }

        }

    }
}
