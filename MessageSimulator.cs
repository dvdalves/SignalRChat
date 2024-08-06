using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;

namespace SignalRChat;

public class MessageSimulator(IHubContext<ChatHub> hubContext) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = new Random();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(random.Next(5000, 10000), stoppingToken);

            var message = $"New message received at {DateTime.Now}";
            await hubContext.Clients.All.SendAsync("ReceiveNotification", message, cancellationToken: stoppingToken);
        }
    }
}