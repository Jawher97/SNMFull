using MediatR;
using FluentValidation;
using SNS.Facebook.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Exceptions.Model;

namespace SNS.Facebook.Application.Features.Commands.Posts
{

    public class CreateFacebookPostCommand : IRequest<Response<FacebookPostDto>>
    {
        public FacebookPostDto FacebookPostDto { get; set; }

    }

    public class CreatePostCommandValidator : AbstractValidator<CreateFacebookPostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.FacebookPostDto.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }

    public class CreateFacebookPostCommandHandler : IRequestHandler<CreateFacebookPostCommand, Response<FacebookPostDto>>
    {
        private readonly IFacebookPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateFacebookPostCommandHandler> _logger;

        public CreateFacebookPostCommandHandler(IFacebookPostRepository<Guid> repository, IMapper mapper, ILogger<CreateFacebookPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<FacebookPostDto>> Handle(CreateFacebookPostCommand request, CancellationToken cancellationToken)
        {
            Response<FacebookPostDto> result = new Response<FacebookPostDto>();
            var facebookPostEntity = _mapper.Map<FacebookPost>(request.FacebookPostDto);
            var newEntity = await _repository.AddAsync(facebookPostEntity);

            if (newEntity != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<FacebookPostDto>(newEntity);
                _logger.LogInformation($"FacebookPost {newEntity.Id} is successfully created.");
            }
            return result;
        }
    }
}
