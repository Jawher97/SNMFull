using AutoMapper;
using SNM.BrandManagement.Application.Exceptions;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SNM.BrandManagement.Application.Features.Commands.Brands.UpdateBrand
{
    public class UpdateCommandHandler : IRequestHandler<UpdateBrandCommand>
    {
        private readonly IBrandRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IBrandRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var facebookToUpdate = await _repository.GetByIdAsync(request.Id);
            if (facebookToUpdate == null)
            {
                throw new NotFoundException(nameof(Brand), request.Id);
            }

            _mapper.Map(request, facebookToUpdate, typeof(UpdateBrandCommand), typeof(Brand));
            await _repository.UpdateAsync(facebookToUpdate);

            _logger.LogInformation($"Entity {facebookToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}