using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.Brand
{ 
    public class GetBrandByIdQuery : IRequest<GetBrandParameterId>
    {
        public GetBrandByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetBrandParameterId
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, GetBrandParameterId>
    {
        private readonly IBrandRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetBrandByIdQueryHandler(IBrandRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetBrandParameterId> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<GetBrandParameterId>(entity));
        }
    }
}
