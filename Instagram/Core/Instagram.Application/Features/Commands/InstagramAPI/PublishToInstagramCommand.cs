//using AutoMapper;
//using FluentValidation;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using SNM.Instagram.Application.DTO;
//using SNM.Instagram.Application.Interfaces;


//namespace SNM.Instagram.Application.Features.Commands.InstagramAPI
//{

//    public class PublishToInstagramCommand : IRequest<string>
//    {

//        public InstagramChannelPostDto InstagramChannelPostDto { get; set; }
//    }
//    public class CreatePostCommandValidator : AbstractValidator<PublishToInstagramCommand>
//    {
//        public CreatePostCommandValidator()
//        {
//            RuleFor(p => p.InstagramChannelPostDto)
//                .NotEmpty().WithMessage("{InstagramChannelPostDto} is required.")
//                .NotNull();
//            // .MaximumLength(250).WithMessage("{PostText} must not exceed 250 characters.");
//        }
//    }
//    public class PublishToInstagramCommandHandler : IRequestHandler<PublishToInstagramCommand, string>
//    {
//        private readonly IInstagramAPIRepository<Guid> _repository;
//        private readonly IMapper _mapper;
//        private readonly ILogger<PublishToInstagramCommandHandler> _logger;

//        public PublishToInstagramCommandHandler(IInstagramAPIRepository<Guid> repository, IMapper mapper, ILogger<PublishToInstagramCommandHandler> logger)
//        {
//            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
//            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        public async Task<string> Handle(PublishToInstagramCommand request, CancellationToken cancellationToken)
//        {
//            //var instagramPost = _mapper.Map<ChannelPostDto>(request);
//      //      request.InstagramChannelPostDto.InstagramChannelDto.PostToPagePhotosURL = $"{request.InstagramChannelPostDto.InstagramChannelDto.ChannelAPI}{request.InstagramChannelPostDto.InstagramChannelDto.UserId}/{request.InstagramChannelPostDto.InstagramChannelDto.PageEdgePhotos}";

//            var newEntity = await _repository.PublishPost(request.InstagramChannelPostDto);

//            _logger.LogInformation($"Post {newEntity} is successfully created.");

//            return newEntity.ToString();
//        }


//    }
//}