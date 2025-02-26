using Microsoft.AspNetCore.Identity;
using AustellAcademyAdmissions.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace AustellAcademyAdmissions.Service
{

    public class MenuService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MenuService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<List<Menu>> GetUserMenusAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);
            if (appUser == null) return new List<Menu>();

            var userRoles = (await _userManager.GetRolesAsync(appUser)).ToList();

            // Fetch data first (roles as string)
            var menus = await _context.Menus
                .Where(m => m.IsActive && m.IsVisible && !string.IsNullOrEmpty(m.Roles))
                .OrderBy(m => m.Order)
                .ToListAsync(); // Fetches data into memory

            // Process .Split(',') on the server-side
            return menus
                .Where(m => m.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Any(role => userRoles.Contains(role.Trim())))
                .Select(m => new Menu
                {
                    Id = m.Id,
                    Title = m.Title,
                    Url = m.Url,
                    Order = m.Order
                })
                .ToList();
        }

    }


}