using AustellAcademyAdmissions.Models;
using AustellAcademyAdmissions.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<RazorpayService>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Register Email Service
builder.Services.AddTransient<EmailService>();

// Add MVC Support
builder.Services.AddControllersWithViews();



var app = builder.Build();

// Ensure Roles are Created at Startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}

// Ensure menus are seeded
SeedMenu(app.Services);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


void SeedMenu(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply pending migrations
        context.Database.Migrate();

        if (!context.Menus.Any())
        {
            // Create Parent Menus first
            var admissionsMenu = new Menu { Title = "Admissions", Url = "#", Roles = "Admin", Order = 2 };
            var studentsMenu = new Menu { Title = "Students", Url = "#", Roles = "Admin,Teacher", Order = 5 };

            context.Menus.AddRange(new List<Menu>
            {
                new Menu { Title = "Dashboard", Url = "/Dashboard", Roles = "Admin,Teacher,Student", Order = 1 },
                admissionsMenu, // Parent Menu
                studentsMenu,  // Parent Menu
                new Menu { Title = "Courses", Url = "/Courses", Roles = "Teacher", Order = 7 }
            });

            context.SaveChanges(); // Save to generate IDs for parent menus

            // Now add submenus referencing parent IDs
            context.Menus.AddRange(new List<Menu>
            {
                new Menu { Title = "New Admission", Url = "/Admissions/Create", Roles = "Admin", Order = 3, ParentId = admissionsMenu.Id },
                new Menu { Title = "Manage Admissions", Url = "/Admissions", Roles = "Admin", Order = 4, ParentId = admissionsMenu.Id },
                new Menu { Title = "All Students", Url = "/Students", Roles = "Admin,Teacher", Order = 6, ParentId = studentsMenu.Id }
            });

            context.SaveChanges();
        }
    }
}


// ✅ Role Creation Function
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roleNames = { "Admin", "Teacher", "Student" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create Default Admin User
    var adminEmail = "admin@austellacademy.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var user = new IdentityUser { UserName = adminEmail, Email = adminEmail };
        var createUser = await userManager.CreateAsync(user, "Admin@123");

        if (createUser.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

