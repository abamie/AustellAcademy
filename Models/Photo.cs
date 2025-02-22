
namespace AustellAcademyAdmissions.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public string Category { get; set; } // Example: "nature", "events", "classroom"
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
