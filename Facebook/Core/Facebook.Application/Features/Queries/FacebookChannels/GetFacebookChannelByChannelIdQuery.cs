using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Exceptions.Model;
using SNS.Facebook.Application.Interfaces;


namespace SNS.Facebook.Application.Features.Queries.FacebookChannels
{ 
    public class GetFacebookChannelByChannelIdQuery : IRequest<Response<FacebookChannelDto>>
    {
        public GetFacebookChannelByChannelIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class GetFacebookChannelByChannelIdQueryHandler : IRequestHandler<GetFacebookChannelByChannelIdQuery, Response<FacebookChannelDto>>
    {
        private readonly IFacebookChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFacebookChannelByChannelIdQueryHandler> _logger;
        public GetFacebookChannelByChannelIdQueryHandler(IFacebookChannelRepository<Guid> repository, IMapper mapper, ILogger<GetFacebookChannelByChannelIdQueryHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<FacebookChannelDto>> Handle(GetFacebookChannelByChannelIdQuery request, CancellationToken cancellationToken)
        {       
            Response<FacebookChannelDto> result = new Response<FacebookChannelDto>();
            var entity = await _repository.GetByChannelIdAsync(request.Id);
            if (entity != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<FacebookChannelDto>(entity);
                _logger.LogInformation($"FacebookChannel is successfully get.");
            }
            return result;
        }
    }
}
