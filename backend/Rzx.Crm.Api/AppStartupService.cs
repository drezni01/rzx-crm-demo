using MediatR;
using Rzx.Crm.Core.EventHandlers;
using Rzx.Crm.Core.Events;

namespace Rzx.Crm.Api
{
    public class AppStartupService : IHostedService
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AppStartupService(IMediator mediator, ILogger<AppStartupService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending appstart message");
            return _mediator.Publish(new ApplicationStartedNotification());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
