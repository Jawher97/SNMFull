using MediatR;
using SNM.BrandManagement.Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Exceptions.Model;
using SNM.BrandManagement.Domain.Enumeration;using Microsoft.Extensions.Configuration;

namespace SNM.BrandManagement.Application.Features.Commands.Posts
{
    public class CreateMediaCommand : IRequest<Response<List<MediaDto>>>
    {
        public List<MediaDto> MediaData { get; set; }
        public string ChannelTypeString { get; set; }
    }

    public class CreateMediaCommandHandler : IRequestHandler<CreateMediaCommand, Response<List<MediaDto>>>
    {
        private readonly IMediaRepository<Guid> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMediaCommandHandler> _logger;
        private readonly IConfiguration _configuration;

        public CreateMediaCommandHandler(IMediaRepository<Guid> repository,
            IMapper mapper, ILogger<CreateMediaCommandHandler> logger, IConfiguration configuration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public async Task<Response<List<MediaDto>>> Handle(CreateMediaCommand request, CancellationToken cancellationToken)
        {
            Response<List<MediaDto>> result = new Response<List<MediaDto>>();
            List<Media> mediaList = new List<Media>();
           var  ChannelTypelist= request.ChannelTypeString.Split(',');
            ChannelTypelist.Any(channelType => ChannelTypelist.Contains(channelType));
            
                
                   if(ChannelTypelist.Contains("Facebook Group") || ChannelTypelist.Contains("Facebook Page")|| ChannelTypelist.Contains("Instagram Profile"))
                    {


                                foreach (var item in request.MediaData)
                                {
                                    //item.Media_url = "https://consultim-it.com/wp-content/uploads/2022/03/pexels-andrea-piacquadio-927022-scaled.jpg";
                                    // item.Media_url = "https://localhost:44303/Resources/Images/Screenshot%202023-10-06%20113336.png";
                                    if(item.Media_type==MediaTypeEnum.IMAGE)
                                        {
                                            item.Media_url = ConvertImageBase64ToUrl(item);

                                        }
                                   
                                    var mediaEntity = _mapper.Map<Media>(item);
                                    mediaList.Add(mediaEntity);
                                }
                     }
                  
                   if(ChannelTypelist.Contains("LinkedIn Profile") || ChannelTypelist.Contains("LinkedIn Page")  || ChannelTypelist.Contains("Twitter Profile"))
                        foreach (var item in request.MediaData)
                        {
                            //item.Media_url = "https://consultim-it.com/wp-content/uploads/2022/03/pexels-andrea-piacquadio-927022-scaled.jpg";
                            // item.Media_url = "https://localhost:44303/Resources/Images/Screenshot%202023-10-06%20113336.png";

                           // item.Media_url = ConvertImageBase64ToUrl(item);
                            var mediaEntity = _mapper.Map<Media>(item);
                            mediaList.Add(mediaEntity);
                        }
                     
                
            
            var newEntities = await _repository.AddRangeAsync(mediaList);

            if (newEntities != null)
            {
                result.Succeeded = true;
                result.Data = _mapper.Map<List<MediaDto>>(newEntities);
                _logger.LogInformation($"Media List is successfully created.");
            }

            return result;
        }

        public string ConvertImageBase64ToUrl(MediaDto media)
        {
            var data = media.Media_url.Split(',')[1];
            string fileName = "";
            // Decode the base64 data
            byte[] mediaBytes = Convert.FromBase64String(data);

            // Define the path and file name for the saved image
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), _configuration["UploadsFiles"]);
            if (media.Media_type == MediaTypeEnum.IMAGE)
            { 
                fileName = Guid.NewGuid().ToString() + ".png"; 
            }
            else
            {
                fileName = Guid.NewGuid().ToString() + ".mp4";
            }
            string outputFilePath = Path.Combine(outputDirectory, fileName);

            // Ensure the output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Save the image to the specified path
            File.WriteAllBytes(outputFilePath, mediaBytes);

            // Generate a URL pointing to the saved image
            string baseUrl = _configuration["AppBaseUrl"];
            string imageRelativePath = Path.Combine(_configuration["UploadsFiles"], fileName);
            string imageUrl = baseUrl + "/" + imageRelativePath.Replace("\\", "/");
            return imageUrl;

        }


    }
}
