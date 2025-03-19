using AutoMapper;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SNM.BrandManagement.Application.Features.Commands.Brands.CreateBrand
{
    public class CreateCommandHandler : IRequestHandler<CreateBrandCommand, Guid>
    {
        private readonly IBrandRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommandHandler> _logger;

        public CreateCommandHandler(IBrandRepository<Guid> repository, IMapper mapper, ILogger<CreateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brandEntity = _mapper.Map<Brand>(request);
            var newEntity = await _repository.AddAsync(brandEntity);

            _logger.LogInformation($"Entity {newEntity.Id} is successfully created.");

            return newEntity.Id;
        }
    }
}