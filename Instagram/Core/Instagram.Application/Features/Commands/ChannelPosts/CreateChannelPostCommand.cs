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

namespace SNM.Instagram.Application.Features.Commands.ChannelPosts
{

    public class CreateChannelPostCommand : IRequest<Guid>
    {
        public ChannelPostDto ChannelPost { get; set; }

    }
    public class CreateChannelPostCommandValidator : AbstractValidator<CreateChannelPostCommand>
    {
        public CreateChannelPostCommandValidator()
        {
            //RuleFor(p => p.ChannelPost.ChannelId)
            //    .NotEmpty().WithMessage("{DisplayName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
    public class CreateCommandHandler : IRequestHandler<CreateChannelPostCommand, Guid>
    {
        private readonly IChannelPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommandHandler> _logger;

        public CreateCommandHandler(IChannelPostRepository<Guid> repository, IMapper mapper, ILogger<CreateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateChannelPostCommand request, CancellationToken cancellationToken)
        {
            var brandEntity = _mapper.Map<ChannelPost>(request.ChannelPost);
            var newEntity = await _repository.AddAsync(brandEntity);

            _logger.LogInformation($"ChannelPost {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}

