using Microsoft.AspNetCore.SignalR;

namespace Rzx.Crm.Api.SignalR
{
    public class MessageHub : Hub
    {
        private ILogger<MessageHub> logger;

        public MessageHub(ILogger<MessageHub> logger)
        {
            this.logger = logger;
        }

        public async Task SendEntityMessageAsync(NotificationMessage message)
        {
            await SendMessageAsync(message.Envelope.topic, message);
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
                logger.LogError(e, $"unable to pub SignalR message [{message}]");
            }
        }
    }
}
