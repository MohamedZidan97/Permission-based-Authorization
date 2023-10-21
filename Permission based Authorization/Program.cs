using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Permission_based_Authorization.DAL;
using Permission_based_Authorization.BL.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DI To Connection String \
var connectionstring = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContextPool<ApplicationDbContext>(opt => opt.UseSqlServer(connectionstring));

// DI To Identity
//Instance of Identity Usres and Roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
    {
        // Default Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    }
    ).AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultUI();

var app = builder.Build();

// Seed Defualt Roles & Users

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var loggerfactory = services.GetRequiredService<ILoggerProvider>();
var logger = loggerfactory.CreateLogger("App");

try
{
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DefualtRoles.SeedRoles(roleManager);
    await DefualtUsers.SeedSuperAdminUserAsync(userManager, roleManager);

    logger.LogInformation("Data Seeded");
    logger.LogInformation("Application Started");

}
catch (Exception ex)
{
    logger.LogWarning(ex, "An error occurred while seeding data");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
