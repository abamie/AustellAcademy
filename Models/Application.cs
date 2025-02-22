namespace AustellAcademyAdmissions.Models
{
public class Application
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string? Status { get; set; } // e.g., Pending, Approved, Rejected
    public  Student? Student { get; set; }
}
}