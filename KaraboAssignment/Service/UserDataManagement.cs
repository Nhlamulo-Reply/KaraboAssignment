using KaraboAssignment.Controllers;
using KaraboAssignment.Data;
using KaraboAssignment.Enums;
using KaraboAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KaraboAssignment.Service
{
    public class UserDataManagement : IUserDataManagement
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public UserDataManagement(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<string> CreateUser(UserDetailsViewModel userViewModel)
        {
            var user = new ApplicationUser
            {
                Firstname = userViewModel.Firstname,
                Lastname = userViewModel.Lastname,
                PhoneNumber = userViewModel.PhoneNumber,
                UserName = userViewModel.Email,
                Email = userViewModel.Email,
                //AccountStatus = userViewModel.AccountStatus
            };

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userViewModel.Email);
            var res = await _userManager.AddPasswordAsync(dbUser, userViewModel.Password);
            await _userManager.AddToRoleAsync(dbUser, userViewModel.UserRole);

            return dbUser.Id;
        }

        public async Task<string> CreateUser(RegisterViewModel registerViewModel)
        {
            var user = new ApplicationUser
            {
                Firstname = registerViewModel.Firstname,
                Lastname = registerViewModel.Lastname,
                PhoneNumber = registerViewModel.PhoneNumber,
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
               // AccountStatus = AccountStatus.Active
            };

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == registerViewModel.Email);

            var res = await _userManager.AddPasswordAsync(dbUser, registerViewModel.Password);

            await _userManager.AddToRoleAsync(dbUser, UserRole.Client.GetDisplayName());

            return dbUser.Id;
        }

        public async Task EditUser(UserDetailsViewModel userViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDetailsViewModel>> GetAllUsersDetails()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public Task<ApplicationUser> GetUserByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetUserRole(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("GetUserRole called with null or empty userId.");
                return null;
            }

            var userRoleEntity = await _dbContext.ApplicationUserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userRoleEntity == null)
            {
                _logger.LogWarning($"No ApplicationUserRole found for user ID: {userId}");
                return null;
            }

            var role = await _roleManager.FindByIdAsync(userRoleEntity.RoleId);

            if (role == null)
            {
                _logger.LogWarning($"No role found for RoleId: {userRoleEntity.RoleId}");
                return null;
            }

            return role.Name;
        }

        public async Task<string?> GetUserRoleByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("GetUserRoleByEmail called with null or empty email.");
                return null;
            }

            var user = await GetUserByEmail(email);

            if (user == null)
            {
                _logger.LogWarning($"No user found with email: {email}");
                return null;
            }

            return await GetUserRole(user.Id);
        }


        public Task SuperDeleteUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
