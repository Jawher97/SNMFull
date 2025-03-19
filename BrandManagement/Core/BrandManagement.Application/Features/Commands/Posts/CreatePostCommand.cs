using MediatR;
using FluentValidation;
using SNM.BrandManagement.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Exceptions.Model;

namespace SNM.BrandManagement.Application.Features.Commands.Posts
{

    public class CreatePostCommand : IRequest<Response<PostDto>>
    {
        public PostDto PostDto { get; set; }        
       
    }
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.PostDto.Caption)
                .NotEmpty().WithMessage("{Caption} is required.")
                .NotNull();
            // .MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }
    public class CreateCommandHandler : IRequestHandler<CreatePostCommand, Response<PostDto>>
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

        public async Task<Response<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            Response<PostDto> result = new Response<PostDto>();
            var postEntity = _mapper.Map<Post>(request.PostDto);
            var newEntity = await _repository.AddAsync(postEntity);

            if (newEntity != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<PostDto>(newEntity);

                _logger.LogInformation($"Post {newEntity.Id} is successfully created.");
            }

            return result;
        }
    }
}
