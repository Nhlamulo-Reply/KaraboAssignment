using KaraboAssignment.Data;
using KaraboAssignment.Enums;
using KaraboAssignment.Helpers;
using KaraboAssignment.Service;
using KaraboAssignment.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace KaraboAssignment.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserDataManagement _usersIO;

        public AccountController(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IUserDataManagement usersIO)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _usersIO = usersIO;
        }

        public async Task<IActionResult> Login()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = model.Email?.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Email is required.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _usersIO.GetUserByEmail(email);

                if (user == null)
                {
                    _logger.LogWarning("User not found after successful login attempt: {Email}", email);
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }

                var userRole = await _usersIO.GetUserRoleByEmail(email);

                if (string.IsNullOrWhiteSpace(userRole))
                {
                    _logger.LogWarning("Role not assigned for user: {Email}", email);
                    ModelState.AddModelError(string.Empty, "User role not found.");
                    return View(model);
                }

                _logger.LogInformation("User {Email} with role {Role} logged in successfully.", email, userRole);

                if (userRole == UserRole.Farmers.GetDisplayName())
                    return RedirectToAction("Index", "Dashboard");

                if (userRole == UserRole.Admin.GetDisplayName())
                    return RedirectToAction("AdminIndex", "Dashboard");

                _logger.LogWarning("User {Email} has unknown role: {Role}", email, userRole);
                ModelState.AddModelError(string.Empty, "Unknown user role.");
                return View(model);
            }

            if (result.RequiresTwoFactor)
            {
                _logger.LogWarning("2FA required for user {Email}.", email);
                ModelState.AddModelError(string.Empty, "Two-factor authentication required (not implemented).");
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User account {Email} is locked out.", email);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid login attempt for {Email}.", email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }


        public async Task<IActionResult> Register()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(registerViewModel.Password))
                {
                    if (!Validators.IsValidPassword(registerViewModel.Password))
                    {
                        ModelState.AddModelError(string.Empty, "Password must 8 or more characters, upper case, lowecase, number, special characters!");
                        return View(registerViewModel);
                    }
                }
                IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    var userId = await _usersIO.CreateUser(registerViewModel);
                    //await _walletManager.CreateUserWallet(ViewModelBuilder.CreateNewUserWallet(userId));

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"There was an error registering new account with email {registerViewModel.Email}");
                }
                //return View();
                var result = await _signInManager.PasswordSignInAsync(registerViewModel.Email, registerViewModel.Password, false, lockoutOnFailure: false);
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Registerss()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registerss(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = model.Email?.Trim();
                string password = model.Password;

                if (!Validators.IsValidEmail(email))
                {
                    ModelState.AddModelError(nameof(model.Email), "Invalid email format.");
                    _logger.LogWarning("Registration failed: Invalid email format {Email}.", email);
                    return View(model);
                }

                if (!Validators.IsValidPassword(password))
                {
                    ModelState.AddModelError(nameof(model.Password), "Password must be 8+ characters, include uppercase, lowercase, number, and special character.");
                    _logger.LogWarning("Registration failed: Weak password for {Email}.", email);
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} registered successfully.", email);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogError("Registration error for {Email}: {Error}", email, error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

    }
}
