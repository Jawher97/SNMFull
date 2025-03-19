using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.Features.Commands.Twitter.Posts
{
    public class UpdatePostTwitterCommand : IRequest<Response<TwitterPostDto>> { 
    public TwitterPostDto TwitterPostDto { get; set; }
}
public class UpdatePostTwitterCommandValidator : AbstractValidator<UpdatePostTwitterCommand>
{
    public UpdatePostTwitterCommandValidator()
    {
        RuleFor(p => p.TwitterPostDto.Id)
            .NotEmpty().WithMessage("{Id} is required.")
            .NotNull();

    }
}
public class UpdateTwitterCommandHandler : IRequestHandler<UpdatePostTwitterCommand, Response<TwitterPostDto>>
{
    private readonly ITwitterPostRepository<Guid> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdatePostTwitterCommandValidator> _logger;

    public UpdateTwitterCommandHandler(ITwitterPostRepository<Guid> repository, IMapper mapper, ILogger<UpdatePostTwitterCommandValidator> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Response<TwitterPostDto>> Handle(UpdatePostTwitterCommand request, CancellationToken cancellationToken)
    {
        Response<TwitterPostDto> result = new Response<TwitterPostDto>();

        var twitterToUpdate = await _repository.GetByIdAsync(request.TwitterPostDto.Id);
        if (twitterToUpdate == null)
        {
            throw new NotFoundException(nameof(TwitterPostDto), request.TwitterPostDto.Id);
        }

        _mapper.Map(request.TwitterPostDto, twitterToUpdate);
        
        await _repository.UpdateAsync(twitterToUpdate);

        result.Succeeded = true;
        result.Data = request.TwitterPostDto;
        _logger.LogInformation($"TwitterPost {request.TwitterPostDto} is successfully updated.");

        return result;
    }
}
    
}
