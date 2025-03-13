using Microsoft.AspNetCore.Mvc;

namespace Payroll.Web.Controllers
{
    public class SalaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
