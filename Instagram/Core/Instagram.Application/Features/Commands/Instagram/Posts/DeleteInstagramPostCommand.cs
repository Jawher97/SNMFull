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

namespace SNM.Instagram.Application.Features.Commands.Instagram.Posts
{
    public class DeleteInstagramPostCommand:IRequest
    {
        public Guid Id { get; set; }

    }
    public class DeleteInstagramCommandHandler : IRequestHandler<DeleteInstagramPostCommand>
    {
        private readonly IInstagramPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteInstagramCommandHandler> _logger;

        public DeleteInstagramCommandHandler(IInstagramPostRepository<Guid> repository, IMapper mapper, ILogger<DeleteInstagramCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteInstagramPostCommand request, CancellationToken cancellationToken)
        {
            var instagramToDelete = await _repository.GetByIdAsync(request.Id);
            if (instagramToDelete == null)
            {
                throw new NotFoundException(nameof(InstagramPost), request.Id);
            }

            await _repository.DeleteAsync(instagramToDelete);

            _logger.LogInformation($"InstagramPost {instagramToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }

}
