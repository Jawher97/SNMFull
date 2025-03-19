using MediatR;
using FluentValidation;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Exceptions.Model;

namespace SNM.Instagram.Application.Features.Commands.InstagramChannels
{
    public class UpdateInstagramChannelCommand : IRequest<Response<InstagramChannelDto>>
    {
         public InstagramChannelDto InstagramChannelDto { get; set; }
        //public Guid Id { get; set; }
        //public string DisplayName { get; set; }
        //public string Description { get; set; }
        //public string Photo { get; set; }
        //public string CoverPhoto { get; set; }
    }
    public class UpdateInstagramChannelCommandValidator : AbstractValidator<UpdateInstagramChannelCommand>
    {
        public UpdateInstagramChannelCommandValidator()
        {
            RuleFor(p => p.InstagramChannelDto.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .NotNull();
             
        }
    }
    public class UpdateInstagramChannelCommandHandler : IRequestHandler<UpdateInstagramChannelCommand, Response<InstagramChannelDto>>
    {
        private readonly IInstagramChannelRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInstagramChannelCommandHandler> _logger;

        public UpdateInstagramChannelCommandHandler(IInstagramChannelRepository<Guid> repository, IMapper mapper, ILogger<UpdateInstagramChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<InstagramChannelDto>> Handle(UpdateInstagramChannelCommand request, CancellationToken cancellationToken)
        {
            Response<InstagramChannelDto> result = new Response<InstagramChannelDto>();

            var instagramToUpdate = await _repository.GetByIdAsync(request.InstagramChannelDto.Id);
            if (instagramToUpdate == null)
            {
                throw new NotFoundException(nameof(InstagramChannel), request.InstagramChannelDto.Id);
            }
      
            _mapper.Map(  request.InstagramChannelDto, instagramToUpdate);
         //   instagramToUpdate.InstagramPostNetwokId = request.InstagramChannelDto.InstagramPostNetwokId;
            await _repository.UpdateAsync(instagramToUpdate);

            result.Succeeded = true;
            result.Data = request.InstagramChannelDto;
            _logger.LogInformation($"InstagramChannel {request.InstagramChannelDto} is successfully updated.");

            return result;
        }
    }
}