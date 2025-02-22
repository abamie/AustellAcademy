using AustellAcademyAdmissions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AustellAcademyAdmissions.Controllers
{

[Authorize(Roles = "Teacher")]
public class TeacherController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}
