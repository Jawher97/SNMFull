using MediatR;
using FluentValidation;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Application.Exceptions;

namespace SNM.BrandManagement.Application.Features.Commands.Posts
{
    public class UpdatePostCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        
    }
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Message} must not exceed 250 characters.");
        }
    }
    public class UpdateCommandHandler : IRequestHandler<UpdatePostCommand>
    {
        private readonly IPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IPostRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var facebookToUpdate = await _repository.GetByIdAsync(request.Id);
            if (facebookToUpdate == null)
            {
                throw new NotFoundException(nameof(Post), request.Id);
            }

            _mapper.Map(request, facebookToUpdate, typeof(UpdatePostCommand), typeof(Post));
            await _repository.UpdateAsync(facebookToUpdate);

            _logger.LogInformation($"Post {facebookToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}