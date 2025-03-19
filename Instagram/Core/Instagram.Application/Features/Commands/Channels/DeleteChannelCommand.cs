using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Commands.Channels
{
    public class DeleteChannelCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteChannelCommandHandler : IRequestHandler<DeleteChannelCommand>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteChannelCommandHandler> _logger;

        public DeleteChannelCommandHandler(IChannelRepository<Guid> repository, IMapper mapper, ILogger<DeleteChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
        {
            var brandToDelete = await _repository.GetByIdAsync(request.Id);
            if (brandToDelete == null)
            {
                throw new NotFoundException(nameof(Channel), request.Id);
            }

            await _repository.DeleteAsync(brandToDelete);

            _logger.LogInformation($"Channel {brandToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
