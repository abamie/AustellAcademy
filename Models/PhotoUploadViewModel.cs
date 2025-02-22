using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AustellAcademyAdmissions.Models
{
    public class PhotoUploadViewModel
    {

        public int Id { get; set; }
        public string CurrentImagePath { get; set; }
        public List<IFormFile> Images { get; set; }
        public string Caption { get; set; }
        public string Category { get; set; }
    }
}