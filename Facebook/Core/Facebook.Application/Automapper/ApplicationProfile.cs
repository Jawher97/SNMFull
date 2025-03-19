using AutoMapper;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Features.Commands.FacebookChannels;
using SNS.Facebook.Application.Features.Commands.Posts;
using SNS.Facebook.Application.Features.Queries.FacebookChannels;
using SNS.Facebook.Domain.Entities;

namespace SNS.Facebook.Application.Automapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
     
            /********** Facebook Channel ********/
            CreateMap<FacebookChannel, FacebookChannelDto>().ReverseMap();
            CreateMap<CreateFacebookChannelCommand, FacebookChannel>().ReverseMap();
            CreateMap<UpdateFacebookChannelCommand, FacebookChannel>().ReverseMap();
            CreateMap<GetFacebookChannelParameterId, FacebookChannel>().ReverseMap();
            CreateMap<GetFacebookChannelsViewModel, FacebookChannel>().ReverseMap();

            /********** Facebook Post ********/
            CreateMap<FacebookPostDto, FacebookPost>().ReverseMap();
            CreateMap<CreateFacebookPostCommand, FacebookPost>().ReverseMap();
            CreateMap<UpdateFacebookPostCommand, FacebookPost>().ReverseMap();

            /********** Brand ********/
            CreateMap<BrandDto, Brand>().ReverseMap();         
            /********** Channel ********/
            CreateMap<ChannelDto, Channel>().ReverseMap();   
            /********** ChannelType ********/
            CreateMap<ChannelTypeDto, ChannelType>().ReverseMap();
            /********** Media Data ********/
            CreateMap<MediaDto, Media>().ReverseMap();
            /********** Post ********/
            CreateMap<PostDto, Post>().ReverseMap();
        }
    }
}