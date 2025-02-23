using Microsoft.AspNetCore.Mvc;
using AustellAcademyAdmissions.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AustellAcademyAdmissions.Service;
using System;
using Microsoft.EntityFrameworkCore;

namespace AustellAcademyAdmissions.Controllers
{
    public class AdmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public AdmissionsController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var admissions = await _context.Students.ToListAsync();
            return View(admissions);
        }


        // GET: Admissions/Apply
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var admission = await _context.Students.FindAsync(id);
            if (admission == null)
            {
                return NotFound();
            }
            return View(admission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student admission, IFormFile documentFile)
        {
            if (id != admission.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    // If a new file is uploaded, replace the existing document
                    if (documentFile != null && documentFile.Length > 0)
                    {
                        // Define path to save file
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(documentFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        // Save file to server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await documentFile.CopyToAsync(fileStream);
                        }

                        // Update database with new document path
                        admission.DocumentPath = "/uploads/" + fileName;
                    }
                    else
                    {
                        // Keep existing document if no new file is uploaded
                        var path = _context.Students.AsNoTracking()
                            .Where(a => a.Id == admission.Id)
                            .Select(a => a.DocumentPath)
                            .FirstOrDefault();

                            admission.DocumentPath = path == null? string.Empty : path;

                    }

                    _context.Update(admission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Students.Any(e => e.Id == admission.Id))
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
            return View(admission);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Student student, IFormFile Document)
        {
            if (!ModelState.IsValid)
            {
                // Handle file upload
                if (Document != null && Document.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Document.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Document.CopyToAsync(fileStream);
                    }

                    // Save the file path to the database
                    student.DocumentPath = uniqueFileName;
                }

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // Send confirmation email
                try
                {
                    var subject = "Application Submitted Successfully";
                    var body = $"Dear {student.Name},\n\nThank you for applying to Austell Academy. We will review your application and get back to you shortly.";
                    await _emailService.SendEmailAsync(student.Email, subject, body);
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    ModelState.AddModelError("", "Failed to send confirmation email. Please contact support.");
                    return View(student);
                }

                return RedirectToAction(nameof(ApplicationSuccess));
            }

            // Log validation errors
            Console.WriteLine("Form submission failed due to validation errors.");
            return View(student);
        }



        // GET: Admissions/ApplicationSuccess
        public IActionResult ApplicationSuccess()
        {
            return View();
        }
    }
}