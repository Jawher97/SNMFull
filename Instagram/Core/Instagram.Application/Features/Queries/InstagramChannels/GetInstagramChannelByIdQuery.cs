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
    public class GetInstagramChannelByIdQuery : IRequest<InstagramChannelDto>
    {
        public GetInstagramChannelByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetInstagramChannelParameterId
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetInstagramChannelByIdQueryHandler : IRequestHandler<GetInstagramChannelByIdQuery, InstagramChannelDto>
    {
        private readonly IInstagramChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetInstagramChannelByIdQueryHandler(IInstagramChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<InstagramChannelDto> Handle(GetInstagramChannelByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByChannelIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<InstagramChannelDto>(entity));
        }
    }
}