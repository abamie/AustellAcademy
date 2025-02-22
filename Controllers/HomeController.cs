using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AustellAcademyAdmissions.Models;
using System.Linq;

namespace AustellAcademyAdmissions.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var notices = _context.Contents
                .OrderByDescending(n => n.CreatedAt)
                .Take(15) // Show latest 5 notices
                .ToList();

            SchoolViewModel svm = new SchoolViewModel();
            svm.Contents = notices;

            return View(svm);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}

