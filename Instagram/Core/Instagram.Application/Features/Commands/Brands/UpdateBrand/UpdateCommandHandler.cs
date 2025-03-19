using AutoMapper;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Features.Commands.Brands.UpdateBrand;

namespace SNM.Instagram.Application.Features.Commands.Brands.UpdateBrand
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
            var instagramToUpdate = await _repository.GetByIdAsync(request.Id);
            if (instagramToUpdate == null)
            {
                throw new NotFoundException(nameof(Entity), request.Id);
            }

            _mapper.Map(request, instagramToUpdate, typeof(UpdateBrandCommand), typeof(Entity));
            await _repository.UpdateAsync(instagramToUpdate);

            _logger.LogInformation($"Entity {instagramToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
