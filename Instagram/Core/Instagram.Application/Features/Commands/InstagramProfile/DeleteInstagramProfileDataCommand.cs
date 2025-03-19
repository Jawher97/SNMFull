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

namespace SNM.Instagram.Application.Features.Commands.InstagramProfile
{
    public class DeleteInstagramProfileDataCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteInstagramProfileDataCommandHandler : IRequestHandler<DeleteInstagramProfileDataCommand>
    {
        private readonly IInstagramProfileDataRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteInstagramProfileDataCommandHandler> _logger;

        public DeleteInstagramProfileDataCommandHandler(IInstagramProfileDataRepository<Guid> repository, IMapper mapper, ILogger<DeleteInstagramProfileDataCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteInstagramProfileDataCommand request, CancellationToken cancellationToken)
        {
            var InstagramProfileDataToDelete = await _repository.GetByIdAsync(request.Id);
            if (InstagramProfileDataToDelete == null)
            {
                throw new NotFoundException(nameof(InstagramProfileData), request.Id);
            }

            await _repository.DeleteAsync(InstagramProfileDataToDelete);

            _logger.LogInformation($"InstagramProfileData {InstagramProfileDataToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}

