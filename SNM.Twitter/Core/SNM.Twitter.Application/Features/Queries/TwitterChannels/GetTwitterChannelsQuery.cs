using AutoMapper;
using MediatR;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Interfaces;

namespace SNM.Twitter.Application.Features.Queries.TwitterChannels
{
    public class GetTwitterChannelsQuery : IRequest<List<TwitterChannelDto>>
    {

    }

    public class GetTwitterChannelsViewModel
    {
        public GetTwitterChannelsViewModel(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class GetTwitterChannelListQueryHandler : IRequestHandler<GetTwitterChannelsQuery, List<TwitterChannelDto>>
    {
        private readonly ITwitterChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetTwitterChannelListQueryHandler(ITwitterChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TwitterChannelDto>> Handle(GetTwitterChannelsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<TwitterChannelDto>>(entities));
        }
    }
}
