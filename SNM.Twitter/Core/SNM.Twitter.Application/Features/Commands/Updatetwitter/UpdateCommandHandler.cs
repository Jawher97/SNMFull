using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.Exceptions;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Features.Commands.Updatetwitter
{
    public class UpdateCommandHandler : IRequestHandler<UpdatetwitterCommand>
    {
        private readonly ItwitterRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;

        public UpdateCommandHandler(ItwitterRepository<Guid> repository, IMapper mapper, ILogger<UpdateCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdatetwitterCommand request, CancellationToken cancellationToken)
        {
            var twitterToUpdate = await _repository.GetByIdAsync(request.Id);
            if (twitterToUpdate == null)
            {
                throw new NotFoundException(nameof(Entity), request.Id);
            }

            _mapper.Map(request, twitterToUpdate, typeof(UpdatetwitterCommand), typeof(Entity));
            await _repository.UpdateAsync(twitterToUpdate);

            _logger.LogInformation($"Entity {twitterToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}