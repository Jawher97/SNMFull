using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.Channels
{
    public class GetChannelByChannelProfileId : IRequest<List<Channel>>

    {
        public GetChannelByChannelProfileId(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
       
    }
    public class GetChannelByChannelProfileIdHandler : IRequestHandler<GetChannelByChannelProfileId, List<Channel>>
    {
        private readonly IChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelByChannelProfileIdHandler(IChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<Channel>> Handle(GetChannelByChannelProfileId request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetChannelByChannelProfileAsync(request.Id);
            return entity;
        }
    }
}
