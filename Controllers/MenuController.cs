using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AustellAcademyAdmissions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;


namespace AustellAcademyAdmissions.Controllers
{


    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MenuController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.OrderBy(m => m.Order).ToListAsync();
            return View(menus);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ParentMenus = new SelectList(_context.Menus.Where(m => m.ParentId == null), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParentMenus = new SelectList(_context.Menus.Where(m => m.ParentId == null), "Id", "Name", menu.ParentId);
            return View(menu);
        }



        [HttpGet]
        public IActionResult Create11() => View();

        [HttpPost]
        public async Task<IActionResult> Create11(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        [HttpGet]
        public async Task<IActionResult> Edit11(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null) return NotFound();
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit11(Menu menu)
        {
            if (!ModelState.IsValid)
            {
                _context.Update(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var menu = await _context.Menus
                .Include(m => m.SubMenus)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            ViewBag.ParentMenus = new SelectList(_context.Menus.Where(m => m.ParentId == null), "Id", "Title");

            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Menus.Any(e => e.Id == menu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParentMenus = new SelectList(_context.Menus.Where(m => m.ParentId == null), "Id", "Name", menu.ParentId);
            return View(menu);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }



    }

}
