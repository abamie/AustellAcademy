using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AustellAcademyAdmissions.Models;



namespace AustellAcademyAdmissions.Controllers
{
public class ClassRoutineController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClassRoutineController(ApplicationDbContext context)
    {
        _context = context;
    }

   /* 
   public async Task<IActionResult> Index()
    {
        var routines = await _context.ClassRoutines.ToListAsync();
        return View(routines);
    } 
    */


public IActionResult Index()
{
    var routines = _context.ClassRoutines.ToList();
    var groupedRoutines = routines.GroupBy(r => r.ClassName)
                                  .ToDictionary(g => g.Key, g => g.ToList());

    return View(groupedRoutines);
}

public IActionResult SeedData()
{
    var routines = new List<ClassRoutine>
    {
        new ClassRoutine { ClassName = "Class-1", Subject = "Geography", Teacher = "Ms. Adams", DayOfWeek = "Friday", StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
        new ClassRoutine { ClassName = "Class-2", Subject = "Physics", Teacher = "Dr. Wilson", DayOfWeek = "Monday", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(1, 0, 0) }
    };

    _context.ClassRoutines.AddRange(routines);
    _context.SaveChanges();
    
    return RedirectToAction("Index");
}
}
}