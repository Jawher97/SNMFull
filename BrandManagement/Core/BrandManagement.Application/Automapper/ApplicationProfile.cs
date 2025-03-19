using AutoMapper;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Features.Commands.Brands.CreateBrand;
using SNM.BrandManagement.Application.Features.Commands.Brands.UpdateBrand;
using SNM.BrandManagement.Application.Features.Commands.Channels;
using SNM.BrandManagement.Application.Features.Commands.Posts;
using SNM.BrandManagement.Application.Features.Queries.Brand;
using SNM.BrandManagement.Application.Features.Queries.Brand.GetBrands;
using SNM.BrandManagement.Application.Features.Queries.Channels;
using SNM.BrandManagement.Application.Features.Queries.Posts;
using SNM.BrandManagement.Domain.Entities;

namespace SNM.BrandManagement.Application.Automapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            
            /********** Brand ********/
            CreateMap<BrandDto, Brand>().ReverseMap();           
            CreateMap<CreateBrandCommand, Brand>().ReverseMap();
            CreateMap<UpdateBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandParameterId, Brand>().ReverseMap();
            CreateMap<GetBrandsViewModel, Brand>().ReverseMap();
            /********** Channel ********/
            CreateMap<ChannelDto, Channel>().ReverseMap();
            CreateMap<CreateChannelCommand, Channel>().ReverseMap();
            CreateMap<UpdateChannelCommand, Channel>().ReverseMap();
            CreateMap<GetChannelParameterId, Channel>().ReverseMap();
            CreateMap<GetChannelParameterBrandId, Channel>().ReverseMap();
            CreateMap<GetChannelsViewModel, Channel>().ReverseMap();
            /********** ChannelType ********/
            CreateMap<ChannelTypeDto, ChannelType>().ReverseMap();
            /********** Media Data ********/
            CreateMap<MediaDto, Media>().ReverseMap();
            /********** Post ********/
            CreateMap<PostDto, Post>().ReverseMap();
            CreateMap<CreatePostCommand, Post>().ReverseMap();
            CreateMap<UpdatePostCommand, Post>().ReverseMap();
            CreateMap<GetPostParameterId, Post>().ReverseMap();
            CreateMap<GetPostsViewModel, Post>().ReverseMap();

            /********** Channel ********/
            CreateMap<ChannelProfileDto, ChannelProfile>().ReverseMap();

        }
    }
}