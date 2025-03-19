using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Exceptions;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts
{
    public class UpdatePostLinkedinCommand : IRequest<Response<LinkedInPostDto>>
    {
        public LinkedInPostDto LinkedinPostDto { get; set; }
    }
    public class UpdatePostLinkedinCommandValidator : AbstractValidator<UpdatePostLinkedinCommand>
    {
        public UpdatePostLinkedinCommandValidator()
        {
            RuleFor(p => p.LinkedinPostDto.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .NotNull();

        }
    }
    public class UpdateLinkedInCommandHandler : IRequestHandler<UpdatePostLinkedinCommand, Response<LinkedInPostDto>>
    {
        private readonly ILinkedInPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePostLinkedinCommandValidator> _logger;

        public UpdateLinkedInCommandHandler(ILinkedInPostRepository<Guid> repository, IMapper mapper, ILogger<UpdatePostLinkedinCommandValidator> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<LinkedInPostDto>> Handle(UpdatePostLinkedinCommand request, CancellationToken cancellationToken)
        {
            Response<LinkedInPostDto> result = new Response<LinkedInPostDto>();

            var linkedinToUpdate = await _repository.GetByIdAsync(request.LinkedinPostDto.Id);
            if (linkedinToUpdate == null)
            {
                throw new NotFoundException(nameof(LinkedInPostDto), request.LinkedinPostDto.Id);
            }

            _mapper.Map(request.LinkedinPostDto, linkedinToUpdate);

            await _repository.UpdateAsync(linkedinToUpdate);

            result.Succeeded = true;
            result.Data = request.LinkedinPostDto;
            _logger.LogInformation($"LinkedinPost {request.LinkedinPostDto} is successfully updated.");

            return result;
        }
    }
}

