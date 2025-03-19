using AutoMapper;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SNM.Instagram.Application.Features.Commands.UpdateInstagram
{
    public class UpdateCommandHandler : IRequestHandler<UpdateInstagramCommand>
    {
        private readonly IInstagramRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(IInstagramRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateInstagramCommand request, CancellationToken cancellationToken)
        {
            var instagramToUpdate = await _repository.GetByIdAsync(request.Id);
            if (instagramToUpdate == null)
            {
                throw new NotFoundException(nameof(Entity), request.Id);
            }

            _mapper.Map(request, instagramToUpdate, typeof(UpdateInstagramCommand), typeof(Entity));
            await _repository.UpdateAsync(instagramToUpdate);

            _logger.LogInformation($"Entity {instagramToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}