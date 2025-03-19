using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Commands.ChannelProfiles
{
    public class CreateChannelProfileCommand : IRequest<ChannelProfile>
    {
        public ChannelProfileDto ChannelProfileDto { get; set; }

    }
    public class CreateChannelProfileCommandValidator : AbstractValidator<CreateChannelProfileCommand>
    {
        public CreateChannelProfileCommandValidator()
        {
            RuleFor(p => p.ChannelProfileDto)
                .NotEmpty().WithMessage("{ProfileChannel} is required.")
                .NotNull();
        }
    }
    public class CreateProfileChannelCommandHandler : IRequestHandler<CreateChannelProfileCommand, ChannelProfile>
    {
         private readonly IChannelProfileRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProfileChannelCommandHandler> _logger;

        public CreateProfileChannelCommandHandler(IChannelProfileRepository<Guid> repository, IMapper mapper, ILogger<CreateProfileChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ChannelProfile> Handle(CreateChannelProfileCommand request, CancellationToken cancellationToken)
        {
            var ProfileEntity = _mapper.Map<ChannelProfile>(request.ChannelProfileDto);
            var newEntity = await _repository.AddAsync(ProfileEntity);

            _logger.LogInformation($"ChannelProfile {newEntity.Id} is successfully created.");

            return newEntity;
        }
    }
}

