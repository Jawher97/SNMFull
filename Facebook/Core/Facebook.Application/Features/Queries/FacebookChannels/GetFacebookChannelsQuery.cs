using AutoMapper;
using MediatR;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Interfaces;


namespace SNS.Facebook.Application.Features.Queries.FacebookChannels
{
    public class GetFacebookChannelsQuery : IRequest<List<FacebookChannelDto>>
    {
    }
    public class GetFacebookChannelsViewModel
    {
        public GetFacebookChannelsViewModel(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetFacebookChannelListQueryHandler : IRequestHandler<GetFacebookChannelsQuery, List<FacebookChannelDto>>
    {
        private readonly IFacebookChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetFacebookChannelListQueryHandler(IFacebookChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<FacebookChannelDto>> Handle(GetFacebookChannelsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<FacebookChannelDto>>(entities));
        }
    }
}
