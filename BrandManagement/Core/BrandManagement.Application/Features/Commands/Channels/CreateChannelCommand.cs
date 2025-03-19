using MediatR;
using FluentValidation;
using SNM.BrandManagement.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Domain.Enumeration;

namespace SNM.BrandManagement.Application.Features.Commands.Channels
{

    public class CreateChannelCommand : IRequest<Channel>
    {
        public string? DisplayName { get; set; }
     
        public string? Photo { get; set; }
        public string? CoverPhoto { get; set; }
        public ActivationStatus? IsActivated { get; set; }
        public Guid BrandId { get; set; }
        public Guid ChannelTypeId { get; set; }
        public Guid? ChannelProfileId { get; set; }
        public string Link { get; set; }
        public string SocialChannelId { get; set; }
    }
    public class CreateChannelCommandValidator : AbstractValidator<CreateChannelCommand>
        {
            public CreateChannelCommandValidator()
            {
                RuleFor(p => p.DisplayName)
                    .NotEmpty().WithMessage("{DisplayName} is required.")
                    .NotNull()
                    .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
            }
        }
    public class CreateCommandHandler : IRequestHandler<CreateChannelCommand, Channel>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommandHandler> _logger;

        public CreateCommandHandler(IChannelRepository<Guid> repository, IMapper mapper, ILogger<CreateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Channel> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
        {
            //_mapper.Map(request, Channel);
            var brandEntity = _mapper.Map<Channel>(request);
            var newEntity = await _repository.AddAsync(brandEntity);

            _logger.LogInformation($"Channel {newEntity.Id} is successfully created.");

            return newEntity;
        }
    }
}
