using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.Exceptions;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;


namespace SNM.Twitter.Application.Features.Commands.TwitterProfileDatas
{
    public class UpdateTwitterProfileDataCommand : IRequest
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
    }

    public class UpdateTwitterProfileDataCommandValidator : AbstractValidator<UpdateTwitterProfileDataCommand>
    {
        public UpdateTwitterProfileDataCommandValidator()
        {
            RuleFor(p => p.DisplayName)
                .NotEmpty().WithMessage("{DisplayName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }

    public class UpdateCommandHandler : IRequestHandler<UpdateTwitterProfileDataCommand>
    {
        private readonly ITwitterProfileDataRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(ITwitterProfileDataRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateTwitterProfileDataCommand request, CancellationToken cancellationToken)
        {
            var twitterToUpdate = await _repository.GetByIdAsync(request.Id);
            if (twitterToUpdate == null)
            {
                throw new NotFoundException(nameof(TwitterProfileData), request.Id);
            }

            _mapper.Map(request, twitterToUpdate, typeof(UpdateTwitterProfileDataCommand), typeof(TwitterProfileData));
            await _repository.UpdateAsync(twitterToUpdate);

            _logger.LogInformation($"TwitterProfileData {twitterToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
