using SNS.Facebook.Application.DTO;


namespace SNS.Facebook.Application.Interfaces
{
    public interface IFacebookAPIRepository
    {
        Task <Tuple<int, string>> PublishPostToFacebook(FacebookPostDto facebookChannelPostDto);
        Task<Tuple<int, string>> UploadPhoto(FacebookChannelDto facebookChannelDto, string photoURL);
        Task<Tuple<int, string>> UpdatePhotoWithPost(FacebookChannelDto facebookChannelDto, string postID, string postText);
    }
}
