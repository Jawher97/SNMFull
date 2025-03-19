using AutoMapper;
using MediatR;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Interfaces;


namespace SNS.Facebook.Application.Features.Queries.FacebookChannels
{
    public class GetFacebookChannelsByBrandIdQuery : IRequest<List<FacebookChannelDto>>
    {
        public GetFacebookChannelsByBrandIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
    }
    public class GetFacebookChannelsByBrandIdViewModel
    {
        public GetFacebookChannelsByBrandIdViewModel(Guid id, string displayName,Guid brandId)
        {
            Id = id;
            DisplayName = displayName;
            BrandId = brandId;
    }

        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetFacebookChannelListByBrandIdQueryHandler : IRequestHandler<GetFacebookChannelsByBrandIdQuery, List<FacebookChannelDto>>
    {
        private readonly IFacebookChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetFacebookChannelListByBrandIdQueryHandler(IFacebookChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<FacebookChannelDto>> Handle(GetFacebookChannelsByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAsync(c=>c.SocialChannel.BrandId==request.Id);

            return await Task.FromResult(_mapper.Map<List<FacebookChannelDto>>(entities));
        }
    }
}
