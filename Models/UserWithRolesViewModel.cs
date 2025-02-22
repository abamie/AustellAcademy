using System.Collections.Generic;


namespace AustellAcademyAdmissions.Models
{
    public class UserWithRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
