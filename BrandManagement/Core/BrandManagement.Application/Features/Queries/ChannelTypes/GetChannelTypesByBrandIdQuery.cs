

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
        public class GetChannelTypesByBrandIdQuery : IRequest<List<ChannelTypeDto>>
        {
            public GetChannelTypesByBrandIdQuery(Guid id)
            {
                BrandId = id;
            }

            public Guid BrandId { get; set; }
            public string DisplayName { get; set; }
        }
        //public class GetChannelTypeParameterBrandId
        //{
        //    public ChannelTypeDto ChannelType { get; set; }

        //}
        public class GetChannelTypesByBrandIdQueryHandler : IRequestHandler<GetChannelTypesByBrandIdQuery, List<ChannelTypeDto>>
        {
            private readonly IChannelTypeRepository<Guid> _repository;
            private readonly IMapper _mapper;

            public GetChannelTypesByBrandIdQueryHandler(IChannelTypeRepository<Guid> repository, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<List<ChannelTypeDto>> Handle(GetChannelTypesByBrandIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetAllByBrandIdAsync(request.BrandId);

                return await Task.FromResult(_mapper.Map<List<ChannelTypeDto>>(entity));
            }
        }
    }


