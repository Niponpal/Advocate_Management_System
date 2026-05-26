using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class CaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
