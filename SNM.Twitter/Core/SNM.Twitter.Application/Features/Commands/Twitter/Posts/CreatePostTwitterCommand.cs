using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.Twitter.Posts
{
    public class CreatePostTwitterCommand : IRequest<Response<TwitterPostDto>>
    {
        public TwitterPostDto twitterPostDto { get; set; }

    }

    public class CreatePostCommandValidator : AbstractValidator<CreatePostTwitterCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.twitterPostDto.Text)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }

        public class CreateTwitterPostCommandHandler : IRequestHandler<CreatePostTwitterCommand, Response<TwitterPostDto>>
    {
        private readonly ITwitterPostRepository<Guid> _repository;

        private readonly IMapper _mapper;
        private readonly ILogger<CreateTwitterPostCommandHandler> _logger;

        public CreateTwitterPostCommandHandler( ITwitterPostRepository<Guid> repository, IMapper mapper, ILogger<CreateTwitterPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
 
        }

        public async Task<Response<TwitterPostDto>> Handle(CreatePostTwitterCommand request, CancellationToken cancellationToken)
        {
            Response<TwitterPostDto> result = new Response<TwitterPostDto>();
          

            var twitterPostEntity = _mapper.Map<TwitterPost>(request.twitterPostDto);

            var newEntity = await _repository.AddAsync(twitterPostEntity);
           


            if (newEntity != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<TwitterPostDto>(newEntity);
                _logger.LogInformation($"TwitterPost {newEntity.Id} is successfully created.");
            }
            return result;
        }
    }
}
