using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.Twitter.twitterAPI
{
    public class PublishPostToTwitterCommand : IRequest<Response<string>>
    {
        public TwitterPostDto TwitterPostDto { get; set; }

    }

    public class PublishPostToTwitterCommandValidator : AbstractValidator<PublishPostToTwitterCommand>
    {
        public PublishPostToTwitterCommandValidator()
        {
            RuleFor(p => p.TwitterPostDto)
                .NotEmpty().WithMessage("{InstagramPostDto} is required.")
                .NotNull();
            // .MaximumLength(250).WithMessage("{PostText} must not exceed 250 characters.");
        }
    }

    public class PublishPostToTwitterCommandHandler : IRequestHandler<PublishPostToTwitterCommand, Response<string>>
    {
        private readonly ITwitterPostApiRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PublishPostToTwitterCommandHandler> _logger;

        public PublishPostToTwitterCommandHandler(ITwitterPostApiRepository repository, IMapper mapper, ILogger<PublishPostToTwitterCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<string>> Handle(PublishPostToTwitterCommand request, CancellationToken cancellationToken)
        {

            

            var newEntity = await _repository.PublishPostToTwitter(request.TwitterPostDto);

            _logger.LogInformation($"Twitter Post {newEntity} is successfully created.");

            return newEntity;
        }

    }
}
