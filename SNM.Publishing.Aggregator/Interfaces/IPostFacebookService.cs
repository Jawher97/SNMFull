using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Models;

namespace SNM.Publishing.Aggregator.Interfaces
{
    public interface IPostFacebookService
    {
        Task<Response<PostDetalisDto>> GeFacebookChannels(ChannelDto channel);
        Task<Response<FacebookPostDto>> PublishFacebookPost(FacebookPostDto facebookPost, Guid channelId);
        Task<Response<string>> PublishScheduleFacebookPost(FacebookPostDto facebooPost, Guid channelId);
        Task<Response<CommentDto>> CreateComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> DeleteComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<CommentDto>> CreateSubComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<CommentDto>> UpdateComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> CreateRectionComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> DeleteReactionComment(CommentDetailsDto comment, Guid channelId);
        Task<Response<string>> CreateRectionPost(PostDetalisDto comment, Guid channelId);
        Task<Response<string>> DeleteReactionPost(PostDetalisDto comment, Guid channelId);
        Task<Response<string>> DeletePost(PostDetalisDto post, Guid channelId);
    }
}
