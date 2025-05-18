using MediatR;
using Rzx.Crm.Core.Events;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Core.EventHandlers
{
    public class SeedData : INotificationHandler<ApplicationStartedNotification>
    {
        private readonly SeedingService seedingService;

        public SeedData(SeedingService seedingService)
        {
            this.seedingService = seedingService;
        }

        public async Task Handle(ApplicationStartedNotification notification, CancellationToken cancellation)
        {
            await seedingService.SeedDataAsync();
        }
    }
}
