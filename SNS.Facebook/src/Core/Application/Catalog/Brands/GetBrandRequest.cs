﻿namespace SNS.Facebook.Application.Catalog.Brands
{
    public class GetBrandRequest : IRequest<BrandDto>
    {
        public Guid Id { get; set; }

        public GetBrandRequest(Guid id) => Id = id;
    }

    public class BrandByIdSpec : Specification<Brand, BrandDto>, ISingleResultSpecification
    {
        public BrandByIdSpec(Guid id) =>
            Query.Where(p => p.Id == id);
    }

    public class GetBrandRequestHandler : IRequestHandler<GetBrandRequest, BrandDto>
    {
        private readonly IRepository<Brand> _repository;
        private readonly IStringLocalizer<GetBrandRequestHandler> _localizer;

        public GetBrandRequestHandler(IRepository<Brand> repository, IStringLocalizer<GetBrandRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

        public async Task<BrandDto> Handle(GetBrandRequest request, CancellationToken cancellationToken) =>
            await _repository.GetBySpecAsync(
                (ISpecification<Brand, BrandDto>)new BrandByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException(string.Format(_localizer["brand.notfound"], request.Id));
    }
}