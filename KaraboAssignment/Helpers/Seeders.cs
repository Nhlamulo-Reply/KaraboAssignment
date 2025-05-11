using KaraboAssignment.Data;
using KaraboAssignment.Enums;
using Microsoft.AspNetCore.Identity;

namespace KaraboAssignment.Helpers
{
    public class Seeders
    {
        public static async Task CreateRolesAsync(WebApplication app)
        {
            var roleManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = app.Services.GetRequiredService<UserManager<ApplicationUser>>();

            var roleNames = Enum.GetNames(typeof(UserRole));

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
