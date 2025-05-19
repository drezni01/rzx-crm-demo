using Microsoft.AspNetCore.SignalR;

namespace Rzx.Crm.Api.SignalR
{
    public class MessageHub : Hub
    {
        private readonly ILogger<MessageHub> _logger;

        public MessageHub(ILogger<MessageHub> logger)
        {
            _logger = logger;
        }

        public async Task SendEntityMessageAsync(NotificationMessage message)
        {
            await SendMessageAsync(message.Envelope.Topic, message);
        }

        private async Task SendMessageAsync(string topic, NotificationMessage message)
        {
            try
            {
                if (Clients != null)
                    await Clients.All.SendAsync(topic, message);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"unable to pub SignalR message [{message}]");
            }
        }
    }
}
