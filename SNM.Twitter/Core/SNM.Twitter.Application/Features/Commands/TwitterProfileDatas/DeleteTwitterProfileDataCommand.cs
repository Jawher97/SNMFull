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

namespace SNM.Twitter.Application.Features.Commands.TwitterProfileDatas
{
    public class DeleteTwitterProfileDataCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteTwitterProfileDataCommandHandler : IRequestHandler<DeleteTwitterProfileDataCommand>
    {
        private readonly ITwitterProfileDataRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTwitterProfileDataCommandHandler> _logger;

        public DeleteTwitterProfileDataCommandHandler(ITwitterProfileDataRepository<Guid> repository, IMapper mapper, ILogger<DeleteTwitterProfileDataCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteTwitterProfileDataCommand request, CancellationToken cancellationToken)
        {
            var brandToDelete = await _repository.GetByIdAsync(request.Id);
            if (brandToDelete == null)
            {
                throw new NotFoundException(nameof(TwitterProfileData), request.Id);
            }

            await _repository.DeleteAsync(brandToDelete);

            _logger.LogInformation($"TwitterProfileData {brandToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
