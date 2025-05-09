using MediatR;
using Microsoft.Extensions.Logging;
using Multitenancy.Application.Common.Events;
using Multitenancy.Shared.Events;

namespace Multitenancy.Infrastructure.Common.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;
        private readonly IPublisher _mediator;

        public EventPublisher(ILogger<EventPublisher> logger, IPublisher mediator) =>
            (_logger, _mediator) = (logger, mediator);

        public Task PublishAsync(IEvent @event)
        {
            _logger.LogInformation("Publishing Event : {event}", @event.GetType().Name);
            return _mediator.Publish(CreateEventNotification(@event));
        }

        private INotification CreateEventNotification(IEvent @event) =>
            (INotification)Activator.CreateInstance(
                typeof(EventNotification<>).MakeGenericType(@event.GetType()), @event)!;
    }
}