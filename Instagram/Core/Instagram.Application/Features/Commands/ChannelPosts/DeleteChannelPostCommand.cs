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

namespace SNM.Instagram.Application.Features.Commands.ChannelPosts
{
    public class DeleteChannelPostCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteChannelPostCommandHandler : IRequestHandler<DeleteChannelPostCommand>
    {
        private readonly IChannelPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteChannelPostCommandHandler> _logger;

        public DeleteChannelPostCommandHandler(IChannelPostRepository<Guid> repository, IMapper mapper, ILogger<DeleteChannelPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteChannelPostCommand request, CancellationToken cancellationToken)
        {
            var channelPostToDelete = await _repository.GetByIdAsync(request.Id);
            if (channelPostToDelete == null)
            {
                throw new NotFoundException(nameof(ChannelPost), request.Id);
            }

            await _repository.DeleteAsync(channelPostToDelete);

            _logger.LogInformation($"ChannelPost {channelPostToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}

