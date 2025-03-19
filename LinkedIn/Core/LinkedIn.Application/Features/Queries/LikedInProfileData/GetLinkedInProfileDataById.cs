using AutoMapper;
using MediatR;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Queries.LikedInProfileData
{
    public class GetLinkedInProfileDataById : IRequest<LinkedInProfileDataDto>
    {
        public GetLinkedInProfileDataById(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
      
    }

    public class GetLinkedInProfileParameterId
    {
        public string Id { get; set; }
       
    }

    public class GetLikedinProfileByIdQueryHandler : IRequestHandler<GetLinkedInProfileDataById, LinkedInProfileDataDto>
    {
        private readonly ILinkedInProfileRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetLikedinProfileByIdQueryHandler(ILinkedInProfileRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<LinkedInProfileDataDto> Handle(GetLinkedInProfileDataById request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            return await Task.FromResult(_mapper.Map<LinkedInProfileDataDto>(entity));
        }
    }
}

