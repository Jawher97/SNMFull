using MediatR;
using FluentValidation;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Application.Exceptions;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Exceptions.Model;

namespace SNS.Facebook.Application.Features.Commands.Posts
{
    public class UpdateFacebookPostCommand : IRequest<Response<FacebookPostDto>>
    {
        public FacebookPostDto FacebookPostDto { get; set; }
    }
   
    public class UpdateFacebookPostCommandValidator : AbstractValidator<UpdateFacebookPostCommand>
    {
        public UpdateFacebookPostCommandValidator()
        {
            RuleFor(p => p.FacebookPostDto.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .NotNull();
                //.MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }
    
    public class UpdateFacebookPostCommandHandler : IRequestHandler<UpdateFacebookPostCommand, Response<FacebookPostDto>>
    {
        private readonly IFacebookPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateFacebookPostCommandHandler> _logger;

        public UpdateFacebookPostCommandHandler(IFacebookPostRepository<Guid> repository, IMapper mapper, ILogger<UpdateFacebookPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<FacebookPostDto>> Handle(UpdateFacebookPostCommand request, CancellationToken cancellationToken)
        {
            Response<FacebookPostDto> result = new Response<FacebookPostDto>();

            var facebookPostToUpdate = await _repository.GetByIdAsync(request.FacebookPostDto.Id);

            if (facebookPostToUpdate == null)
            {
                throw new NotFoundException(nameof(FacebookPost), request.FacebookPostDto.Id);
            }
            _mapper.Map(facebookPostToUpdate, request,  typeof(FacebookPost) ,typeof(UpdateFacebookPostCommand));
             await _repository.UpdateAsync(facebookPostToUpdate);

            
             result.Succeeded = true;
             result.Data = request.FacebookPostDto;
            _logger.LogInformation($"FacebookPost {request.FacebookPostDto} is successfully updated.");

            return result;
        }
    }
}