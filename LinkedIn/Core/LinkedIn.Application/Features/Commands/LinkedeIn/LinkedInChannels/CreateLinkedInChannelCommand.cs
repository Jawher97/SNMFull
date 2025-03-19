using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Application.Features.Commands.LinkedeIn.LinkedInChannels
{
    public class CreateLinkedInChannelCommand : IRequest<Response<LinkedInChannelDto>>
    {
        public LinkedInChannelDto linkedInChannelDto { get; set; }

    }

  

    public class CreateLinkedInChannelCommandHandler : IRequestHandler<CreateLinkedInChannelCommand, Response<LinkedInChannelDto>>
    {
        private readonly ILinkedInChannelRepository<Guid> _repository;
        // private readonly ITwitterImagesRepository<Guid> _repositoryImage;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLinkedInChannelCommandHandler> _logger;

        public CreateLinkedInChannelCommandHandler(ILinkedInChannelRepository<Guid> repository, IMapper mapper, ILogger<CreateLinkedInChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //_repositoryImage = repositoryImage ?? throw new ArgumentNullException(nameof(repositoryImage));
        }

        public async Task<Response<LinkedInChannelDto>> Handle(CreateLinkedInChannelCommand request, CancellationToken cancellationToken)
        {
            Response<LinkedInChannelDto> result = new Response<LinkedInChannelDto>();
            // List<LinkedInImages> LinkedInImageList = new List<LinkedInImages>();

            var LinkedInPostEntity = _mapper.Map<LinkedInChannel>(request.linkedInChannelDto);

            var newEntity = await _repository.AddAsync(LinkedInPostEntity);


            if (newEntity != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<LinkedInChannelDto>(newEntity);
                _logger.LogInformation($"LinkedInChannelDto {newEntity.Id} is successfully created.");
            }
            return result;
        }
    }
}

