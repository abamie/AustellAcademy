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


    public class ClassController : Controller
    {


        private readonly ApplicationDbContext _context;
        public ClassController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes.ToListAsync();
            return View(classes);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Class model)
        {
            if (ModelState.IsValid)
            {
                _context.Classes.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existingClass = await _context.Classes.FindAsync(id);
            if (existingClass == null)
            {
                return NotFound();
            }
            return View(existingClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Class model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Classes.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


    }

}