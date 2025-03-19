using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Interfaces;

namespace SNM.BrandManagement.Application.Features.Queries.Brand.GetBrands
{
    public class GetBrandsAndChannelsQuery : IRequest<List<BrandDto>>
    {
    }
    public class GetBrandsAndChannelsHandler : IRequestHandler<GetBrandsAndChannelsQuery, List<BrandDto>>
    {
        private readonly IBrandRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetBrandsAndChannelsHandler(IBrandRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<BrandDto>> Handle(GetBrandsAndChannelsQuery request, CancellationToken cancellationToken)
        {
             var entities = await _repository.GetAllWithChannelAsync();

            return await Task.FromResult(_mapper.Map<List<BrandDto>>(entities));
        }
    }
}
