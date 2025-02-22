using System.Collections.Generic;
using AustellAcademyAdmissions.Models; // Ensure the namespace is correct

namespace AustellAcademyAdmissions.Models
{
    public class SchoolViewModel
    {
        public IEnumerable<Content> Contents { get; set; } // Change 'content' to 'Content'
    }
}
