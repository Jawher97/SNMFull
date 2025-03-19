using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Exceptions;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Commands.ChannelProfiles
{
    public class UpdateChannelProfileCommand : IRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Headline { get; set; }
        public string CoverPhoto { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }

        public string ProfileUserId { get; set; }
        public string ProfileLink { get; set; }

        public string AccessToken { get; set; }
        public string expires_in { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiresIn { get; set; }
        public string Scope { get; set; }
    }
    public class UpdateChannelProfileCommandValidator : AbstractValidator<UpdateChannelProfileCommand>
    {
        public UpdateChannelProfileCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .NotNull();
                //.MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
    public class UpdateCommandHandler : IRequestHandler<UpdateChannelProfileCommand>
    {
        private readonly IChannelProfileRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IChannelProfileRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateChannelProfileCommand request, CancellationToken cancellationToken)
        {
            var Update = await _repository.GetByIdAsync(request.Id);
            if (Update == null)
            {
                throw new NotFoundException(nameof(ChannelProfile), request.Id);
            }

            _mapper.Map(request, Update, typeof(UpdateChannelProfileCommand), typeof(Channel));
            await _repository.UpdateAsync(Update);

            _logger.LogInformation($"ChannelProfile {Update.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
