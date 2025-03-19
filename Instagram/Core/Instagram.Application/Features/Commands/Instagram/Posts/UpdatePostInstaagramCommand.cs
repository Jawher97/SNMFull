using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Exceptions;
using SNM.Instagram.Application.Exceptions.Model;
using SNM.Instagram.Application.Features.Commands.InstagramChannels;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Commands.Instagram.Posts
{
    public class UpdatePostInstaagramCommand:IRequest<Response<InstagramPostDto>>
    {
        public InstagramPostDto InstagramPostDto { get; set; }
    }
    public class UpdatePostInstaagramCommandValidator : AbstractValidator<UpdatePostInstaagramCommand>
    {
        public UpdatePostInstaagramCommandValidator()
        {
            RuleFor(p => p.InstagramPostDto.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .NotNull();

        }
    }
    public class UpdateInstagramCommandHandler : IRequestHandler<UpdatePostInstaagramCommand, Response<InstagramPostDto>>
    {
        private readonly IInstagramPostRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInstagramChannelCommandHandler> _logger;

        public UpdateInstagramCommandHandler(IInstagramPostRepository<Guid> repository, IMapper mapper, ILogger<UpdateInstagramChannelCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response<InstagramPostDto>> Handle(UpdatePostInstaagramCommand request, CancellationToken cancellationToken)
        {
            Response<InstagramPostDto> result = new Response<InstagramPostDto>();

            var instagramToUpdate = await _repository.GetByIdAsync(request.InstagramPostDto.Id);
            if (instagramToUpdate == null)
            {
                throw new NotFoundException(nameof(InstagramPostDto), request.InstagramPostDto.Id);
            }

            _mapper.Map( request.InstagramPostDto, instagramToUpdate);
            //instagramToUpdate.InstagramPostNetwokId = request.InstagramPostDto.InstagramPostNetwokId;
            await _repository.UpdateAsync(instagramToUpdate);

            result.Succeeded = true;
            result.Data = request.InstagramPostDto;
            _logger.LogInformation($"InstagramPost {request.InstagramPostDto} is successfully updated.");

            return result;
        }
    }

}
