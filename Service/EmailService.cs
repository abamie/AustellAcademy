using MailKit.Net.Smtp;
using MimeKit;

namespace AustellAcademyAdmissions.Service
{


    using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _senderEmail;
    private readonly string _password;
    private readonly bool _enableSsl;

    public EmailService(IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        _smtpServer = emailSettings["SmtpServer"];
        _port = int.Parse(emailSettings["Port"]);
        _senderEmail = emailSettings["SenderEmail"];
        _password = emailSettings["Password"];
        _enableSsl = bool.Parse(emailSettings["EnableSsl"]);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Austell Academy", _senderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _port, _enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_senderEmail, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email sending failed: {ex.Message}");
            throw;
        }
    }
}

public class EmailService22
{
    private readonly IConfiguration _configuration;

    public EmailService22(IConfiguration configuration)
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