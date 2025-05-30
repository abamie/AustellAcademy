using System;
using System.ComponentModel.DataAnnotations;

namespace AustellAcademyAdmissions.Models
{
    public class Admission
    {
         [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public required string Name { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid Phone Number. Use 10-15 digits.")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public required string Gender { get; set; } // New property

        public string? DocumentPath { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Default status is "Pending"

        public int? ClassId { get; set; }
        public Class Class { get; set; }
    }
}

