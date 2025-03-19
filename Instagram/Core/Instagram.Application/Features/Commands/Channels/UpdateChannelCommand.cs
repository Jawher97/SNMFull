using MediatR;
using FluentValidation;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions;

namespace SNM.Instagram.Application.Features.Commands.Channels
{
    public class UpdateChannelCommand : IRequest
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
    }
    public class UpdateChannelCommandValidator : AbstractValidator<UpdateChannelCommand>
    {
        public UpdateChannelCommandValidator()
        {
            RuleFor(p => p.DisplayName)
                .NotEmpty().WithMessage("{DisplayName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
    public class UpdateCommandHandler : IRequestHandler<UpdateChannelCommand>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IChannelRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateChannelCommand request, CancellationToken cancellationToken)
        {
            var instagramToUpdate = await _repository.GetByIdAsync(request.Id);
            if (instagramToUpdate == null)
            {
                throw new NotFoundException(nameof(Channel), request.Id);
            }

            _mapper.Map(request, instagramToUpdate, typeof(UpdateChannelCommand), typeof(Channel));
            await _repository.UpdateAsync(instagramToUpdate);

            _logger.LogInformation($"Channel {instagramToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
