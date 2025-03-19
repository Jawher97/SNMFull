using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.Exceptions;
using SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Commands.LinkedeIn.LinkedInChannels
{
    public class DeleteLinkedInChannelCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class DeleteLinledinChannelCommandHandler : IRequestHandler<DeleteLinkedInChannelCommand, Unit> // Ajoutez Unit comme type de résultat
    {
        private readonly ILinkedInChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteLinledinChannelCommandHandler> _logger;

        public DeleteLinledinChannelCommandHandler(ILinkedInChannelRepository<Guid> repository, IMapper mapper, ILogger<DeleteLinledinChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteLinkedInChannelCommand request, CancellationToken cancellationToken)
        {
            var Delete = await _repository.GetByIdAsync(request.Id);

            if (Delete == null)
            {
                throw new NotFoundException(nameof(LinkedInChannel), request.Id);
            }

            await _repository.DeleteAsync(Delete);

            _logger.LogInformation($"LinkeinChannel {Delete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
