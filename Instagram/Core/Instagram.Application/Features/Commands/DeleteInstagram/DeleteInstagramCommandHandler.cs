using AutoMapper;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SNM.Instagram.Application.Features.Commands.DeleteInstagram
{
    public class DeleteInstagramCommandHandler : IRequestHandler<DeleteInstagramCommand>
    {
        private readonly IInstagramRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteInstagramCommandHandler> _logger;

        public DeleteInstagramCommandHandler(IInstagramRepository<Guid> repository, IMapper mapper, ILogger<DeleteInstagramCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteInstagramCommand request, CancellationToken cancellationToken)
        {
            var instagramToDelete = await _repository.GetByIdAsync(request.Id);
            if (instagramToDelete == null)
            {
                throw new NotFoundException(nameof(Entity), request.Id);
            }

            await _repository.DeleteAsync(instagramToDelete);

            _logger.LogInformation($"Entity {instagramToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}