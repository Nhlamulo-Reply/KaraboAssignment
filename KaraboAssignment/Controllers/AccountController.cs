using KaraboAssignment.Data;
using KaraboAssignment.Enums;
using KaraboAssignment.Helpers;
using KaraboAssignment.Service;
using KaraboAssignment.Models;
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
                    return RedirectToAction("AdminIndex", "Dashboard");

                if (userRole == UserRole.Employees.GetDisplayName())
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

                if (!string.IsNullOrEmpty(registerViewModel.PhoneNumber))
                {
                    if (!Validators.IsValidCellphone(registerViewModel.PhoneNumber))
                    {
                        ModelState.AddModelError(string.Empty, "Enter a valid south african  phone number");
                        return View(registerViewModel);
                    }
                }


                if (!string.IsNullOrEmpty(registerViewModel.Email))
                {
                    if (!Validators.IsValidEmail(registerViewModel.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Enter a valid email address  phone number");
                        return View(registerViewModel);
                    }
                }

                IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    var userId = await _usersIO.CreateUser(registerViewModel);
               

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"There was an error registering new account with email {registerViewModel.Email}", ex);
                }
                //return View();
                var result = await _signInManager.PasswordSignInAsync(registerViewModel.Email, registerViewModel.Password, false, lockoutOnFailure: false);
                return RedirectToAction("AdminIndex", "Dashboard");
            }
            return View();
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
