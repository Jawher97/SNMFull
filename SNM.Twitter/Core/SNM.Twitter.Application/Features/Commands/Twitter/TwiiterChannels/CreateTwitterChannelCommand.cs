using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.Twitter.TwiiterChannels
{
    public class CreateTwitterChannelCommand : IRequest<Response<TwitterChannelDto>>
    {
        public TwitterChannelDto twitterChannelDto { get; set; }

    }



    public class CreateTwitterChannelCommandHandler : IRequestHandler<CreateTwitterChannelCommand, Response<TwitterChannelDto>>
    {
        private readonly ITwitterChannelRepository<Guid> _repository;
        
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTwitterChannelCommandHandler> _logger;

        public CreateTwitterChannelCommandHandler(ITwitterChannelRepository<Guid> repository, IMapper mapper, ILogger<CreateTwitterChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
        }

        public async Task<Response<TwitterChannelDto>> Handle(CreateTwitterChannelCommand request, CancellationToken cancellationToken)
        {
            Response<TwitterChannelDto> result = new Response<TwitterChannelDto>();
           

            var LinkedInPostEntity = _mapper.Map<TwitterChannel>(request.twitterChannelDto);

            var newEntity = await _repository.AddAsync(LinkedInPostEntity);


            if (newEntity != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<TwitterChannelDto>(newEntity);
                _logger.LogInformation($"TwitterChannelDto {newEntity.Id} is successfully created.");
            }
            return result;
        }
    }
}
