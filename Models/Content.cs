namespace AustellAcademyAdmissions.Models
{
   public class Content
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string? ImageUrl { get; set; } // Store image path
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

}
