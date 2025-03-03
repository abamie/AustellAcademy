using System;
using System.ComponentModel.DataAnnotations;

namespace AustellAcademyAdmissions.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public string OrderId { get; set; }

        public string PaymentId { get; set; }

        public string Signature { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }

        public int AdmissionId { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}