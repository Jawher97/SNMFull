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

namespace SNM.BrandManagement.Application.Features.Commands.Posts
{
    public class DeletePostCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeletePostCommandHandler> _logger;

        public DeletePostCommandHandler(IPostRepository<Guid> repository, IMapper mapper, ILogger<DeletePostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var brandToDelete = await _repository.GetByIdAsync(request.Id);
            if (brandToDelete == null)
            {
                throw new NotFoundException(nameof(Post), request.Id);
            }

            await _repository.DeleteAsync(brandToDelete);

            _logger.LogInformation($"Post {brandToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
