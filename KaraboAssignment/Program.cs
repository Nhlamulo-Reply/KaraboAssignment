using KaraboAssignment.Data;
using KaraboAssignment.Enums;
using KaraboAssignment.Helpers;
using KaraboAssignment.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Call CreateRolesAsync here to ensure it's done after the application has started
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await CreateRolesAsync(roleManager, userManager); // Call the async method properly here
}

// Configure the HTTP request pipeline.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/// <summary>
/// This method creates roles if they don't already exist.
/// </summary>
/// <param name="roleManager">RoleManager for managing roles</param>
/// <param name="userManager">UserManager for managing users</param>
async Task CreateRolesAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
{
    try
    {
        var roleNames = Enum.GetNames(typeof(UserRole));

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var role = new ApplicationRole { Name = roleName };
                await roleManager.CreateAsync(role);
            }
        }
    }
    catch (Exception ex)
    {
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Program>();
        logger.LogError(ex, "An error occurred while creating roles.");
    }
}
