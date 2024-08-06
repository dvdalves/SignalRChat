using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Net.Mail;

namespace SignalRChat.Hubs;

public class ChatHub(IConfiguration configuration) : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
        await NotifyManagement(message);
    }

    public async Task NotifyManagement(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
        await SendEmailAsync("david.santos@portaldecompraspublicas.com.br", "New Notification", message);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = configuration["Email:Address"];
        var password = configuration["Email:Password"];

        var smtpClient = new SmtpClient("smtp-mail.outlook.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(email, password),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(email),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}