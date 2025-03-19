using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.Interfaces;


namespace SNM.BrandManagement.Application.Features.Queries.Channels
{ 
    public class GetChannelByIdQuery : IRequest<GetChannelParameterId>
    {
        public GetChannelByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetChannelParameterId
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetChannelByIdQueryHandler : IRequestHandler<GetChannelByIdQuery, GetChannelParameterId>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelByIdQueryHandler(IChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetChannelParameterId> Handle(GetChannelByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<GetChannelParameterId>(entity));
        }
    }
}
