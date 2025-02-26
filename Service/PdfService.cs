using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using System.Threading.Tasks;
using AustellAcademyAdmissions.Models;
namespace AustellAcademyAdmissions.Service
{

    public class PdfService
    {
        public async Task<byte[]> GenerateStudentPdf(Student student)
        {
            
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Seek(0, SeekOrigin.Begin); // Ensure it's seekable
                using (var writer = new PdfWriter(memoryStream))
                {
                    
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                        var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                        // Title
                        document.Add(new Paragraph("Student Details")
                            .SetFont(boldFont)
                            .SetFontSize(18)
                            .SetTextAlignment(TextAlignment.CENTER));

                        // Student information
                        document.Add(new Paragraph($"Name: {student.Name}").SetFont(normalFont));
                        document.Add(new Paragraph($"Email: {student.Email}").SetFont(normalFont));
                        document.Add(new Paragraph($"Phone: {student.Phone}").SetFont(normalFont));
                        document.Add(new Paragraph($"Address: {student.Address}").SetFont(normalFont));
                        document.Add(new Paragraph($"Gender: {student.Gender}").SetFont(normalFont));
                        document.Add(new Paragraph($"Class: {student.Class?.ClassName ?? "N/A"}").SetFont(normalFont));
                        document.Add(new Paragraph($"Status: {student.Status}").SetFont(normalFont));
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
