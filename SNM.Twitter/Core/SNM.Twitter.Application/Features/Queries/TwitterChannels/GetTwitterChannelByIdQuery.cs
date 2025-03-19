using AutoMapper;
using MediatR;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Interfaces;


namespace SNM.Twitter.Application.Features.Queries.TwitterChannels
{
    public class GetTwitterChannelByIdQuery : IRequest<TwitterChannelDto>
    {
        public GetTwitterChannelByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    
    }

    public class GetTwitterChannelParameterId
    {
        public Guid Id { get; set; }
   
    }

    public class GetTwitterChannelByIdQueryHandler : IRequestHandler<GetTwitterChannelByIdQuery, TwitterChannelDto>
    {
        private readonly ITwitterChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetTwitterChannelByIdQueryHandler(ITwitterChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TwitterChannelDto> Handle(GetTwitterChannelByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByChannelIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<TwitterChannelDto>(entity));
        }
    }
}
