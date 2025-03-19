using AutoMapper;
using MediatR;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Interfaces;

namespace SNM.Instagram.Application.Features.Queries.InstagramPosts
{
public class GetInstagramPostsByIdQuery : IRequest<InstagramPostDto>
{
    public Guid Id { get; set; }
        public GetInstagramPostsByIdQuery(Guid id)
        {
            Id = id;
        }
    }

public class GetInstagramPostByIdQueryHandler : IRequestHandler<GetInstagramPostsByIdQuery, InstagramPostDto>
{
    private readonly IInstagramPostRepository<Guid> _repository;
    private readonly IMapper _mapper;

    public GetInstagramPostByIdQueryHandler(IInstagramPostRepository<Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

        public async Task<InstagramPostDto> Handle(GetInstagramPostsByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByIdAsync(request.Id);
            return _mapper.Map<InstagramPostDto>(post);
        }
    }
}