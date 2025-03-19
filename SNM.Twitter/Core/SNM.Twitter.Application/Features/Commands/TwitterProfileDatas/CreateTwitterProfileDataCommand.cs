using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.TwitterProfileDatas
{
    public class CreateTwitterProfileDataCommand : IRequest<Guid>
    {
        public TwitterProfileDataDto TwitterProfileDataDto { get; set; }
    }

    public class CreateTwitterProfileDataCommandValidator : AbstractValidator<CreateTwitterProfileDataCommand>
    {
        public CreateTwitterProfileDataCommandValidator()
        {
            RuleFor(p => p.TwitterProfileDataDto)
                .NotEmpty().WithMessage("{TwitterProfileData} is required.")
                .NotNull();
        }
    }

    public class CreateTwitterProfileDataCommandHandler : IRequestHandler<CreateTwitterProfileDataCommand, Guid>
    {
        private readonly ITwitterProfileDataRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTwitterProfileDataCommandHandler> _logger;

        public CreateTwitterProfileDataCommandHandler(ITwitterProfileDataRepository<Guid> repository, IMapper mapper, ILogger<CreateTwitterProfileDataCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateTwitterProfileDataCommand request, CancellationToken cancellationToken)
        {
            var brandEntity = _mapper.Map<TwitterProfileData>(request.TwitterProfileDataDto);
            var newEntity = await _repository.AddAsync(brandEntity);

            _logger.LogInformation($"TwitterProfileData {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}
