using System.Net;
using System.Net.Mail;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infrastructure.Email;

public class SendMail : ISendMail
{
    private readonly EmailOptions _options;

    public SendMail(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }
    
    public async Task SendMailAsync(User user, Finance finance)
    {
        var from = new MailAddress(_options.From, "Market");
        var to = new MailAddress(user.Email, user.Username);

        using var mailMessage = new MailMessage(from, to);
        mailMessage.Subject = user.Email;
        mailMessage.Body = $"Hallo {user.Username}\nFinance:{finance.TotalRevenue}";

        using var smtp = new SmtpClient(_options.Host, _options.Port);
        smtp.EnableSsl = true;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_options.UserName, _options.Password);

        await smtp.SendMailAsync(mailMessage);
    }
}