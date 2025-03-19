using Multitenancy.Shared.Events;

namespace Multitenancy.Application.Common.Events
{
    public interface IEventPublisher : ITransientService
    {
        Task PublishAsync(IEvent @event);
    }
}