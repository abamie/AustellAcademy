using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AustellAcademyAdmissions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AustellAcademyAdmissions.Controllers
{
    public class ContentController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContentController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Content content, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    content.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.Contents.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(content);
        }

        public async Task<IActionResult> Index()
        {
            var contents = await _context.Contents.Include(c => c.Category).ToListAsync();
            return View(contents);
        }

       

        public async Task<IActionResult> NoticeBoard()
        {
            var notices = await _context.Contents
                .Where(c => c.Category.Name == "Announcement")
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(notices);
        }

        public IActionResult Edit(int id)
        {
            var content = _context.Contents.Find(id);
            if (content == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", content.CategoryId);
            return View(content);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Content content, IFormFile ContentImage)
        {
            if (id != content.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                if (ContentImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ContentImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ContentImage.CopyToAsync(fileStream);
                    }
                    content.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.Update(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", content.CategoryId);
            return View(content);
        }


        public async Task<IActionResult> EditNotice(int id)
        {
            var notice = await _context.Contents.FindAsync(id);
            if (notice == null)
                return NotFound();

            return View(notice);
        }

        public IActionResult Delete(int id)
        {
            var content = _context.Contents.Find(id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                // Delete associated image file
                if (!string.IsNullOrEmpty(content.ImageUrl))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, content.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Contents.Remove(content);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditNotice(Content notice)
        {
            if (ModelState.IsValid)
            {
                _context.Contents.Update(notice);
                await _context.SaveChangesAsync();
                return RedirectToAction("NoticeBoard");
            }
            return View(notice);
        }

        public async Task<IActionResult> DeleteNotice(int id)
        {
            var notice = await _context.Contents.FindAsync(id);
            if (notice != null)
            {
                _context.Contents.Remove(notice);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("NoticeBoard");
        }

    }
}