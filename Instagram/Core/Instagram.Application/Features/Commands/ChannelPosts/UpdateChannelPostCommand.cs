using MediatR;
using FluentValidation;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions;

namespace SNM.Instagram.Application.Features.Commands.ChannelPosts
{
    public class UpdateChannelPostCommand : IRequest
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
    }
    public class UpdateChannelPostCommandValidator : AbstractValidator<UpdateChannelPostCommand>
    {
        public UpdateChannelPostCommandValidator()
        {
            RuleFor(p => p.DisplayName)
                .NotEmpty().WithMessage("{DisplayName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
    public class UpdateCommandHandler : IRequestHandler<UpdateChannelPostCommand>
    {
        private readonly IChannelPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IChannelPostRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateChannelPostCommand request, CancellationToken cancellationToken)
        {
            var channelPostToUpdate = await _repository.GetByIdAsync(request.Id);
            if (channelPostToUpdate == null)
            {
                throw new NotFoundException(nameof(ChannelPost), request.Id);
            }

            _mapper.Map(request, channelPostToUpdate, typeof(UpdateChannelPostCommand), typeof(ChannelPost));
            await _repository.UpdateAsync(channelPostToUpdate);

            _logger.LogInformation($"ChannelPost {channelPostToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
