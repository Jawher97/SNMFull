using AutoMapper;
using MediatR;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Exceptions.Model;
using SNM.Instagram.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Queries.InstagramChannels
{
    public class GetInstagramChannelByChannelIdQuery: IRequest<InstagramChannelDto>
    {
        public GetInstagramChannelByChannelIdQuery(Guid channelId)
        {
            ChannelId = channelId;
        }

        public Guid ChannelId { get; set; }    
    }
  
    public class GetInstagramChannelByChannelIdQueryHandler : IRequestHandler<GetInstagramChannelByChannelIdQuery, InstagramChannelDto>
    {
        private readonly IInstagramChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetInstagramChannelByChannelIdQueryHandler(IInstagramChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<InstagramChannelDto> Handle(GetInstagramChannelByChannelIdQuery request, CancellationToken cancellationToken)
        {
     
            var entity = await _repository.GetByChannelIdAsync(request.ChannelId);


            return await Task.FromResult(_mapper.Map<InstagramChannelDto>(entity));
        }
    }
}

