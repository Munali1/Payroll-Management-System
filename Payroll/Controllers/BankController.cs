using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;
namespace Payroll.Web.Controllers
{
    public class BankController : Controller
    {
        private readonly IBankDetailsService bankDetailsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployeeService employeeService;

        public BankController(IBankDetailsService bankDetailsService,UserManager<ApplicationUser> userManager,IEmployeeService employeeService)
        {
            this.bankDetailsService = bankDetailsService;
            this.userManager = userManager;
            this.employeeService = employeeService;
        }
        [Authorize(Roles ="Admin,HR")]
        public async Task<IActionResult> Index()
        {
            var bankList = await bankDetailsService.GetBankList();
            return View(bankList);
        }
        public async Task<IActionResult> Details(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var bankDetails = await bankDetailsService.GetById(id);
            bankDetails.AccountHolderName = user.FirstName + user.LastName;
            return View(bankDetails);
        }
        [Authorize(Roles ="Employee")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BankDetails bankDetails)
        {

            var user=await userManager.GetUserAsync(User);
            bankDetails.EmployeeId = employeeService.getEmpId(user.Id);
            bankDetails.AccountHolderName = employeeService.getName(user.Id);
            if (ModelState.IsValid)
            { 
                await bankDetailsService.Create(bankDetails);
                return RedirectToAction("Index");
            }
            return View(bankDetails);
        }
        public  async Task<IActionResult> Edit(int id)
        {
            var bank = await bankDetailsService.GetById(id);

            return View(bank);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(BankDetails bank)
        {
            var bankDetails = await bankDetailsService.GetById(bank.Id);
            if(bankDetails == null)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await bankDetailsService.Update(bankDetails);
                    return RedirectToAction("Index");
                }
            }
            return View(bankDetails);   
        }
        public async Task<IActionResult> Delete(int id)
        {
            var bank = await bankDetailsService.GetById(id);
            if (bank != null)
            {
                await bankDetailsService.Delete(id);
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
         
        }
    }
}
