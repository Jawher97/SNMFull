using SNS.Facebook.Domain.Common.Events;

namespace SNS.Facebook.Application.Catalog.Products.EventHandlers
{
    public class ProductDeletedEventHandler : EventNotificationHandler<EntityDeletedEvent<Product>>
    {
        private readonly ILogger<ProductDeletedEventHandler> _logger;

        public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger) => _logger = logger;

        public override Task Handle(EntityDeletedEvent<Product> @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{event} Triggered", @event.GetType().Name);
            return Task.CompletedTask;
        }
    }
}