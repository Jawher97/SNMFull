using AutoMapper;
using MediatR;

namespace SNM.Instagram.Application.Features.Queries.InstagramPost
{
    public class GetInstagramPostQuery : IRequest<List<InstagramPostDto>>
    {
        public string SearchTerm { get; set; }

        public GetInstagramPostQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }

    public class GetInstagramPostsQueryHandler : IRequestHandler<GetInstagramPostQuery, List<InstagramPostDto>>
    {
        private readonly IInstagramPostRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetInstagramPostsQueryHandler(IInstagramPostRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<InstagramPostDto>> Handle(GetInstagramPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByHashtagAsync(request.SearchTerm);
            return _mapper.Map<List<InstagramPostDto>>(post);
        }
    }
}
