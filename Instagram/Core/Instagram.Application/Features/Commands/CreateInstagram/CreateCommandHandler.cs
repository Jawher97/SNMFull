using AutoMapper;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SNM.Instagram.Application.Features.Commands.CreateInstagram
{
    public class CreateCommandHandler : IRequestHandler<CreateInstagramCommand, Guid>
    {
        private readonly IInstagramRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommandHandler> _logger;

        public CreateCommandHandler(IInstagramRepository<Guid> repository, IMapper mapper, ILogger<CreateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateInstagramCommand request, CancellationToken cancellationToken)
        {
            var instagramEntity = _mapper.Map<InstagramPost>(request);
            var newEntity = await _repository.AddAsync(instagramEntity);

            _logger.LogInformation($"Entity {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}