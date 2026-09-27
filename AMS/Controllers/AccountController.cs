using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static AMS.Auth_IdentityModel.IdentityModel;

namespace AMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

       public AccountController( SignInManager<User> signInManager, UserManager<User> userManager, IAuthService authService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
