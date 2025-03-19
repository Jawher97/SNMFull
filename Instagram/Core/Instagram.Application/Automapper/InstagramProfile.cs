using AutoMapper;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Features.Commands.Brands.CreateBrand;
using SNM.Instagram.Application.Features.Commands.Brands.UpdateBrand;
using SNM.Instagram.Application.Features.Commands.Channels;
using SNM.Instagram.Application.Features.Commands.CreateInstagram;
using SNM.Instagram.Application.Features.Commands.Instagram.Posts;
using SNM.Instagram.Application.Features.Commands.InstagramChannels;
using SNM.Instagram.Application.Features.Commands.InstagramPosts;
using SNM.Instagram.Application.Features.Commands.Posts;
using SNM.Instagram.Application.Features.Commands.UpdateInstagram;
using SNM.Instagram.Application.Features.Queries.Brand.GetBrands;
using SNM.Instagram.Application.Features.Queries.Channels;
using SNM.Instagram.Application.Features.Queries.GetEntities;
using SNM.Instagram.Application.Features.Queries.GetEntityById;
using SNM.Instagram.Application.Features.Queries.Insights;
using SNM.Instagram.Application.Features.Queries.InstagramChannels;
using SNM.Instagram.Application.Features.Queries.InstagramPost;
using SNM.Instagram.Application.Features.Queries.Posts;
using SNM.Instagram.Domain.Common;
using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Automapper
{
    public class InstagramProfile : Profile
    {
        public InstagramProfile()
        {

            // Instagram Channel
            CreateMap<InstagramChannel, InstagramChannelDto>().ReverseMap();
            CreateMap<CreateInstagramChannelCommand, InstagramChannel>().ReverseMap();
            CreateMap<UpdateInstagramChannelCommand, InstagramChannel>().ReverseMap();
            //Instagram Post 

            CreateMap<UpdatePostInstaagramCommand, InstagramPost>().ReverseMap();
            CreateMap<InstagramPost, InstagramPostDto>().ReverseMap();
            





            CreateMap<CreateInstagramCommand, Entity>().ReverseMap();
            CreateMap<UpdateInstagramCommand, Entity>().ReverseMap();
            CreateMap<GetParameterId, Entity>().ReverseMap();
            CreateMap<GetEntitiesViewModel, Entity>().ReverseMap();
            //Brand
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<CreateBrandCommand, Brand>().ReverseMap();
            CreateMap<UpdateBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandsQuery, Brand>().ReverseMap();
            CreateMap<GetBrandsViewModel, Brand>().ReverseMap();
            CreateMap<BrandDto, Brand>().ReverseMap();
            /********** Channel ********/
            CreateMap<Channel, ChannelDto>().ReverseMap();
            CreateMap<CreateChannelCommand, Channel>().ReverseMap();
            CreateMap<UpdateChannelCommand, Channel>().ReverseMap();
            CreateMap<GetChannelParameterId, Channel>().ReverseMap();
            CreateMap<GetChannelParameterBrandId, Channel>().ReverseMap();
            CreateMap<GetChannelsViewModel, Channel>().ReverseMap();
            /********** Post ********/
            CreateMap <Post, PostDto>().ReverseMap();
     //      CreateMap<CreateInstagramPostCommand, InstagramPost>().ReverseMap();
            CreateMap<UpdatePostCommand, Post>().ReverseMap();
            CreateMap<GetPostParameterId, Post>().ReverseMap();
            CreateMap<GetPostsViewModel, Post>().ReverseMap();
            CreateMap<CreatePostCommand, Post>().ReverseMap();
            /********** ChannelPost ********/
            CreateMap<ChannelPost, ChannelPostDto>().ReverseMap();
          
            CreateMap<GetBrandsViewModel, Brand>().ReverseMap();
            /********** InstagramPost ********/
          //  CreateMap<InstagramPost, InstagramPostDto>().ReverseMap();
          
            CreateMap<InstagramPostDto, InstagramPost>().ReverseMap();
            CreateMap<GetInstagramPostViewModel, InstagramPost>().ReverseMap();
            CreateMap<GetInstagramPostQuery, InstagramPost>().ReverseMap();
            CreateMap<GetPostsViewModel, InstagramPost>().ReverseMap();
        
            /********** InstagramChannelPost ********/
            CreateMap<InstagramChannelPost, InstagramChannelPostDto>().ReverseMap();
         //   CreateMap<InstagramChannel, InstagramChannelDto>().ReverseMap();
            CreateMap<GetInstagramChannelsQuery, InstagramChannelPost>().ReverseMap();
            CreateMap<InstagramProfileData, InstagramProfileDataDto>().ReverseMap();
            /********** Insight ********/
            CreateMap<GetInsightsQuery,Insight>().ReverseMap();
            CreateMap<Insight, InsightDto>().ReverseMap();

        }
    }
    }
