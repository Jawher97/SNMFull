using MediatR;
using FluentValidation;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Application.Exceptions;

namespace SNS.Facebook.Application.Features.Commands.FacebookChannels
{
    public class UpdateFacebookChannelCommand : IRequest
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
    }
    public class UpdateFacebookChannelCommandValidator : AbstractValidator<UpdateFacebookChannelCommand>
    {
        public UpdateFacebookChannelCommandValidator()
        {
            RuleFor(p => p.DisplayName)
                .NotEmpty().WithMessage("{DisplayName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
    public class UpdateCommandHandler : IRequestHandler<UpdateFacebookChannelCommand>
    {
        private readonly IFacebookChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IFacebookChannelRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateFacebookChannelCommand request, CancellationToken cancellationToken)
        {
            var facebookToUpdate = await _repository.GetByIdAsync(request.Id);
            if (facebookToUpdate == null)
            {
                throw new NotFoundException(nameof(FacebookChannel), request.Id);
            }

            _mapper.Map(request, facebookToUpdate, typeof(UpdateFacebookChannelCommand), typeof(FacebookChannel));
            await _repository.UpdateAsync(facebookToUpdate);

            _logger.LogInformation($"FacebookChannel {facebookToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}