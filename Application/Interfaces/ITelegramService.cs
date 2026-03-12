using Domain.Entities;

namespace Application.Interfaces;

public interface ITelegramService
{
    Task SendMessage(string message);
    Task SendOrderNotification(Order order);
    Task SendPhoto(string photoUrl);
}