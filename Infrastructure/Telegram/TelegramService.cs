using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infrastructure.Telegram;

public class TelegramService : ITelegramService
{
    private readonly HttpClient _httpClient;
    private readonly TelegramSettings _telegram;
    
    public TelegramService(HttpClient httpClient, IOptions<TelegramSettings> telegram)
    {
        _httpClient = httpClient;
        _telegram = telegram.Value;
    }

    public async Task SendMessage(string message)
    {
        var token = _telegram.TelegramToken;
        var url = $"https://api.telegram.org/bot{token}/sendMessage";

        var data = new Dictionary<string, string>
        {
            { "chat_id", _telegram.TelegramChatId },
            { "text", message }
        };

        var content = new FormUrlEncodedContent(data);

        await _httpClient.PostAsync(url, content);
    }

    public Task SendOrderNotification(Order order)
    {
        throw new NotImplementedException();
    }

    public Task SendPhoto(string photoUrl)
    {
        throw new NotImplementedException();
    }
}