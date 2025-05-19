using MediatR;
using Rzx.Crm.Core.Events;

namespace Rzx.Crm.Api.SignalR
{
    public class CustomerStatsNotificationHandler : INotificationHandler<CustomerStatsNotification>
    {
        private readonly MessageHub _messageHub;

        public CustomerStatsNotificationHandler(MessageHub messageHub)
        {
            _messageHub = messageHub;
        }

        public async Task Handle(CustomerStatsNotification notification, CancellationToken cancellationToken)
        {
            await _messageHub.SendEntityMessageAsync(
                new NotificationMessage(
                    "customer_stats",
                    new Payload(EntityModificationTypeEnum.UPDATE.ToString(), notification.Stats)
                )
            );
        }
    }
}
