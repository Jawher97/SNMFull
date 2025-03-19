using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.Exceptions;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Features.Commands.Deletetwitter
{
    public class DeletetwitterCommandHandler : IRequestHandler<DeletetwitterCommand>
    {
        private readonly ItwitterRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeletetwitterCommandHandler> _logger;

        public DeletetwitterCommandHandler(ItwitterRepository<Guid> repository, IMapper mapper, ILogger<DeletetwitterCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeletetwitterCommand request, CancellationToken cancellationToken)
        {
            var twitterToDelete = await _repository.GetByIdAsync(request.Id);
            if (twitterToDelete == null)
            {
                throw new NotFoundException(nameof(Entity), request.Id);
            }

            await _repository.DeleteAsync(twitterToDelete);

            _logger.LogInformation($"Entity {twitterToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}