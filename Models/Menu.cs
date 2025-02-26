
using System.ComponentModel.DataAnnotations;

namespace AustellAcademyAdmissions.Models
{
  public class Menu
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }  // Menu Title
    public string Url { get; set; }  // Link
    public int Order { get; set; }  // Position in Menu
    public bool IsActive { get; set; }  // Show/Hide Menu Item
    public string Roles { get; set; }  // Comma-separated roles (e.g., "Admin,Teacher,Student")
     public bool IsVisible { get; set; } // New: Show/Hide Menu
     public int? ParentId { get; set; } // If null, it's a main menu item

    public Menu Parent { get; set; }

    public ICollection<Menu> SubMenus { get; set; }
}

}