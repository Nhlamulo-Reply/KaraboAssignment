using KaraboAssignment.Data;
using KaraboAssignment.Enums;
using KaraboAssignment.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext + Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register services
builder.Services.AddScoped<IUserDataManagement, UserDataManagement>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add controllers and views
builder.Services.AddControllersWithViews();

builder.Services.AddLogging();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        await CreateRolesAsync(roleManager, userManager, dbContext);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database seeding failed!");
    }
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/// <summary>
/// This method creates roles if they don't already exist.
/// </summary>
/// <param name="roleManager">RoleManager for managing roles</param>
/// <param name="userManager">UserManager for managing users</param>


async Task CreateRolesAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
{
    try
    {
       
        var roleNames = Enum.GetNames(typeof(UserRole));
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            }
        }


        var adminEmail = "admin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                Firstname = "Admin",
                Lastname = "User",
                PhoneNumber = "+1234567890",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

          
            dbContext.Users.Add(adminUser);
            await dbContext.SaveChangesAsync();

         
            var dbAdmin = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (dbAdmin == null)
            {
                throw new InvalidOperationException("Admin user not found after creation.");
            }

           
            var passwordResult = await userManager.AddPasswordAsync(dbAdmin, "Admin@123");
            if (!passwordResult.Succeeded)
            {
                throw new Exception($"Failed to set admin password: {string.Join(", ", passwordResult.Errors)}");
            }

            var roleResult = await userManager.AddToRoleAsync(dbAdmin, UserRole.Employees.ToString());
            if (!roleResult.Succeeded)
            {
                throw new Exception($"Failed to add admin to Employee role: {string.Join(", ", roleResult.Errors)}");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Program>();
        logger.LogError(ex, "An error occurred during database seeding.");
        throw;
    }
}