using SNM.Twitter.Application.DTO;
using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Application.Interfaces
{
    public interface ITwitterAPIRepository<Guid>
    {
        //Task<HttpResponseMessage> Finalize(TwitterPostDto post);
        Task<string> GetUserBanner(string oauth_token, string oauth_token_secret, string screen_name);
        Task<TwitterPost> PublishToTwitterv2(Post post);
        Task<Tuple<int, string>> Retweet(string tweetId, string accessToken);
        Task<string> PublishToTwitter(TwitterPostDto post, string oauth_token, string oauth_token_secret);


    }
}
