using MediatR;
using Rzx.Crm.Core.Events;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Core.EventHandlers
{
    public class SeedData : INotificationHandler<ApplicationStartedNotification>
    {
        private readonly SeedingService _seedingService;

        public SeedData(SeedingService seedingService)
        {
            _seedingService = seedingService;
        }

        public async Task Handle(ApplicationStartedNotification notification, CancellationToken cancellation)
        {
            await _seedingService.SeedDataAsync();
        }
    }
}
