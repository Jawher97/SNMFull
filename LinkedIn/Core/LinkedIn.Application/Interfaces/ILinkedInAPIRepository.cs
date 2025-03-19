
using Microsoft.AspNetCore.Mvc;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Domain.Entities;

namespace SNM.LinkedIn.Application.Interfaces
{
    public interface ILinkedInAPIRepository<Guid>
    {

        Task<Response<ChannelProfile>> LinkedINAuth(string code);
        Task<string> refreshAccessToken(string refreshToken);
        Task<Response<ChannelProfile>> GetProfileID(string accessToken);
        Task<Response<ChannelProfile>> GetMemberProfile(string accesstoken, string person_id);
        Task<Response<ChannelProfile>> GetLinkedInProfilePicture(string accessToken, string userUrn);
        Task<MemberProfile> GetCommentersProfile(string accesstoken, string entityUrn);
        Task<Response<Channel>> GetCompanyProfile(string accesstoken, string org_id);
        Task<Response<Dictionary<string, object>>> GetOrgDetails(string accessToken,Guid brandId);
        Task<Response<string>> PublishToLinkedIn(LinkedInPostDto post, LinkedInChannelDto linkedInProfileDto);
        Task<LinkedInPost> GetLastsLinkedInPost(Post post);
        Task<Response<string>> CreateArticle(string accessToken, string author_urn, LinkedInArticleDto article);
        Task<Response<string>> CreatePoll(string accessToken, string orgId, string commentary, string question);
        Task<Response<string>> CreateReaction(PostDetalisDto post, string accessToken);
        Task<Response<string>> DeleteReaction(PostDetalisDto post, string accessToken);
        Task<string> EditPost(string accessToken, string commentary, string shareUrn);
        Task<Response<string>> ResharePost(string accessToken, string author_urn, string shareId, string commentary);
        Task<Response<CommentDto>> CreateComment(CommentDetailsDto comment,string accessToken);
        Task<Response<CommentDto>> CreateSubComment(CommentDetailsDto comment, string accessToken);
        Task<Response<CommentDto>> EditComment(CommentDetailsDto comment, string accessToken);
        Task<Response<string>> DeleteComment(CommentDetailsDto comment,string accessToken);
        Task<Response<string>> DeleteReactionComment(CommentDetailsDto comment, string accessToken);
        Task<string> DisableCommentsOnCreatedPost(string accessToken, string shareId, string orgId);
        Task<Response<string>> DeletePostAsync(PostDetalisDto post, string accessToken);

        Task<Response<string>> CreateReactionComment(CommentDetailsDto comment, string accessToken);

    }
}
