using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SNS.Facebook.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Application.DTO;

namespace SNS.Facebook.Application.Features.Commands.FacebookChannels
{

    public class CreateFacebookChannelCommand : IRequest<Guid>
    {
        public FacebookChannelDto FacebookChannelDto { get; set; }
        
    }
        public class CreateFacebookChannelCommandValidator : AbstractValidator<CreateFacebookChannelCommand>
        {
            public CreateFacebookChannelCommandValidator()
            {
                RuleFor(p => p.FacebookChannelDto)
                    .NotEmpty().WithMessage("{FacebookChannel} is required.")
                    .NotNull();
            }
        }
    public class CreateFacebookChannelCommandHandler : IRequestHandler<CreateFacebookChannelCommand, Guid>
    {
        private readonly IFacebookChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateFacebookChannelCommandHandler> _logger;

        public CreateFacebookChannelCommandHandler(IFacebookChannelRepository<Guid> repository, IMapper mapper, ILogger<CreateFacebookChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateFacebookChannelCommand request, CancellationToken cancellationToken)
        {
            var brandEntity = _mapper.Map<FacebookChannel>(request.FacebookChannelDto);
            var newEntity = await _repository.AddAsync(brandEntity);

            _logger.LogInformation($"FacebookChannel {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}
