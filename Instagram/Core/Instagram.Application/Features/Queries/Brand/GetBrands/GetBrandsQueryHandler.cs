using AutoMapper;
using SNM.Instagram.Application.Interfaces;
using MediatR;
using SNM.Instagram.Application.DTO;

namespace SNM.Instagram.Application.Features.Queries.Brand.GetBrands
{
    public class GetBrandListQueryHandler : IRequestHandler<GetBrandsQuery, List<BrandDto>>
    {
        private readonly IBrandRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetBrandListQueryHandler(IBrandRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<BrandDto>>(entities));
        }
    }
}
