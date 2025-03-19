using AutoMapper;
using MediatR;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Queries.InstagramChannels
{
    public class GetInstagramChannelsQuery : IRequest<List<InstagramChannelDto>>
    {
    }
    public class GetInstagramChannelsViewModel
    {
        public GetInstagramChannelsViewModel(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetInstagramChannelListQueryHandler : IRequestHandler<GetInstagramChannelsQuery, List<InstagramChannelDto>>
    {
        private readonly IInstagramChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetInstagramChannelListQueryHandler(IInstagramChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<InstagramChannelDto>> Handle(GetInstagramChannelsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<InstagramChannelDto>>(entities));
        }
    }
}
