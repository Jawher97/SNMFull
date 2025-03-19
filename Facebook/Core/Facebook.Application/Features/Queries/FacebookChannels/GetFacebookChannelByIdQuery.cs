using AutoMapper;
using MediatR;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Interfaces;


namespace SNS.Facebook.Application.Features.Queries.FacebookChannels
{ 
    public class GetFacebookChannelByIdQuery : IRequest<FacebookChannelDto>
    {
        public GetFacebookChannelByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetFacebookChannelParameterId
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetFacebookChannelByIdQueryHandler : IRequestHandler<GetFacebookChannelByIdQuery, FacebookChannelDto>
    {
        private readonly IFacebookChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetFacebookChannelByIdQueryHandler(IFacebookChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<FacebookChannelDto> Handle(GetFacebookChannelByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<FacebookChannelDto>(entity));
        }
    }
}
