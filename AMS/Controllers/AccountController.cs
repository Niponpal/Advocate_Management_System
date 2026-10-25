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

        // ========================= REGISTER GET =========================
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new RegisterViewModel());
        }

        // ========================= REGISTER POST =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim();
            string fullName = model.FullName.Trim();
            string phone = model.PhoneNumber?.Trim() ?? string.Empty;
            string address = model.Address?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(model.AccountType))
            {
                ModelState.AddModelError(
                    nameof(model.AccountType),
                    "Please select an account type.");

                return View(model);
            }

            string requestedRole = model.AccountType.Trim();

            // Public registration is limited to Advocate and Client.
            // Create Administrator accounts through a secure admin process.
            string[] allowedRoles = { "Advocate", "Client" };

            string? accountType = allowedRoles.FirstOrDefault(
                role => role.Equals(
                    requestedRole,
                    StringComparison.OrdinalIgnoreCase));

            if (accountType == null)
            {
                ModelState.AddModelError(
                    nameof(model.AccountType),
                    "Please select a valid account type.");

                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "An account with this email already exists.");

                return View(model);
            }

            // Ensure the selected role exists.
            if (!await _roleManager.RoleExistsAsync(accountType))
            {
                var role = new Role(accountType)
                {
                    Description = $"{accountType} account",
                    StatusId = 1,
                    CreatedBy = 0,
                    CreatedDateUtc = DateTimeOffset.UtcNow
                };

                var roleResult = await _roleManager.CreateAsync(role);

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description);
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

            var createUserResult = await _userManager.CreateAsync(
                user,
                model.Password);

            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            var assignRoleResult = await _userManager.AddToRoleAsync(
                user,
                accountType);

            if (!assignRoleResult.Succeeded)
            {
                // Roll back user creation if role assignment fails.
                await _userManager.DeleteAsync(user);

                foreach (var error in assignRoleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            await _signInManager.SignInAsync(
                user,
                isPersistent: false);

            TempData["SuccessMessage"] =
                "Your account has been created successfully.";

            return RedirectToAction("Index", "Dashboard");
        }

        // ========================= LOGIN GET =========================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View(new LoginViewModel());
        }

        // ========================= LOGIN POST =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || string.IsNullOrWhiteSpace(user.UserName))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password.");

                return View(model);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            if (loginResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) &&
                    Url.IsLocalUrl(returnUrl) &&
                    returnUrl != "/")
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            if (loginResult.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account is locked due to multiple failed attempts. Please try again later.");

                return View(model);
            }

            if (loginResult.IsNotAllowed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Login is not allowed for this account. Please verify your account.");

                return View(model);
            }

            ModelState.AddModelError(
                string.Empty,
                "Invalid email or password.");

            return View(model);
        }

        // ========================= LOGOUT =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["SuccessMessage"] =
                "You have been logged out successfully.";

            return RedirectToAction("Index", "Home");
        }

        // ========================= ACCESS DENIED =========================
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
