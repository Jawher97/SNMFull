using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts
{
    public class CreatePostLinkedinCommand : IRequest<Response<LinkedInPostDto>>
    {
        public LinkedInPostDto linkedInPostDto { get; set; }

    }

    public class CreatePostCommandValidator : AbstractValidator<CreatePostLinkedinCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.linkedInPostDto.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }

    public class CreateLinkedInPostCommandHandler : IRequestHandler<CreatePostLinkedinCommand, Response<LinkedInPostDto>>
    {
        private readonly ILinkedInPostRepository<Guid> _repository;
       // private readonly ITwitterImagesRepository<Guid> _repositoryImage;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLinkedInPostCommandHandler> _logger;

        public CreateLinkedInPostCommandHandler(ILinkedInPostRepository<Guid> repository, IMapper mapper, ILogger<CreateLinkedInPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //_repositoryImage = repositoryImage ?? throw new ArgumentNullException(nameof(repositoryImage));
        }

        public async Task<Response<LinkedInPostDto>> Handle(CreatePostLinkedinCommand request, CancellationToken cancellationToken)
        {
            Response<LinkedInPostDto> result = new Response<LinkedInPostDto>();
            // List<LinkedInImages> LinkedInImageList = new List<LinkedInImages>();

            var LinkedInPostEntity = _mapper.Map<LinkedInPost>(request.linkedInPostDto);

            var newEntity = await _repository.AddAsync(LinkedInPostEntity);
          

            if (newEntity != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<LinkedInPostDto>(newEntity);
                _logger.LogInformation($"LinkedinPost {newEntity.Id} is successfully created.");
            }
            return result;
        }
    }
}
  
