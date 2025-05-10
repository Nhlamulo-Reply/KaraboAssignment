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
        private readonly Farmer _farmer;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserDataManagement(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
         
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
               // AccountStatus = userViewModel.AccountStatus
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
                //  AccountStatus = AccountStatus.Active
            };

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == registerViewModel.Email);

            var res = await _userManager.AddPasswordAsync(dbUser, registerViewModel.Password);

            await _userManager.AddToRoleAsync(dbUser, UserRole.Farmers.GetDisplayName());

            return dbUser.Id;
        }


        public async Task<string> CreatFarmer(Farmer farmer)
        {
            var user = new Farmer
            {
                Name = farmer.Name,
                Email = farmer.Email,
              
            };

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            var dbUser = await _dbContext.Farmers.FirstOrDefaultAsync(x => x.Email == farmer.Email);

          return dbUser.FarmerId;
        }

        private async Task UpdateUserRole(ApplicationUser user, string newRole)
        {
            var userRole = await GetUserRole(user.Id);

            await _userManager.RemoveFromRoleAsync(user, userRole);
            await _userManager.AddToRoleAsync(user, newRole);
        }

        public async Task EditUser(UserDetailsViewModel userViewModel)
        {
            var dbUser = await GetUserByUserId(userViewModel.Id);

            dbUser.Firstname = userViewModel.Firstname;
            dbUser.Lastname = userViewModel.Lastname;
            dbUser.PhoneNumber = userViewModel.PhoneNumber;

            /* if (userViewModel.AccountStatus != 0)
           {
               dbUser.AccountStatus = userViewModel.AccountStatus;
           }

           if (dbUser.Email.ToLower() != userViewModel.Email.Trim().ToLower())
           {
               dbUser.Email = userViewModel.Email.Trim();
               dbUser.NormalizedEmail = dbUser.Email.ToUpper();
               dbUser.UserName = dbUser.Email;
               dbUser.NormalizedUserName = dbUser.UserName.ToUpper();
           }*/
            _dbContext.Update(dbUser);

            if (!string.IsNullOrEmpty(userViewModel.Password))
            {
                await _userManager.RemovePasswordAsync(dbUser);
                var res = await _userManager.AddPasswordAsync(dbUser, userViewModel.Password);

                if (!res.Succeeded)
                {
                    throw new Exception("password change failed: " + string.Join(",", res.Errors));
                }
            }

            if (!string.IsNullOrEmpty(userViewModel.UserRole))
            {
                await UpdateUserRole(dbUser, userViewModel.UserRole);
            }
            await _dbContext.SaveChangesAsync();
        }

        /*public async Task UpdateUserAccountStatus(string userId, AccountStatus accountStatus)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

          //  dbUser.AccountStatus = accountStatus;

            _dbContext.Update(dbUser);
            await _dbContext.SaveChangesAsync();
        }*/

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<ApplicationUser> GetUserByUserId(string userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return user;
        }

        public async Task<string> GetUserRole(string userId)
        {
            var allRoles = await _roleManager.Roles.ToListAsync();

            var userRoleId = (await _dbContext.ApplicationUserRoles.FirstOrDefaultAsync(x => x.UserId == userId)).RoleId;
            var userRole = allRoles.FirstOrDefault(x => x.Id == userRoleId).Name;

            return userRole;
        }

        public async Task<string> GetUserRoleByEmail(string email)
        {
            var user = await GetUserByEmail(email);

            return await GetUserRole(user.Id);
        }

        public async Task<List<UserDetailsViewModel>> GetAllUsersDetails()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var allRoles = await _roleManager.Roles.ToListAsync();
            var allUserRoles = await _dbContext.ApplicationUserRoles.ToListAsync();

            var usersViewModel = new List<UserDetailsViewModel>();

            foreach (var user in allUsers)
            {
                var userRoleId = allUserRoles.Where(c => c.UserId == user.Id).FirstOrDefault().RoleId;
                var userRole = allRoles.FirstOrDefault(x => x.Id == userRoleId).Name;

                var usrVm = new UserDetailsViewModel
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = userRole,
                   // AccountStatus = user.AccountStatus
                };
                usersViewModel.Add(usrVm);
            }

            return usersViewModel;
        }

        public async Task SuperDeleteUser(string userId)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var userRole = await GetUserRole(userId);

            await _userManager.RemoveFromRoleAsync(dbUser, userRole);

            _dbContext.Remove(dbUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}
