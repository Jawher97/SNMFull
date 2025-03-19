using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Commands.LinkedeIn.LinkedinAPI
{
    public class PublishPostToLinkedinCommand : IRequest<Response<string>>
    {
        public LinkedInPostDto LinkedInPostDto { get; set; }
        public LinkedInChannelDto LinkedInChannelDto { get; set; }

    }

    public class PublishPostToLinkedInCommandValidator : AbstractValidator<PublishPostToLinkedinCommand>
    {
        public PublishPostToLinkedInCommandValidator()
        {
            RuleFor(p => p.LinkedInPostDto)
                .NotEmpty().WithMessage("{LinkedinPostDto} is required.")
                .NotNull();
            // .MaximumLength(250).WithMessage("{PostText} must not exceed 250 characters.");
        }
    }

    public class PublishPostToLinkedinCommandHandler : IRequestHandler<PublishPostToLinkedinCommand, Response<string>>
    {
        private readonly ILinkedInAPIRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PublishPostToLinkedinCommandHandler> _logger;

        public PublishPostToLinkedinCommandHandler(ILinkedInAPIRepository<Guid> repository, IMapper mapper, ILogger<PublishPostToLinkedinCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<string>> Handle(PublishPostToLinkedinCommand request, CancellationToken cancellationToken)
        {



            var newEntity = await _repository.PublishToLinkedIn(request.LinkedInPostDto,request.LinkedInChannelDto);

            _logger.LogInformation($"Linkedin Post {newEntity} is successfully created.");

            return newEntity;
        }
    }
}
