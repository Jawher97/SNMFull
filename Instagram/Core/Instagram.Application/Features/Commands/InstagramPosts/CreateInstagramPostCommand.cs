using MediatR;

namespace SNM.Instagram.Application.Features.Commands.InstagramPosts
{
    public class CreateInstagramPostCommand : IRequest<InstagramPostDto>
    {
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Hashtags { get; set; }
    }
}