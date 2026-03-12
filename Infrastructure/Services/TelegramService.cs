using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services;

public class TelegramService : ITelegramService
{
    private readonly HttpClient _httpClient;

    private const string Token = "7705064539:AAGV1jdLu08BKFApdlY1Wo0fFUS6uY75Hh0";
    private const string ChatId = "5426464671";

    public TelegramService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendMessage(string message)
    {
        const string url = $"https://api.telegram.org/bot{Token}/sendMessage";

        var data = new Dictionary<string, string>
        {
            { "chat_id", ChatId },
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