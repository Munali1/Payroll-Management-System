using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Payroll.Application.Interfaces;
using Payroll.Domain.Entities;

namespace Payroll.Web.Controllers
{
    public class BankController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BankController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [Authorize(Roles ="Admin,HR")]
        public async Task<IActionResult> Index()
        {
            var bankList = await unitOfWork.bankDetailsRepository.GetAllAsync(null, "Employee");
            return View(bankList);
        }
        public async Task<IActionResult> Details(int id)
        {
            var bankDetails = await unitOfWork.bankDetailsRepository.GetAsync(x => x.Id == id, "Employee");
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
            if (ModelState.IsValid)
            {
              unitOfWork.bankDetailsRepository.Add(bankDetails);
             
                return RedirectToAction("Index");
            }
            return View(bankDetails);
        }
        public  async Task<IActionResult> Edit(int id)
        {
            var bank = await unitOfWork.bankDetailsRepository.GetAsync(x => x.Id == id);

            return View(bank);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(BankDetails bank)
        {
            var bankDetails = await unitOfWork.bankDetailsRepository.GetAsync(x => x.Id == bank.Id);
            if(bankDetails == null)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.bankDetailsRepository.Update(bankDetails);
                    return RedirectToAction("Index");
                }
            }
            return View(bankDetails);   
        }
        public async Task<IActionResult> Delete(int id)
        {
            var bank = await unitOfWork.bankDetailsRepository.GetAsync(x => x.Id == id);
            unitOfWork.bankDetailsRepository.Remove(bank);
            return RedirectToAction("Index");
        }
    }
}
