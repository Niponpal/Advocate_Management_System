using AMS.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static AMS.Auth_IdentityModel.IdentityModel;

namespace AMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // =========================================================
        // REGISTER - GET
        // =========================================================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // =========================================================
        // REGISTER - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // ---------------------------------------------
            // Validate Model
            // ---------------------------------------------

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ---------------------------------------------
            // Check Existing Email
            // ---------------------------------------------

            var existingUser = await _userManager.FindByEmailAsync(
                model.Email.Trim()
            );

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "An account with this email address already exists."
                );

                return View(model);
            }

            // ---------------------------------------------
            // Validate Account Type
            // ---------------------------------------------

            var accountType = model.AccountType?.Trim();

            if (string.IsNullOrWhiteSpace(accountType))
            {
                ModelState.AddModelError(
                    nameof(model.AccountType),
                    "Please select an account type."
                );

                return View(model);
            }

            // Only Buyer or Seller is allowed
            if (!accountType.Equals(
                    "Buyer",
                    StringComparison.OrdinalIgnoreCase) &&
                !accountType.Equals(
                    "Seller",
                    StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    nameof(model.AccountType),
                    "Invalid account type. Please select Buyer or Seller."
                );

                return View(model);
            }

            // Normalize account type
            accountType = accountType.Equals(
                "Buyer",
                StringComparison.OrdinalIgnoreCase)
                ? "Buyer"
                : "Seller";

            // =====================================================
            // CREATE ROLE IF IT DOES NOT EXIST
            // =====================================================

            var roleExists = await _roleManager.RoleExistsAsync(
                accountType
            );

            if (!roleExists)
            {
                var role = new Role(accountType)
                {
                    Description = $"{accountType} account",
                    StatusId = 1,
                    CreatedBy = 0,
                    CreatedDateUtc = DateTimeOffset.UtcNow
                };

                var createRoleResult =
                    await _roleManager.CreateAsync(role);

                if (!createRoleResult.Succeeded)
                {
                    foreach (var error in createRoleResult.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description
                        );
                    }

                    return View(model);
                }
            }

            // =====================================================
            // CREATE USER
            // =====================================================

            var user = new User
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),

                FullName = model.FullName.Trim(),

                // Your User model uses Phone
                // while RegisterViewModel uses PhoneNumber
                Phone = model.PhoneNumber.Trim(),

                Address = model.Address.Trim(),

                RegisterDate = DateTime.UtcNow,

                CreatedBy = 0,
                CreatedDate = DateTimeOffset.UtcNow,

                UpdatedBy = null,
                UpdatedDate = null
            };

            // ---------------------------------------------
            // Create Identity User
            // ---------------------------------------------

            var createUserResult = await _userManager.CreateAsync(
                user,
                model.Password
            );

            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(model);
            }

            // =====================================================
            // ASSIGN BUYER / SELLER ROLE
            // =====================================================

            var assignRoleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    accountType
                );

            if (!assignRoleResult.Succeeded)
            {
                // If role assignment fails,
                // remove the newly created user.
                await _userManager.DeleteAsync(user);

                foreach (var error in assignRoleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(model);
            }

            // =====================================================
            // SIGN IN AFTER REGISTRATION
            // =====================================================

            await _signInManager.SignInAsync(
                user,
                isPersistent: false
            );

            TempData["SuccessMessage"] =
                "Your account has been created successfully.";

            // =====================================================
            // REDIRECT
            // =====================================================

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        // =========================================================
        // LOGIN - GET
        // =========================================================

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // =========================================================
        // LOGIN - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // ---------------------------------------------
            // Validate Model
            // ---------------------------------------------

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ---------------------------------------------
            // Find User By Email
            // ---------------------------------------------

            var user = await _userManager.FindByEmailAsync(
                model.Email.Trim()
            );

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password."
                );

                return View(model);
            }

            // =====================================================
            // PASSWORD LOGIN
            // =====================================================

            var loginResult =
                await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true
                );

            // =====================================================
            // LOGIN SUCCESS
            // =====================================================

            if (loginResult.Succeeded)
            {
                // Prevent Open Redirect
                if (!string.IsNullOrWhiteSpace(returnUrl) &&
                    Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }

            // =====================================================
            // ACCOUNT LOCKED
            // =====================================================

            if (loginResult.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account has been locked. Please try again later."
                );

                return View(model);
            }

            // =====================================================
            // LOGIN NOT ALLOWED
            // =====================================================

            if (loginResult.IsNotAllowed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Login is not allowed for this account."
                );

                return View(model);
            }

            // =====================================================
            // INVALID LOGIN
            // =====================================================

            ModelState.AddModelError(
                string.Empty,
                "Invalid email or password."
            );

            return View(model);
        }

        // =========================================================
        // LOGOUT
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["SuccessMessage"] =
                "You have been logged out successfully.";

            return RedirectToAction(
                "Login",
                "Account"
            );
        }

        // =========================================================
        // ACCESS DENIED
        // =========================================================

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}