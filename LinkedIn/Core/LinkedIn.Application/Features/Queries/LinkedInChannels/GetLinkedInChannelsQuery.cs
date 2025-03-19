using AutoMapper;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Interfaces;
using MediatR;

namespace SNM.LinkedIn.Application.Features.Queries.LinkedInChannels
{
    public class GetLinkedInChannelByIdQuery : IRequest<LinkedInChannelDto>
    {
        public GetLinkedInChannelByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        
    }

    public class GetLinkedInChannelParameterId
    {
        public Guid Id { get; set; }
       
    }

    public class GetLinkedinChannelByIdQueryHandler : IRequestHandler<GetLinkedInChannelByIdQuery, LinkedInChannelDto>
    {
        private readonly ILinkedInChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetLinkedinChannelByIdQueryHandler(ILinkedInChannelRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<LinkedInChannelDto> Handle(GetLinkedInChannelByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByChannelIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<LinkedInChannelDto>(entity));
        }
    }
}
