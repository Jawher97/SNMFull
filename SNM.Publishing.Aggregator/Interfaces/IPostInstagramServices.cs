using Microsoft.AspNetCore.Mvc;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Models;

namespace SNM.Publishing.Aggregator.Interfaces
{
    public interface IPostInstagramServices
    {
        Task<Response<PostDetalisDto>> GeInstagramChannels(ChannelDto channel);
        Task<Response<InstagramPostDto>> PublishInstagramPost(InstagramPostDto instagramPost,Guid channelId);
        Task<Response<string>> PublishSchedulePostInstagram(InstagramPostDto instagramPost,Guid channelId);
        Task<Response<CommentDto>> CreateComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<CommentDto>> CreateSubComment(CommentDetailsDto comment, Guid channelId);
    }
}
