using Domain.Entities;

namespace Infrastructure.Email;

public interface ISendMail
{
    Task SendMailAsync(User user, Finance finance);
}