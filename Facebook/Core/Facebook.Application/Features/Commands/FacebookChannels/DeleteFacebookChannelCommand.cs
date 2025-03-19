using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Application.Exceptions;
using SNS.Facebook.Application.Interfaces;
using SNS.Facebook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNS.Facebook.Application.Features.Commands.FacebookChannels
{
    public class DeleteFacebookChannelCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteFacebookChannelCommandHandler : IRequestHandler<DeleteFacebookChannelCommand>
    {
        private readonly IFacebookChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteFacebookChannelCommandHandler> _logger;

        public DeleteFacebookChannelCommandHandler(IFacebookChannelRepository<Guid> repository, IMapper mapper, ILogger<DeleteFacebookChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteFacebookChannelCommand request, CancellationToken cancellationToken)
        {
            var brandToDelete = await _repository.GetByIdAsync(request.Id);
            if (brandToDelete == null)
            {
                throw new NotFoundException(nameof(FacebookChannel), request.Id);
            }

            await _repository.DeleteAsync(brandToDelete);

            _logger.LogInformation($"FacebookChannel {brandToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
