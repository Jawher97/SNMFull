using AutoMapper;

using SNM.Twitter.Application.DTO;

using SNM.Twitter.Application.Features.Commands.Twitter.Posts;
using SNM.Twitter.Application.Features.Commands.TwitterChannels;
using SNM.Twitter.Application.Features.Commands.Updatetwitter;

using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Automapper
{
    public class twitterProfile : Profile
    {
        public twitterProfile()
        {
            /****TwitterChannel*****/
            CreateMap<TwitterChannel, TwitterChannelDto>().ReverseMap();
            CreateMap<CreateTwitterChannelCommand, TwitterChannel>().ReverseMap();
            /****TwitterPost*****/
            CreateMap<TwitterPost, TwitterPostDto>().ReverseMap();
            CreateMap<TwitterImages, TwitterImagesDto>().ReverseMap();
            CreateMap < CreatePostTwitterCommand, TwitterPost > ().ReverseMap();




            /********** Brand ********/
            CreateMap<BrandDto, Brand>().ReverseMap();
       
            /********** Channel ********/
            CreateMap<Channel, ChannelDto>().ReverseMap();
          
           
            /********** ChannelPost ********/
            CreateMap<ChannelPost, ChannelPostDto>().ReverseMap();
        
            CreateMap<TwitterProfileData, TwitterProfileDataDto>().ReverseMap();
           


        }
    }
}