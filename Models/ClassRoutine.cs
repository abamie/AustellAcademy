using System.ComponentModel.DataAnnotations;

namespace AustellAcademyAdmissions.Models
{
public class ClassRoutine
{
    [Key]
    public int Id { get; set; }
    
    public string ClassName { get; set; }
    public string Subject { get; set; }
    public string Teacher { get; set; }
    public string DayOfWeek { get; set; } // e.g., Monday, Tuesday, etc.
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
}


