using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Features.Commands.Createtwitter
{
    public class CreateCommandHandler : IRequestHandler<CreatetwitterCommand, Guid>
    {
        private readonly ItwitterRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommandHandler> _logger;

        public CreateCommandHandler(ItwitterRepository<Guid> repository, IMapper mapper, ILogger<CreateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreatetwitterCommand request, CancellationToken cancellationToken)
        {
            var twitterEntity = _mapper.Map<Entity>(request);
            var newEntity = await _repository.AddAsync(twitterEntity);

            _logger.LogInformation($"Entity {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}