using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Interfaces;


namespace SNS.Facebook.Application.Features.Commands.FacebookAPI
{
   
    public class PublishToFacebookCommand : IRequest<string>
    {
        public FacebookPostDto FacebookPostDto { get; set; }

    }

    public class CreatePostCommandValidator : AbstractValidator<PublishToFacebookCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.FacebookPostDto)
                .NotEmpty().WithMessage("{FacebookPostDto} is required.")
                .NotNull();
               // .MaximumLength(250).WithMessage("{PostText} must not exceed 250 characters.");
        }
    }
    
    public class PublishToFacebookCommandHandler : IRequestHandler<PublishToFacebookCommand, string>
    {
        private readonly IFacebookAPIRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PublishToFacebookCommandHandler> _logger;

        public PublishToFacebookCommandHandler(IFacebookAPIRepository repository, IMapper mapper, ILogger<PublishToFacebookCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(PublishToFacebookCommand request, CancellationToken cancellationToken)
        {
            request.FacebookPostDto.FacebookChannel.PostToPageURL = $"{request.FacebookPostDto.FacebookChannel.ChannelAPI}{request.FacebookPostDto.FacebookChannel.SocialChannelNetwokId}/feed";
            request.FacebookPostDto.FacebookChannel.PostToPagePhotosURL = $"{request.FacebookPostDto.FacebookChannel.ChannelAPI}{request.FacebookPostDto.FacebookChannel.SocialChannelNetwokId}/{request.FacebookPostDto.FacebookChannel.PageEdgePhotos}";

            var newEntity = await _repository.PublishPostToFacebook(request.FacebookPostDto);

            _logger.LogInformation($"Facebook Post {newEntity} is successfully created.");

            return newEntity.ToString();
        }

       
    }
}
