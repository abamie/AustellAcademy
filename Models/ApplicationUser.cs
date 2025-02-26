using Microsoft.AspNetCore.Identity;

namespace AustellAcademyAdmissions.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties if needed
        public string FullName { get; set; }
    }
}