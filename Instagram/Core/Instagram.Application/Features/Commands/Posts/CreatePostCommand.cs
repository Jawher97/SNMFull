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

namespace SNM.Instagram.Application.Features.Commands.Posts
{

    public class CreatePostCommand : IRequest<Guid>
    {
        public string Message { get; set; }


    }
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }
    public class CreateCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommandHandler> _logger;

        public CreateCommandHandler(IPostRepository<Guid> repository, IMapper mapper, ILogger<CreateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var brandEntity = _mapper.Map<Post>(request);
            var newEntity = await _repository.AddAsync(brandEntity);

            _logger.LogInformation($"Post {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}

