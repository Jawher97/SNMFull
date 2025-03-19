using Microsoft.AspNetCore.Mvc;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Models;

namespace SNM.Publishing.Aggregator.Interfaces
{
    public interface IPostTwitterServices
    {
        Task<Response<PostDto>> GeTwitterChannels(ChannelDto channel);
        Task<Response<TwitterPostDto>> PublishTwitterPost(TwitterPostDto twitterPost, Guid channelId);
        Task<Response<string>> PublishSchedulePostTwitter(TwitterPostDto twitterPost, Guid channelId);
        
    }
}
