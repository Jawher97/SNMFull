using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.ChannelTypes
{
    public class GetChannelTypesByNameQuery : IRequest<ChannelTypeDto>
    {
        public GetChannelTypesByNameQuery(string name)
        {
            Name = name;
        }

      public string Name { get; set; }
    }
    //public class GetChannelTypeParameterBrandId
    //{
    //    public ChannelTypeDto ChannelType { get; set; }

    //}
    public class GetChannelTypesByNameQueryHandler : IRequestHandler<GetChannelTypesByNameQuery, ChannelTypeDto>
    {
        private readonly IChannelTypeRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelTypesByNameQueryHandler(IChannelTypeRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ChannelTypeDto> Handle(GetChannelTypesByNameQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAllByNameAsync(request.Name);

            return await Task.FromResult(_mapper.Map<ChannelTypeDto>(entity));
        }
    }
}

