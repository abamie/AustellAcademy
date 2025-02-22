using System.Collections.Generic;
namespace AustellAcademyAdmissions.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Content> Contents { get; set; }
    }
}
