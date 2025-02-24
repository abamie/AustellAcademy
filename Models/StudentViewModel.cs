namespace AustellAcademyAdmissions.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int? ClassId { get; set; }
        public string? DocumentPath { get; set; }
        public string Status { get; set; }

       public string? ClassName {get;set;}
    }


}