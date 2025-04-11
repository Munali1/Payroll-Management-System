using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public LeaveController(ILeaveService leaveService,IEmployeeService employeeService,IEmailServiceInterface emailService)
        {
            this.leaveService = leaveService;
            this.employeeService = employeeService;
            this.emailService = emailService;
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
            ViewBag.LeaveType=StaticValues.StaticValues.LeaveType;
            return View();
        }
        [HttpPost]
        public IActionResult RequestLeave(int id)
        {
            Leave leave = new Leave()
            {
                EmployeeId = id,
                Status = "Pending"
            };
            leaveService.applyLeave(leave);
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles ="HR")]
        public IActionResult ApproveLeave(int id)
        {
            var leave = leaveService.getIndividuaLeave(id);
            ViewBag.LeaveStatus=StaticValues.StaticValues.LeaveStatus;
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
        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ApproveLeave(int id, string status,string remarks)
        {
            var leave = await leaveService.getIndividuaLeave(id);
            ViewBag.LeaveStatus = StaticValues.StaticValues.LeaveStatus;
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

    }
}
