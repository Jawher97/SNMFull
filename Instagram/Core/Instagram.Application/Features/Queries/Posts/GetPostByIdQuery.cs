using AutoMapper;
using MediatR;
using SNM.Instagram.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Queries.Posts
{
    public class GetPostByIdQuery : IRequest<GetPostParameterId>
    {
        public GetPostByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string Message { get; set; }
    }
    public class GetPostParameterId
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, GetPostParameterId>
    {
        private readonly IPostRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetPostByIdQueryHandler(IPostRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetPostParameterId> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<GetPostParameterId>(entity));
        }
    }
}

