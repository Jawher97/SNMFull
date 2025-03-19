using AutoMapper;
using SNM.LinkedIn.Application.DTO;

using SNM.LinkedIn.Domain.Entities;



using SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts;

namespace SNM.LinkedIn.Application.Automapper
{
    public class LinkedInProfile : Profile
    {
        public LinkedInProfile()
        {
          
          


          
          
            /********** Channel ********/
            CreateMap<Channel, ChannelDto>().ReverseMap();
          
          
            /********** ChannelPost ********/
          
            CreateMap<LinkedInChannel, LinkedInChannelDto>().ReverseMap();
            /********** LinkedProfileData ********/
            CreateMap<LinkedInProfileData, LinkedInProfileDataDto>().ReverseMap();
            /********** LinkedInPost ********/
            CreateMap<LinkedInPost, LinkedInPostDto>().ReverseMap();
            CreateMap<LinkedInInsight, LinkedInInsightDto>().ReverseMap();
            CreateMap<CreatePostLinkedinCommand, LinkedInPost>().ReverseMap();
            CreateMap<MediaLinkedin, MediaLinkedinDto>().ReverseMap();
            /********** ChannelProfile ********/
            CreateMap<ChannelProfile, ChannelProfileDto>().ReverseMap();
        }
    }
}