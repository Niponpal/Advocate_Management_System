using AMS.Helper;
using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static AMS.Auth_IdentityModel.IdentityModel;

namespace AMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdvocateRepository _advocateRepository;
        private readonly ISignInHelper _signInHelper;
        private readonly UserManager<User> _userManager;
        private readonly IAppointmentRepository _appointmentRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IAdvocateRepository advocateRepository,
            ISignInHelper signInHelper,
            UserManager<User> userManager,
            IAppointmentRepository appointmentRepository)
        {
            _logger = logger;
            _advocateRepository = advocateRepository;
            _signInHelper = signInHelper;
            _userManager = userManager;
            _appointmentRepository = appointmentRepository;
        }

        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }

        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View("404");
        }
    }
}
