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

namespace SNM.Instagram.Application.Features.Commands.FacebookChannels
{
    public class DeleteInstagramChannelCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteInstagramChannelCommandHandler : IRequestHandler<DeleteInstagramChannelCommand>
    {
        private readonly IInstagramChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteInstagramChannelCommandHandler> _logger;

        public DeleteInstagramChannelCommandHandler(IInstagramChannelRepository<Guid> repository, IMapper mapper, ILogger<DeleteInstagramChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteInstagramChannelCommand request, CancellationToken cancellationToken)
        {
            var InstagramChannelToDelete = await _repository.GetByIdAsync(request.Id);
            if (InstagramChannelToDelete == null)
            {
                throw new NotFoundException(nameof(InstagramChannel), request.Id);
            }

            await _repository.DeleteAsync(InstagramChannelToDelete);

            _logger.LogInformation($"InstagramChannel {InstagramChannelToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
