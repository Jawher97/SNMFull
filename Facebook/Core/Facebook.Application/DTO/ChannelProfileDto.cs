using System.Threading.Channels;

namespace SNS.Facebook.Application.DTO
{
    public class ChannelProfileDto : ModelBaseDto
    {
        public string UserName { get; set; }
        public string Headline { get; set; }
        public string CoverPhoto { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }

        public string ProfileUserId { get; set; }
        public string ProfileLink { get; set; }

        public string AccessToken { get; set; }
        public string expires_in { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiresIn { get; set; }
        public string Scope { get; set; }

        public virtual ICollection<ChannelDto> Channel { get; set; }
    }
}
