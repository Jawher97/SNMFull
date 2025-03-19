using AutoMapper;
using SNM.Instagram.Application.Interfaces;
using MediatR;

namespace SNM.Instagram.Application.Features.Queries.GetEntities
{
    public class GetEntityListQueryHandler : IRequestHandler<GetEntitiesQuery, List<GetEntitiesViewModel>>
    {
        private readonly IInstagramRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetEntityListQueryHandler(IInstagramRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GetEntitiesViewModel>> Handle(GetEntitiesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<GetEntitiesViewModel>>(entities));
        }
    }
}