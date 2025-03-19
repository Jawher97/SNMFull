using AutoMapper;
using MediatR;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Queries.ChannelPosts
{
    public class GetChannelPostsQuery : IRequest<List<ChannelPostDto>>
    {
    }
    //public class GetChannelPostsViewModel
    //{
    //    public GetChannelPostsViewModel(Guid id, string displayName)
    //    {
    //        Id = id;
    //        DisplayName = displayName;
    //    }

    //    public Guid Id { get; set; }
    //    public string DisplayName { get; set; }
    //}
    public class GetChannelPostListQueryHandler : IRequestHandler<GetChannelPostsQuery, List<ChannelPostDto>>
    {
        private readonly IChannelPostRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetChannelPostListQueryHandler(IChannelPostRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ChannelPostDto>> Handle(GetChannelPostsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<ChannelPostDto>>(entities));
        }
    }
}
