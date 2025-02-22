using Microsoft.AspNetCore.Mvc;
using AustellAcademyAdmissions.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AustellAcademyAdmissions.Service;
using System;

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


       

        // GET: Admissions/Apply
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }



[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Apply(Student student, IFormFile Document)
{
    if (ModelState.IsValid==false)
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