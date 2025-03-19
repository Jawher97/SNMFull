using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.Channels
{
    public class GetChannelsQuery : IRequest<List<GetChannelsViewModel>>
    {
    }
    public class GetChannelsViewModel
    {
        public GetChannelsViewModel(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetChannelListQueryHandler : IRequestHandler<GetChannelsQuery, List<GetChannelsViewModel>>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelListQueryHandler(IChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GetChannelsViewModel>> Handle(GetChannelsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<GetChannelsViewModel>>(entities));
        }
    }
}
