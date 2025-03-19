using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.Exceptions;
using SNM.LinkedIn.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts
{
    public class DeleteLinkedInPostCommand : IRequest<Unit> 
    {
        public Guid Id { get; set; }
    }

    public class DeleteLinledinPostCommandHandler : IRequestHandler<DeleteLinkedInPostCommand, Unit> // Ajoutez Unit comme type de résultat
    {
        private readonly ILinkedInPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteLinledinPostCommandHandler> _logger;

        public DeleteLinledinPostCommandHandler(ILinkedInPostRepository<Guid> repository, IMapper mapper, ILogger<DeleteLinledinPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteLinkedInPostCommand request, CancellationToken cancellationToken)
        {
            var linkedinPostToDelete = await _repository.GetByIdAsync(request.Id);

            if (linkedinPostToDelete == null)
            {
                throw new NotFoundException(nameof(LinkedInPost), request.Id);
            }

            await _repository.DeleteAsync(linkedinPostToDelete);

            _logger.LogInformation($"LinkeinPost {linkedinPostToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
