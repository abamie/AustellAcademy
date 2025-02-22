using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AustellAcademyAdmissions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AustellAcademyAdmissions.Controllers
{
    public class GalleryController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public GalleryController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            var photos = await _context.Photos.ToListAsync();
            return View(photos);
        }

        public IActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(PhotoUploadViewModel model)
        {
            if (model.Images == null || model.Images.Count == 0)
            {
                ModelState.AddModelError("Images", "Please select at least one image.");
                return View(model);
            }

            string uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in model.Images)
            {
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var photo = new Photo
                    {
                        ImagePath = "/uploads/" + fileName,
                        Caption = model.Caption,
                        Category = model.Category
                    };

                    _context.Photos.Add(photo);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var photo = _context.Photos.Find(id);
            if (photo == null) return NotFound();

            var model = new PhotoUploadViewModel
            {
                Id = photo.Id,
                CurrentImagePath = photo.ImagePath,
                Caption = photo.Caption,
                Category = photo.Category
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(PhotoUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var photo = _context.Photos.Find(model.Id);
                if (photo == null)
                {
                    return NotFound();
                }

                photo.Caption = model.Caption;
                photo.Category = model.Category;

                // If new images are uploaded, process them
                if (model.Images != null && model.Images.Count > 0)
                {
                    var uploadFolder = Path.Combine("wwwroot/images");
                    foreach (var file in model.Images)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Assuming you store only one image per record, update it
                        photo.ImagePath = "/images/" + fileName;
                    }
                }

                _context.Update(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }



        [HttpPost] // Ensure POST method is used
        [ValidateAntiForgeryToken] // Prevent CSRF attacks
        public async Task<IActionResult> Delete(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            // Remove photo file from the server
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", photo.ImagePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public IActionResult Manage()
        {
            var photos = _context.Photos.Select(p => new PhotoUploadViewModel
            {
                Id = p.Id,
                CurrentImagePath = p.ImagePath,
                Caption = p.Caption,
                Category = p.Category
            }).ToList();

            return View(photos);
        }

    }
}