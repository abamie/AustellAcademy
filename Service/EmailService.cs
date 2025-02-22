using MailKit.Net.Smtp;
using MimeKit;

namespace AustellAcademyAdmissions.Service
{
public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Austell Academy", emailSettings["Username"]));
        message.To.Add(new MailboxAddress("Applicant", to));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var client = new SmtpClient())
        {
             await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), true);
            await client.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
}