using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AustellAcademyAdmissions.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public required string Name { get; set; }

        public virtual ICollection<Content> Contents { get; set; }
    }
}
