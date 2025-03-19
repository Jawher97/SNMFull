using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.Channels
{
    public class GetChannelByChannelIdQuery : IRequest<List<ChannelDto>>
    {
        public GetChannelByChannelIdQuery(Guid id, Guid brandId)
        {
            ChannelTypeId = id;

            BrandId= brandId;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid ChannelTypeId { get; set; }
        public Guid BrandId { get; set; }
    }
    public class GetChannelParameterByChannelIdQuery
    {
        public ChannelDto Channel { get; set; }

    }
    public class GetChannelByChannelIdHandler : IRequestHandler<GetChannelByChannelIdQuery, List<ChannelDto>>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelByChannelIdHandler(IChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ChannelDto>> Handle(GetChannelByChannelIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetAllByChannelTypeIdIdAsync(request.ChannelTypeId, request.BrandId);

            return await Task.FromResult(_mapper.Map<List<ChannelDto>>(entity));
        }
    }
}
