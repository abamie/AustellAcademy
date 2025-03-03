using System;
using System.ComponentModel.DataAnnotations;

namespace AustellAcademyAdmissions.Models
{
  public class Student
  {

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public  string Name { get; set; }

    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [Required(ErrorMessage = "Email Address is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public  string Email { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid Phone Number. Use 10-15 digits.")]
    public  string Phone { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public  string Address { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    public  string Gender { get; set; }

    [StringLength(20)]
    public string Status { get; set; }

    public int? ClassId { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public virtual Class Class { get; set; }
  }
}