using MediatR;
using Rzx.Crm.Core.Events;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Api.SignalR
{
    public class EntityUpdateNotificationHandler : INotificationHandler<EntityModificationNotification<Customer>>,
        INotificationHandler<EntityModificationNotification<Order>>
    {
        private readonly MessageHub messageHub;

        public EntityUpdateNotificationHandler(MessageHub messageHub)
        {
            this.messageHub = messageHub;
        }

        public async Task Handle(EntityModificationNotification<Customer> notification, CancellationToken cancellationToken)
        {
            await Publish("customer", notification.Entity, notification.ModificationType.ToString());
        }

        public async Task Handle(EntityModificationNotification<Order> notification, CancellationToken cancellationToken)
        {
            await Publish("order", notification.Entity, notification.ModificationType.ToString());
        }

        private async Task Publish(string topic, object entity, string eventType)
        {
            await messageHub.SendEntityMessageAsync(new NotificationMessage
            {
                Envelope = new Envelope
                {
                    topic = topic
                },
                Payload = new Payload
                {
                    Entity = entity,
                    EventType = eventType
                }
            });
        }
    }
}
