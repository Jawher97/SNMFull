using MediatR;
using FluentValidation;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Application.Exceptions;
using SNM.BrandManagement.Domain.Enumeration;
using SNM.BrandManagement.Application.DTO;

namespace SNM.BrandManagement.Application.Features.Commands.Channels
{
    public class UpdateChannelCommand : IRequest
    {
       public ChannelDto channelDto { get; set; }
    }
    public class UpdateChannelCommandValidator : AbstractValidator<UpdateChannelCommand>
    {
        public UpdateChannelCommandValidator()
        {
            RuleFor(p => p.channelDto.DisplayName)
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
            var Update = await _repository.GetByIdAsync(request.channelDto.Id);
            if (Update == null)
            {
                throw new NotFoundException(nameof(Channel), request.channelDto.Id);
            }

            _mapper.Map(request, Update, typeof(ChannelDto), typeof(Channel));
            Update.IsActivated = ActivationStatus.Active;
            await _repository.UpdateAsync(Update);

            _logger.LogInformation($"Channel {Update.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}