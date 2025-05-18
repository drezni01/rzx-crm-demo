using MediatR;
using Rzx.Crm.Core.EventHandlers;
using Rzx.Crm.Core.Events;

namespace Rzx.Crm.Api
{
    public class AppStartupService : IHostedService
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public AppStartupService(IMediator mediator, ILogger<AppStartupService> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending appstart message");
            return mediator.Publish(new ApplicationStartedNotification());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
