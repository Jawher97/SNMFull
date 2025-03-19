using AutoMapper;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SNM.Instagram.Application.Features.Commands.Brands.DeleteBrand
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
    {
        private readonly IBrandRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteBrandCommandHandler> _logger;

        public DeleteBrandCommandHandler(IBrandRepository<Guid> repository, IMapper mapper, ILogger<DeleteBrandCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brandToDelete = await _repository.GetByIdAsync(request.Id);
            if (brandToDelete == null)
            {
                throw new NotFoundException(nameof(Entity), request.Id);
            }

            await _repository.DeleteAsync(brandToDelete);

            _logger.LogInformation($"Entity {brandToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
