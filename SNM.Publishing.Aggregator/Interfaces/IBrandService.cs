using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Models;

namespace SNM.Publishing.Aggregator.Interfaces
{
    public interface IBrandService
    {
        Task<Response<PostDto>> CreatePost(PostDto post, ICollection<ChannelDto> SocialChannels);                
    }
}
