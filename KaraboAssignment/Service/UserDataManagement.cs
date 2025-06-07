using KaraboAssignment.Controllers;
using KaraboAssignment.Data;
using KaraboAssignment.Enums;
using KaraboAssignment.Models;
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

           public async Task<Guid> CreatFarmer(RegisterViewModel farmer)
        {
            try
            {
                // 1. Create Identity User
                var user = new ApplicationUser
                {
                    UserName = farmer.Email,
                    Email = farmer.Email,
                    Firstname = farmer.Firstname,
                    Lastname = farmer.Lastname
             
                };

                var result = await _userManager.CreateAsync(user,farmer.Password);

                if (!result.Succeeded)
                    throw new Exception($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                await _userManager.AddToRoleAsync(user, UserRole.Farmers.GetDisplayName());

                var farmerEntity = new Farmer
                {
                    FarmerId = Guid.NewGuid(),
                    IdentityUserId = user.Id,
                    Name = farmer.Firstname ?? "Unknown",
                    Email = farmer.Email,
                    PhoneNumber = farmer.PhoneNumber,
                 
                };

                _dbContext.Farmers.Add(farmerEntity);
                await _dbContext.SaveChangesAsync();

                return farmerEntity.FarmerId;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create farmer", ex);
            }
        }

      
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

   





      
    }
}
