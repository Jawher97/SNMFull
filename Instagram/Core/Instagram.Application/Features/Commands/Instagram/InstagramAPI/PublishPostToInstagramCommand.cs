using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions.Model;
using SNM.Instagram.Application.Interfaces;

namespace SNM.Instagram.Application.Features.Commands.Instagram.InstagramAPI
{
    public class PublishPostToInstagramCommand:IRequest<Response<string>>
    {
        public InstagramPostDto InstagramPostDto { get; set; }

    }

   public class PublishPostToInstagramCommandValidator :  AbstractValidator<PublishPostToInstagramCommand>
    {
        public PublishPostToInstagramCommandValidator()
        {
            RuleFor(p => p.InstagramPostDto)
                .NotEmpty().WithMessage("{InstagramPostDto} is required.")
                .NotNull();
            // .MaximumLength(250).WithMessage("{PostText} must not exceed 250 characters.");
        }
    }

    public class PublishPostToInstagramCommandHandler : IRequestHandler<PublishPostToInstagramCommand, Response<string>>
    {
        private readonly IInstagramPostApiRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PublishPostToInstagramCommandHandler> _logger;

        public PublishPostToInstagramCommandHandler(IInstagramPostApiRepository repository, IMapper mapper, ILogger<PublishPostToInstagramCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<string>> Handle(PublishPostToInstagramCommand request, CancellationToken cancellationToken)
        {
           

            var newEntity = await _repository.PublishPostToInstagram(request.InstagramPostDto);

            _logger.LogInformation($"Instagram Post {newEntity} is successfully created.");

            return newEntity;
        }


    }
}
