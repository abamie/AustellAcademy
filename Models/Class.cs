namespace AustellAcademyAdmissions.Models
{
    public class Class
    {
        public int Id { get; set; }
        public required string ClassName { get; set; } // Example: "Grade 1", "Grade 2"
        public string? Description { get; set; }
    }

}