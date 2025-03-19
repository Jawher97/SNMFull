using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.Posts
{
    public class GetPostsQuery : IRequest<List<GetPostsViewModel>>
    {
    }
    public class GetPostsViewModel
    {
        public GetPostsViewModel(Guid id, string message)
        {
            Id = id;
            Message = message;
        }

        public Guid Id { get; set; }
        public string Message { get; set; }
    }
    public class GetPostListQueryHandler : IRequestHandler<GetPostsQuery, List<GetPostsViewModel>>
    {
        private readonly IPostRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetPostListQueryHandler(IPostRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GetPostsViewModel>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return await Task.FromResult(_mapper.Map<List<GetPostsViewModel>>(entities));
        }
    }
}
