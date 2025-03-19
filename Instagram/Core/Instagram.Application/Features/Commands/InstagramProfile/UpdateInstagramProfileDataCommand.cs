using MediatR;
using FluentValidation;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions;

namespace SNM.Instagram.Application.Features.Commands.InstagramProfile
{
    public class UpdateInstagramProfileDataCommand : IRequest
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
    }
    public class UpdateInstagramProfileDataCommandValidator : AbstractValidator<UpdateInstagramProfileDataCommand>
    {
        public UpdateInstagramProfileDataCommandValidator()
        {
            RuleFor(p => p.DisplayName)
                .NotEmpty().WithMessage("{DisplayName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
    public class UpdateCommandHandler : IRequestHandler<UpdateInstagramProfileDataCommand>
    {
        private readonly IInstagramProfileDataRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IInstagramProfileDataRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateInstagramProfileDataCommand request, CancellationToken cancellationToken)
        {
            var InstagramProfileDataToUpdate = await _repository.GetByIdAsync(request.Id);
            if (InstagramProfileDataToUpdate == null)
            {
                throw new NotFoundException(nameof(InstagramProfileData), request.Id);
            }

            _mapper.Map(request, InstagramProfileDataToUpdate, typeof(UpdateInstagramProfileDataCommand), typeof(InstagramProfileData));
            await _repository.UpdateAsync(InstagramProfileDataToUpdate);

            _logger.LogInformation($"InstagramProfileData {InstagramProfileDataToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
