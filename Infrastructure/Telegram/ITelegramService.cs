using Domain.Entities;

namespace Infrastructure.Telegram;

public interface ITelegramService
{
    Task SendMessage(string message);
    Task SendOrderNotification(Order order);
    Task SendPhoto(string photoUrl);
}