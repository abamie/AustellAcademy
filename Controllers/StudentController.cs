using AustellAcademyAdmissions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AustellAcademyAdmissions.Controllers
{
[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }
}
}
