using AustellAcademyAdmissions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;  // ✅ Add this
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;  // ✅ Add this
using System;
using System.Threading.Tasks;

namespace AustellAcademyAdmissions.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;  // ✅ Add RoleManager
        private readonly UserManager<IdentityUser> _userManager;  // ✅ Add UserManager

        public AdminController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        

        public async Task<IActionResult> Applications()
        {
            var applications = await _context.Applications
                .Include(a => a.admission)
                .ToListAsync();
            return View(applications);
        }

        public IActionResult Index()
        {
            return View();
        }

        // ✅ Fix: Pass RoleManager & UserManager to CreateRoles
        public async Task<IActionResult> CreateRoles()
        {
            string[] roleNames = { "Admin", "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // ✅ Create default admin user
            var adminEmail = "admin@austellacademy.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var createUser = await _userManager.CreateAsync(user, "Admin@123");

                if (createUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            return Ok("Roles and Admin created successfully!");
        }



        
    }
}
