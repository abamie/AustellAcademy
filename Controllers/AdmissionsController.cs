using Microsoft.AspNetCore.Mvc;
using AustellAcademyAdmissions.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AustellAcademyAdmissions.Service;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var admissions = await _context.Admissions.ToListAsync();
            return View(admissions);
        }


        // GET: Admissions/Apply
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var admission = await _context.Admissions.FindAsync(id);
            if (admission == null)
            {
                return NotFound();
            }

            var model = new AdmissionViewModel
            {
                Id = admission.Id,
                Name = admission.Name,
                Email = admission.Email,
                Phone = admission.Phone,
                Address = admission.Address,
                Gender = admission.Gender,
                ClassId = admission.ClassId,
                Status = admission.Status,
                DocumentPath = admission.DocumentPath // Store existing document path
            };

            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", admission.ClassId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdmissionViewModel model, IFormFile documentFile)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var admission = await _context.Admissions.FindAsync(id);
                if (admission == null)
                {
                    return NotFound();
                }

                // Update admission details
                admission.Name = model.Name;
                admission.Email = model.Email;
                admission.Phone = model.Phone;
                admission.Address = model.Address;
                admission.Gender = model.Gender;
                admission.ClassId = model.ClassId;
                admission.Status = model.Status;

                if (documentFile != null && documentFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(documentFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("documentFile", "Only PDF, JPG, and PNG files are allowed.");
                        ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", model.ClassId);
                        return View(model);
                    }

                    if (documentFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("documentFile", "File size must be less than 5MB.");
                        ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", model.ClassId);
                        return View(model);
                    }

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Delete old file if exists
                    if (!string.IsNullOrEmpty(admission.DocumentPath))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", admission.DocumentPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await documentFile.CopyToAsync(fileStream);
                    }

                    admission.DocumentPath = "/uploads/" + fileName;
                }
                else
                {
                    // Keep existing document if no new file is uploaded
                    admission.DocumentPath = _context.Admissions.AsNoTracking()
                        .Where(a => a.Id == admission.Id)
                        .Select(a => a.DocumentPath)
                        .FirstOrDefault();

                    // admission.DocumentPath = path == null ? string.Empty : path;

                }

                _context.Admissions.Update(admission);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", model.ClassId);
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Confirm(int id)
        {
            var admission = await _context.Admissions
                .Include(a => a.Class)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admission == null)
            {
                return NotFound();
            }

            return View(admission);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmAdmission(int id)
        {
            var admission = await _context.Admissions.FindAsync(id);
            if (admission == null)
            {
                return NotFound();
            }

            admission.Status = "Approved";
            _context.Admissions.Update(admission);
            await _context.SaveChangesAsync();

            // Send Email Confirmation
            string subject = "Admission Confirmation - Austell Academy";
            string message = $"Dear {admission.Name},<br/><br/>" +
                             "Congratulations! Your admission has been confirmed at Austell Academy.<br/><br/>" +
                             $"Class: {admission.Class?.ClassName}<br/>" +
                             "We look forward to welcoming you.<br/><br/>Best Regards,<br/>Austell Academy";

            await _emailService.SendEmailAsync(admission.Email, subject, message);

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Admission student, IFormFile Document)
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

                _context.Admissions.Add(student);
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

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName");
            return View(new AdmissionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdmissionViewModel model, IFormFile documentFile)
        {
            if (ModelState.IsValid)
            {
                var admission = new Admission
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    Gender = model.Gender,
                    ClassId = model.ClassId,
                    Status = model.Status,
                    DateOfBirth=model.DateOfBirth
                };

                if (documentFile != null && documentFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(documentFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await documentFile.CopyToAsync(fileStream);
                    }

                    admission.DocumentPath = "/uploads/" + fileName;
                }

                _context.Admissions.Add(admission);
                await _context.SaveChangesAsync();

                try
                {
                    var subject = "Application Submitted Successfully";
                    var body = $"Dear {admission.Name},\n\nThank you for applying to Austell Academy. We will review your application and get back to you shortly.";
                    await _emailService.SendEmailAsync(admission.Email, subject, body);
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    ModelState.AddModelError("", "Failed to send confirmation email. Please contact support.");
                    return View(admission);
                }

                return RedirectToAction("ApplicationSuccess", new { id = admission.Id });


                // return RedirectToAction(nameof(ApplicationSuccess));

                //return RedirectToAction(nameof(Index));
            }

            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", model.ClassId);
            return View(model);
        }




        // GET: Admissions/ApplicationSuccess
        public IActionResult ApplicationSuccess(int id)
        {
            var admission = _context.Admissions
                .Include(a => a.Class)
                .FirstOrDefault(a => a.Id == id);

            if (admission == null)
            {
                return NotFound();
            }

            var viewModel = new AdmissionViewModel
            {
                Name = admission.Name,
                Email = admission.Email,
                Phone = admission.Phone,
                Address = admission.Address,
                Gender = admission.Gender,
                ClassName = admission.Class.ClassName,
                Status = admission.Status,
                DocumentPath = admission.DocumentPath,
                Id=id,
                DateOfBirth=admission.DateOfBirth
            };

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> ConvertToStudent(int id)
        {
            var admission = await _context.Admissions.FindAsync(id);
            if (admission == null) return NotFound();

            if (admission.Status != "Approved")
            {
                return BadRequest("Only approved admissions can be converted to students.");
            }

            var student = new Student
            {
                Name = admission.Name,
                Email = admission.Email,
                Phone = admission.Phone,
                Address = admission.Address,
                Gender = admission.Gender,
                ClassId = admission.ClassId,
                EnrollmentDate = DateTime.Now,
                Status = "Active"
            };

            _context.Students.Add(student);
            _context.Admissions.Remove(admission); // Remove admission after conversion
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Students");
        }

        /*
                [HttpPost]
                public async Task<IActionResult> ConvertToStudent(int id)
                {
                    var admission = await _context.Admissions.FindAsync(id);
                    if (admission == null)
                    {
                        return NotFound();
                    }

                    if (admission.Status != "Approved")
                    {
                        return BadRequest("Only approved admissions can be converted to students.");
                    }

                    // Create a new student record
                    var student = new Student
                    {
                        Name = admission.Name,
                        Email = admission.Email,
                        Phone = admission.Phone,
                        Address = admission.Address,
                        Gender = admission.Gender,
                        ClassId = admission.ClassId,
                        EnrollmentDate = DateTime.Now
                    };

                    _context.Students.Add(student);
                    _context.Admissions.Remove(admission); // Optional: Remove admission record
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Students"); // Redirect to student list
                }

                */


    }
}