using AutoMapper;
using MediatR;
using SNM.Instagram.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Queries.ChannelPosts
{
    public class GetChannelPostByIdQuery : IRequest<GetChannelPostParameterId>
    {
        public GetChannelPostByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetChannelPostParameterId
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class GetChannelPostByIdQueryHandler : IRequestHandler<GetChannelPostByIdQuery, GetChannelPostParameterId>
    {
        private readonly IChannelPostRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelPostByIdQueryHandler(IChannelPostRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetChannelPostParameterId> Handle(GetChannelPostByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<GetChannelPostParameterId>(entity));
        }
    }
}
