using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AustellAcademyAdmissions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AustellAcademyAdmissions.Service;


namespace AustellAcademyAdmissions.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfService _pdfService;

       /* public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        } */


        public StudentsController(ApplicationDbContext context, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        // List Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.Include(s => s.Class).ToListAsync();
            return View(students);
        }

        // Create Student (GET)
        public IActionResult Create()
        {
            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName");
            return View(new Student());
        }

        // Create Student (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                student.EnrollmentDate = DateTime.Now;
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", student.ClassId);
            return View(student);
        }

        // Edit Student (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", student.ClassId);

            var model = new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                Address = student.Address,
                Gender = student.Gender,
                ClassId = student.ClassId,
                Status = student.Status
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = await _context.Students.FindAsync(model.Id);
                if (student == null) return NotFound();

                student.Name = model.Name;
                student.Email = model.Email;
                student.Phone = model.Phone;
                student.Address = model.Address;
                student.Gender = model.Gender;
                student.ClassId = model.ClassId;
                student.Status = model.Status;

                _context.Update(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Classes = new SelectList(_context.Classes, "Id", "ClassName", model.ClassId);
            return View(model);
        }


        // Edit Student (POST)
        // Delete Student
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Student Details
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students.Include(s => s.Class).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();

            return View(student);
        }
        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound();

               var pdfBytes = await _pdfService.GenerateStudentPdf(student);
            return File(pdfBytes, "application/pdf", $"{student.Name}_Details.pdf");
        }
    }


}