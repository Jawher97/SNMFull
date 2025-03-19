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
using SNM.Instagram.Application.DTO;

namespace SNM.Instagram.Application.Features.Commands.InstagramChannels
{

    public class CreateInstagramChannelCommand : IRequest<Guid>
    {
        public InstagramChannelDto InstagramChannelDto { get; set; }

    }
    public class CreateInstagramChannelCommandValidator : AbstractValidator<CreateInstagramChannelCommand>
    {
        public CreateInstagramChannelCommandValidator()
        {
            RuleFor(p => p.InstagramChannelDto)
                .NotEmpty().WithMessage("{InstagramChannel} is required.")
                .NotNull();
        }
    }
    public class CreateInstagramChannelCommandHandler : IRequestHandler<CreateInstagramChannelCommand, Guid>
    {
        private readonly IInstagramChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateInstagramChannelCommandHandler> _logger;

        public CreateInstagramChannelCommandHandler(IInstagramChannelRepository<Guid> repository, IMapper mapper, ILogger<CreateInstagramChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateInstagramChannelCommand request, CancellationToken cancellationToken)
        {
            var InstagramChannel = _mapper.Map<InstagramChannel>(request.InstagramChannelDto);
            var newEntity = await _repository.AddAsync(InstagramChannel);

            _logger.LogInformation($"InstagramChannel {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}
