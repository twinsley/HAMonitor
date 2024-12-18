using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace hamonitor;

public class RebootNotification
{
    public void SendEmailNotification(int rebootCounter, string emailRecipient)
    {
        string body = $"Home Assistant has been power cycled {rebootCounter} times. After 5 times it will be considered dead.";
        string subject = $"Home Assistant was rebooted.";
        
        SendEmail(emailRecipient, subject, body);
    }

    private async void SendEmail(string email, string subject, string body)
    {
        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            "Monitoring System", 
            "monitoring@winsley.app"
        ));
        message.To.Add(new MailboxAddress(
            "Tim Winsley", 
            email
        ));
        message.Subject = subject;
        var bodyBuilder = new BodyBuilder
        {
            TextBody = body,
            HtmlBody = body
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
// SecureSocketOptions.StartTls force a secure connection over TLS
        await client.ConnectAsync("smtp.sendgrid.net", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(
            userName: "apikey", // the userName is the exact string "apikey" and not the API key itself.
            password: Environment.GetEnvironmentVariable("EmailAPIKey") // password is the API key
        );

        Console.WriteLine("Sending email");
        await client.SendAsync(message);
        Console.WriteLine("Email sent");

        await client.DisconnectAsync(true);
    }
}