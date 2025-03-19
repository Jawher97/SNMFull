using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Exceptions;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts;
using SNM.LinkedIn.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Commands.LinkedeIn.LinkedInChannels
{
    public class UpdateLinkedInChannelCommand : IRequest<Response<LinkedInChannelDto>>
    {
        public LinkedInChannelDto LinkedInChannelDto { get; set; }
    }
    public class UpdateChannelLinkedinCommandValidator : AbstractValidator<UpdateLinkedInChannelCommand>
    {
        public UpdateChannelLinkedinCommandValidator()
        {
            RuleFor(p => p.LinkedInChannelDto.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .NotNull();

        }
    }
    public class UpdateLinkedInChannelCommandHandler : IRequestHandler<UpdateLinkedInChannelCommand, Response<LinkedInChannelDto>>
    {
        private readonly ILinkedInChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLinkedInChannelCommandHandler> _logger;

        public UpdateLinkedInChannelCommandHandler(ILinkedInChannelRepository<Guid> repository, IMapper mapper, ILogger<UpdateLinkedInChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<LinkedInChannelDto>> Handle(UpdateLinkedInChannelCommand request, CancellationToken cancellationToken)
        {
            Response<LinkedInChannelDto> result = new Response<LinkedInChannelDto>();

            var linkedinToUpdate = await _repository.GetByIdAsync(request.LinkedInChannelDto.Id);
            if (linkedinToUpdate == null)
            {
                throw new NotFoundException(nameof(LinkedInPostDto), request.LinkedInChannelDto.Id);
            }

            _mapper.Map(request.LinkedInChannelDto, linkedinToUpdate);

            await _repository.UpdateAsync(linkedinToUpdate);

            result.Succeeded = true;
            result.Data = request.LinkedInChannelDto;
            _logger.LogInformation($"LinkedInChannel {request.LinkedInChannelDto} is successfully updated.");

            return result;
        }
    }
}

