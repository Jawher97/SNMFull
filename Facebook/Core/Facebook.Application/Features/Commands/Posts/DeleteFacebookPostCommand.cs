using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Application.Exceptions;
using SNS.Facebook.Application.Interfaces;
using SNS.Facebook.Domain.Entities;


namespace SNS.Facebook.Application.Features.Commands.Posts
{
    public class DeleteFacebookPostCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteFacebookPostCommandHandler : IRequestHandler<DeleteFacebookPostCommand>
    {
        private readonly IFacebookPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteFacebookPostCommandHandler> _logger;

        public DeleteFacebookPostCommandHandler(IFacebookPostRepository<Guid> repository, IMapper mapper, ILogger<DeleteFacebookPostCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteFacebookPostCommand request, CancellationToken cancellationToken)
        {
            var facebookPostToDelete = await _repository.GetByIdAsync(request.Id);
           
            if (facebookPostToDelete == null)
            {
                throw new NotFoundException(nameof(FacebookPost), request.Id);
            }

            await _repository.DeleteAsync(facebookPostToDelete);

            _logger.LogInformation($"FacebookPost {facebookPostToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
