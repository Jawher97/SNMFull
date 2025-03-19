using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.ChannelProfiles
{
    public class GetChannelProfileByBrandIdQuery : IRequest<List<ChannelProfileDto>>
    {
        public GetChannelProfileByBrandIdQuery(Guid id)
        {
            BrandId = id;
        }

        public Guid BrandId { get; set; }
   
    }
    public class GetChannelParameterBrandId
    {
        public ChannelProfileDto ChannelProfileDto { get; set; }

    }
    public class GetChannelByBrandIdQueryHandler : IRequestHandler<GetChannelProfileByBrandIdQuery, List<ChannelProfileDto>>
    {
        private readonly IChannelProfileRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelByBrandIdQueryHandler(IChannelProfileRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ChannelProfileDto>> Handle(GetChannelProfileByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAllByBrandIdAsync(request.BrandId);

            return await Task.FromResult(_mapper.Map<List<ChannelProfileDto>>(entity));
        }
    }
}
