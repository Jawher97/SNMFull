using AutoMapper;
using MediatR;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Queries.LinkedInChannels
{
    public class GetLinkedinChannelbyChannelId :IRequest<LinkedInChannel>
    { 
        public GetLinkedinChannelbyChannelId(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
    }

public class GetLinkedinChannelQueryHandler : IRequestHandler<GetLinkedinChannelbyChannelId, LinkedInChannel>
{
        private readonly ILinkedInChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

    public GetLinkedinChannelQueryHandler(ILinkedInChannelRepository<Guid> repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<LinkedInChannel> Handle(GetLinkedinChannelbyChannelId request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetLinkedinChannelbyChannelId(request.Id);

        return await Task.FromResult(_mapper.Map<LinkedInChannel>(entity));
    }
}
}
