using System;
using System.ComponentModel.DataAnnotations;

namespace AustellAcademyAdmissions.Models
{
    public class Student
    {
      public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
      public string Status { get; set; }

    public int? ClassId { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public virtual Class Class { get; set; }
    }
}