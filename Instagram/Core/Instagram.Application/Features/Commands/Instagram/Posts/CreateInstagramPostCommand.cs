using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions.Model;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Commands.Instagram.Posts
{
    public class CreateInstagramPostCommand:IRequest<Response<InstagramPostDto>>
    {
        public InstagramPostDto instagramPostDto { get; set; }

    }

    public class CreatePostCommandValidator : AbstractValidator<CreateInstagramPostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.instagramPostDto.Caption)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }

    public class CreateInstagramPostCommandHandler : IRequestHandler<CreateInstagramPostCommand, Response<InstagramPostDto>>
    {
        private readonly IInstagramPostRepository<Guid> _repository;

        private readonly IMapper _mapper;
        private readonly ILogger<CreateInstagramPostCommandHandler> _logger;

        public CreateInstagramPostCommandHandler(IInstagramPostRepository<Guid> repository, IMapper mapper, ILogger<CreateInstagramPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
           
        }

        public async Task<Response<InstagramPostDto>> Handle(CreateInstagramPostCommand request, CancellationToken cancellationToken)
        {
            Response<InstagramPostDto> result = new Response<InstagramPostDto>();
         

            var instagramPostEntity = _mapper.Map<InstagramPost>(request.instagramPostDto);

            var newEntity = await _repository.AddAsync(instagramPostEntity);
           
            

            if (newEntity != null )
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<InstagramPostDto>(newEntity);
                _logger.LogInformation($"InstagramPost {newEntity.Id} is successfully created.");
            }
            return result;
        }
    }
}
