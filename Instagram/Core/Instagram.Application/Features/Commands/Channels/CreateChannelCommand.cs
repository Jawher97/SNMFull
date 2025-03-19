using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SNM.Instagram.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Features.Commands.Channels
{

    public class CreateChannelCommand : IRequest<Guid>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
        public Guid BrandId { get; set; }
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
    public class CreateCommandHandler : IRequestHandler<CreateChannelCommand, Guid>
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

        public async Task<Guid> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
        {
            var brandEntity = _mapper.Map<Channel>(request);
            var newEntity = await _repository.AddAsync(brandEntity);

            _logger.LogInformation($"Channel {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}
