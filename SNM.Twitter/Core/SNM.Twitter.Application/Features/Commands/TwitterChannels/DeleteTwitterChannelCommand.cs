using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.Exceptions;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.TwitterChannels
{
    public class DeleteTwitterChannelCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteTwitterChannelCommandHandler : IRequestHandler<DeleteTwitterChannelCommand>
    {
        private readonly ITwitterChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTwitterChannelCommandHandler> _logger;

        public DeleteTwitterChannelCommandHandler(ITwitterChannelRepository<Guid> repository, IMapper mapper, ILogger<DeleteTwitterChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteTwitterChannelCommand request, CancellationToken cancellationToken)
        {
            var brandToDelete = await _repository.GetByChannelIdAsync(request.Id);
            if (brandToDelete == null)
            {
                throw new NotFoundException(nameof(TwitterChannel), request.Id);
            }

            await _repository.DeleteAsync(brandToDelete);

            _logger.LogInformation($"TwitterChannel {brandToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
