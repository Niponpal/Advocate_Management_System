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
            return View();
        }


        // =====================================================
        // REGISTER - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            // -----------------------------------------------
            // Model Validation
            // -----------------------------------------------

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // -----------------------------------------------
            // Clean Data
            // -----------------------------------------------

            string email = model.Email.Trim();

            string fullName = model.FullName.Trim();

            string phone = model.PhoneNumber.Trim();

            string address = model.Address.Trim();

            string accountType = model.AccountType.Trim();


            // -----------------------------------------------
            // Check Existing Email
            // -----------------------------------------------

            var existingUser =
                await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "An account with this email already exists."
                );

                return View(model);
            }


            // =================================================
            // VALIDATE ACCOUNT TYPE
            // =================================================

            string[] allowedRoles =
            {
                "Advocate",
                "Client",
                "Administrator"
            };


            if (!allowedRoles.Contains(
                    accountType,
                    StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    nameof(model.AccountType),
                    "Please select a valid account type."
                );

                return View(model);
            }


            // Normalize role name

            if (accountType.Equals(
                    "Advocate",
                    StringComparison.OrdinalIgnoreCase))
            {
                accountType = "Advocate";
            }
            else if (accountType.Equals(
                         "Client",
                         StringComparison.OrdinalIgnoreCase))
            {
                accountType = "Client";
            }
            else
            {
                accountType = "Administrator";
            }


            // =================================================
            // CREATE ROLE
            // =================================================

            var roleExists =
                await _roleManager.RoleExistsAsync(accountType);


            if (!roleExists)
            {
                var role = new Role(accountType)
                {
                    Description =
                        $"{accountType} account",

                    StatusId = 1,

                    CreatedBy = 0,

                    CreatedDateUtc =
                        DateTimeOffset.UtcNow
                };


                var createRoleResult =
                    await _roleManager.CreateAsync(role);


                if (!createRoleResult.Succeeded)
                {
                    foreach (var error in
                             createRoleResult.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description
                        );
                    }

                    return View(model);
                }
            }


            // =================================================
            // CREATE USER
            // =================================================

            var user = new User
            {
                UserName = email,

                Email = email,

                FullName = fullName,

                Phone = phone,

                Address = address,

                RegisterDate = DateTime.UtcNow,

                CreatedBy = 0,

                CreatedDate =
                    DateTimeOffset.UtcNow,

                UpdatedBy = null,

                UpdatedDate = null
            };


            // =================================================
            // CREATE IDENTITY USER
            // =================================================

            var createUserResult =
                await _userManager.CreateAsync(
                    user,
                    model.Password
                );


            if (!createUserResult.Succeeded)
            {
                foreach (var error in
                         createUserResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(model);
            }


            // =================================================
            // ASSIGN ROLE
            // =================================================

            var assignRoleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    accountType
                );


            if (!assignRoleResult.Succeeded)
            {
                // Remove user if role assignment fails

                await _userManager.DeleteAsync(user);


                foreach (var error in
                         assignRoleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return View(model);
            }


            // =================================================
            // SIGN IN
            // =================================================

            await _signInManager.SignInAsync(
                user,
                isPersistent: false
            );


            TempData["SuccessMessage"] =
                "Your account has been created successfully.";


            // =================================================
            // ROLE BASED REDIRECT
            // =================================================

            switch (accountType)
            {
                case "Administrator":

                    return RedirectToAction(
                        "Index",
                        "Admin"
                    );


                case "Advocate":

                    return RedirectToAction(
                        "Index",
                        "Advocate"
                    );


                case "Client":

                    return RedirectToAction(
                        "Index",
                        "Client"
                    );


                default:

                    return RedirectToAction(
                        "Index",
                        "Home"
                    );
            }
        }


        // =====================================================
        // LOGIN - GET
        // =====================================================

        [HttpGet]
        public IActionResult Login(
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }


        // =====================================================
        // LOGIN - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;


            if (!ModelState.IsValid)
            {
                return View(model);
            }


            string email = model.Email.Trim();


            // -----------------------------------------------
            // Find User
            // -----------------------------------------------

            var user =
                await _userManager.FindByEmailAsync(email);


            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password."
                );

                return View(model);
            }


            // -----------------------------------------------
            // Login
            // -----------------------------------------------

            var loginResult =
                await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true
                );


            // =================================================
            // SUCCESS
            // =================================================

            if (loginResult.Succeeded)
            {
                // Safe return URL

                if (!string.IsNullOrWhiteSpace(returnUrl) &&
                    Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }


                // ---------------------------------------------
                // Administrator
                // ---------------------------------------------

                if (await _userManager.IsInRoleAsync(
                        user,
                        "Administrator"))
                {
                    return RedirectToAction(
                        "Index",
                        "Admin"
                    );
                }


                // ---------------------------------------------
                // Advocate
                // ---------------------------------------------

                if (await _userManager.IsInRoleAsync(
                        user,
                        "Advocate"))
                {
                    return RedirectToAction(
                        "Index",
                        "Advocate"
                    );
                }


                // ---------------------------------------------
                // Client
                // ---------------------------------------------

                if (await _userManager.IsInRoleAsync(
                        user,
                        "Client"))
                {
                    return RedirectToAction(
                        "Index",
                        "Client"
                    );
                }


                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            // =================================================
            // LOCKED OUT
            // =================================================

            if (loginResult.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account has been locked. Please try again later."
                );

                return View(model);
            }


            // =================================================
            // NOT ALLOWED
            // =================================================

            if (loginResult.IsNotAllowed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Login is not allowed for this account."
                );

                return View(model);
            }


            // =================================================
            // INVALID LOGIN
            // =================================================

            ModelState.AddModelError(
                string.Empty,
                "Invalid email or password."
            );

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

            TempData["SuccessMessage"] =
                "You have been logged out successfully.";

            return RedirectToAction(
                "Login",
                "Account"
            );
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