using SNS.Facebook.Shared.Events;

namespace SNS.Facebook.Application.Common.Events
{
    public interface IEventPublisher : ITransientService
    {
        Task PublishAsync(IEvent @event);
    }
}