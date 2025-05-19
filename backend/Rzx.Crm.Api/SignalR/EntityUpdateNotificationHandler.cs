using MediatR;
using Rzx.Crm.Core.Events;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Api.SignalR
{
    public class EntityUpdateNotificationHandler : INotificationHandler<EntityModificationNotification<Customer>>,
        INotificationHandler<EntityModificationNotification<Order>>
    {
        private readonly MessageHub _messageHub;

        public EntityUpdateNotificationHandler(MessageHub messageHub)
        {
            _messageHub = messageHub;
        }

        public async Task Handle(EntityModificationNotification<Customer> notification, CancellationToken cancellationToken)
        {
            await _messageHub.SendEntityMessageAsync(
                new NotificationMessage(
                    "customer",
                    new Payload(notification.ModificationType.ToString(), notification.Entity)
                )
            );
        }

        public async Task Handle(EntityModificationNotification<Order> notification, CancellationToken cancellationToken)
        {
            await _messageHub.SendEntityMessageAsync(
                new NotificationMessage(
                    "order",
                    new Payload(notification.ModificationType.ToString(), notification.Entity)
                )
            );
        }
    }
}
