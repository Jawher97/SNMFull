using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Interfaces;

namespace SNM.Twitter.Application.Features.Commands.TwitterAPI
{
    public class PublishToTwitterCommand : IRequest<string>
    {
        public TwitterChannelPostDto TwitterChannelPostDto { get; set; }
    }

    public class CreatePostCommandValidator : AbstractValidator<PublishToTwitterCommand>
    {
        public CreatePostCommandValidator() 
        {
            RuleFor(p => p.TwitterChannelPostDto)
            .NotEmpty().WithMessage("{TwitterChannelPostDto} is required.")
            .NotNull();
        }
    }

    public class PublishToTwitterCommandHandler : IRequestHandler<PublishToTwitterCommand, string>
    {
        private readonly ITwitterAPIRepository<Guid> _repository;
        private readonly ILogger<PublishToTwitterCommandHandler> _logger;

        public PublishToTwitterCommandHandler(ITwitterAPIRepository<Guid> repository, ILogger<PublishToTwitterCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(PublishToTwitterCommand request, CancellationToken cancellationToken)
        {
           /* var newEntity = await _repository.PublishToTwitter(request.TwitterChannelPostDto);

            _logger.LogInformation($"Post {newEntity} is successfully created.");

            return newEntity.ToString();*/
        
            throw new NotImplementedException();
        }
    }
}
