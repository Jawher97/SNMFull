using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.TwitterChannels
{

    public class CreateTwitterChannelCommand : IRequest<Guid>
    {
        public TwitterChannelDto TwitterChannelDto { get; set; }

    }
    public class CreateTwitterChannelCommandValidator : AbstractValidator<CreateTwitterChannelCommand>
    {
        public CreateTwitterChannelCommandValidator()
        {
            RuleFor(p => p.TwitterChannelDto)
                .NotEmpty().WithMessage("{InstagramChannel} is required.")
                .NotNull();
        }
    }
    public class CreateTwiiterChannelCommandHandler : IRequestHandler<CreateTwitterChannelCommand, Guid>
    {
        private readonly ITwitterChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTwiiterChannelCommandHandler> _logger;

        public CreateTwiiterChannelCommandHandler(ITwitterChannelRepository<Guid> repository, IMapper mapper, ILogger<CreateTwiiterChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateTwitterChannelCommand request, CancellationToken cancellationToken)
        {
            var TwitterChannel = _mapper.Map<TwitterChannel>(request.TwitterChannelDto);
            var newEntity = await _repository.AddAsync(TwitterChannel);

            _logger.LogInformation($"TwitterChannel {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}
