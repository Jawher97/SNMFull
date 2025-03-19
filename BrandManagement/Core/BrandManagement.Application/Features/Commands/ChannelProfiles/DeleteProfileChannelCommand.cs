using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Application.Exceptions;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Commands.ChannelProfiles
{
    public class DeleteProfileChannelCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteProfileChannelCommandHandler : IRequestHandler<DeleteProfileChannelCommand>
    {
        private readonly IChannelProfileRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteProfileChannelCommandHandler> _logger;

        public DeleteProfileChannelCommandHandler(IChannelProfileRepository<Guid> repository, IMapper mapper, ILogger<DeleteProfileChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteProfileChannelCommand request, CancellationToken cancellationToken)
        {
            var Delete = await _repository.GetByIdAsync(request.Id);
            if (Delete == null)
            {
                throw new NotFoundException(nameof(ChannelProfile), request.Id);
            }

            await _repository.DeleteAsync(Delete);

            _logger.LogInformation($"ChannelProfile {Delete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}

