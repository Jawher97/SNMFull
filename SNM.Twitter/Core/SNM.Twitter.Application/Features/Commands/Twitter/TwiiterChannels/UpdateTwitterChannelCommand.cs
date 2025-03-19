using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.Twitter.TwiiterChannels
{
    public class UpdateTwitterChannelCommand : IRequest<Response<TwitterChannelDto>>
    {
        public TwitterChannelDto TwitterChannelDto { get; set; }
    }
    public class UpdateChannelLinkedinCommandValidator : AbstractValidator<UpdateTwitterChannelCommand>
    {
        public UpdateChannelLinkedinCommandValidator()
        {
            RuleFor(p => p.TwitterChannelDto.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .NotNull();

        }
    }
    public class UpdateLinkedInChannelCommandHandler : IRequestHandler<UpdateTwitterChannelCommand, Response<TwitterChannelDto>>
    {
        private readonly ITwitterChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLinkedInChannelCommandHandler> _logger;

        public UpdateLinkedInChannelCommandHandler(ITwitterChannelRepository<Guid> repository, IMapper mapper, ILogger<UpdateLinkedInChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<TwitterChannelDto>> Handle(UpdateTwitterChannelCommand request, CancellationToken cancellationToken)
        {
            Response<TwitterChannelDto> result = new Response<TwitterChannelDto>();

            var linkedinToUpdate = await _repository.GetByIdAsync(request.TwitterChannelDto.Id);
            if (linkedinToUpdate == null)
            {
                throw new NotFoundException(nameof(TwitterChannelDto), request.TwitterChannelDto.Id);
            }

            _mapper.Map(request.TwitterChannelDto, linkedinToUpdate);

            await _repository.UpdateAsync(linkedinToUpdate);

            result.Succeeded = true;
            result.Data = request.TwitterChannelDto;
            _logger.LogInformation($"twitterChannel {request.TwitterChannelDto} is successfully updated.");

            return result;
        }
    }
}

