using AMS.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static AMS.Auth_IdentityModel.IdentityModel;

namespace AMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // =====================================================
        // REGISTER - GET
        // =====================================================
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        // =====================================================
        // REGISTER - POST
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim();
            string fullName = model.FullName.Trim();
            string phone = model.PhoneNumber?.Trim() ?? string.Empty;
            string address = model.Address?.Trim() ?? string.Empty;
            string accountType = model.AccountType.Trim();

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
                return View(model);
            }

            string[] allowedRoles = { "Advocate", "Client", "Administrator" };
            var matchedRole = allowedRoles.FirstOrDefault(r => r.Equals(accountType, StringComparison.OrdinalIgnoreCase));

            if (matchedRole == null)
            {
                ModelState.AddModelError(nameof(model.AccountType), "Please select a valid account type.");
                return View(model);
            }

            accountType = matchedRole;

            var roleExists = await _roleManager.RoleExistsAsync(accountType);
            if (!roleExists)
            {
                var role = new Role(accountType)
                {
                    Description = $"{accountType} account",
                    StatusId = 1,
                    CreatedBy = 0,
                    CreatedDateUtc = DateTimeOffset.UtcNow
                };

                var createRoleResult = await _roleManager.CreateAsync(role);
                if (!createRoleResult.Succeeded)
                {
                    foreach (var error in createRoleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            var user = new User
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Phone = phone,
                Address = address,
                RegisterDate = DateTime.UtcNow,
                CreatedBy = 0,
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedBy = null,
                UpdatedDate = null
            };

            var createUserResult = await _userManager.CreateAsync(user, model.Password);

            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var assignRoleResult = await _userManager.AddToRoleAsync(user, accountType);

            if (!assignRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                foreach (var error in assignRoleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // সফলভাবে রেজিস্ট্রেশনের পর সাইন-ইন করিয়ে সরাসরি Dashboard-এ পাঠাবে
            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["SuccessMessage"] = "Your account has been created successfully.";

            return RedirectToAction("Index", "Dashboard");
        }

        // =====================================================
        // LOGIN - GET
        // =====================================================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // =====================================================
        // LOGIN - POST
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim();
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );

            if (loginResult.Succeeded)
            {
                // নির্দিষ্ট returnUrl থাকলে সেখানে পাঠাবে (যদি তা শুধু Home পেজ না হয়)
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl != "/")
                {
                    return Redirect(returnUrl);
                }

                // অন্যথায় সরাসরি Dashboard Controller-এর Index পেজে পাঠাবে
                return RedirectToAction("Index", "Dashboard");
            }

            if (loginResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account has been locked due to multiple failed attempts. Please try again later.");
                return View(model);
            }

            if (loginResult.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Login is not allowed for this account. Please verify your account.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        // =====================================================
        // LOGOUT
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login", "Account");
        }

        // =====================================================
        // ACCESS DENIED
        // =====================================================
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}