using Microsoft.AspNetCore.Mvc;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Models;

namespace SNM.Publishing.Aggregator.Interfaces
{
    public interface IPostLinkedInService
    {
        Task<Response<LinkedInPostDto>> PublishPostLinkedIn(LinkedInPostDto LinkedInPost ,Guid ChannelId);
        Task<Response<string>> PublishSchedulePostLinkedIn(LinkedInPostDto LinkedInPost,  Guid ChannelId);

        Task<Response<PostDetalisDto>> GeLinkedinChannels(ChannelDto channel);
        Task<Response<CommentDto>> CreateComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> DeleteComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<CommentDto>> UpdateComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<CommentDto>>  CreateSubComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> CreateRectionComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> DeleteReactionComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> CreateRectionPost(PostDetalisDto comment, Guid channelId);
        Task<Response<string>> DeleteReactionPost(PostDetalisDto comment, Guid channelId);
        Task<Response<string>> DeletePost(PostDetalisDto comment, Guid channelId);

    }
}
