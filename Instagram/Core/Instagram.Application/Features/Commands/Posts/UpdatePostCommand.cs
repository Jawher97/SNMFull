using MediatR;
using FluentValidation;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Exceptions.Model;

namespace SNM.Instagram.Application.Features.Commands.Posts
{
    public class UpdatePostCommand : IRequest<Response<PostDto>>
    {
      public PostDto PostDto { get; set; }

    }
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(p => p.PostDto.Id)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull();
             
        }
    }
    public class UpdateCommandHandler : IRequestHandler<UpdatePostCommand, Response<PostDto>>
    {
        private readonly IPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IPostRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<PostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            Response<PostDto> result = new Response<PostDto>();
            var instagramToUpdate = await _repository.GetByIdAsync(request.PostDto.Id);
            if (instagramToUpdate == null)
            {
                throw new NotFoundException(nameof(Post), request.PostDto.Id);
            }

            _mapper.Map(request, instagramToUpdate, typeof(UpdatePostCommand), typeof(Post));
            await _repository.UpdateAsync(instagramToUpdate);

            _logger.LogInformation($"Post {instagramToUpdate.Id} is successfully updated.");

            result.Succeeded = true;
            result.Data = request.PostDto;
            _logger.LogInformation($"InstagramChannel {request.PostDto} is successfully updated.");

            return result;
        }
    }
}
