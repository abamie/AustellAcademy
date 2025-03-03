using System.ComponentModel.DataAnnotations;

namespace AustellAcademyAdmissions.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public required string ClassName { get; set; } // Example: "Grade 1", "Grade 2"
        public string? Description { get; set; }
    }

}