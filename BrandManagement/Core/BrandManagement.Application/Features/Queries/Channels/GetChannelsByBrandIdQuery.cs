using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Interfaces;

namespace SNM.BrandManagement.Application.Features.Queries.Channels
{ 
    public class GetChannelsByBrandIdQuery : IRequest<List<ChannelDto>>
    {
        public GetChannelsByBrandIdQuery(Guid id)
        {
            BrandId = id;
        }

        public Guid BrandId { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetChannelParameterBrandId
    {
        public ChannelDto Channel { get; set; }  
       
    }
    public class GetChannelByBrandIdQueryHandler : IRequestHandler<GetChannelsByBrandIdQuery, List<ChannelDto>>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelByBrandIdQueryHandler(IChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ChannelDto>> Handle(GetChannelsByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAllByBrandIdAsync(request.BrandId);

            return await Task.FromResult(_mapper.Map<List<ChannelDto>>(entity));
        }
    }
}
