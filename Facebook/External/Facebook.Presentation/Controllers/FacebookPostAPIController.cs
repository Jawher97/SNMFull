using System.IO;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.BrandManagement.Application.DTO;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Exceptions.Model;
using SNS.Facebook.Application.Features.Commands.FacebookAPI;
using SNS.Facebook.Application.Features.Commands.Posts;
using SNS.Facebook.Application.Features.Queries.FacebookChannels;
using SNS.Facebook.Domain.Entities;
using SNS.Facebook.Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;

namespace SNS.Facebook.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class FacebookPostAPIController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public FacebookPostAPIController(IMediator mediator , IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException();
       
        }
        [HttpGet("GetfacebookChannel")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<FacebookChannelDto> GetFacebookChannelbyChannelId(Guid channelId)
        {

            var getEntities = new GetFacebookChannelByChannelIdQuery(channelId);
            var entities = await _mediator.Send(getEntities);

            return entities.Data;
        }
        [HttpGet("GetFacebookSummary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<object>> GetFacebookSummary([FromQuery] Guid channelId)
        {
            Response<object> result = new Response<object>();  
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            string url = "https://graph.facebook.com/v18.0/" + facebookchannel.SocialChannelNetwokId + "?fields=followers_count,talking_about_count,about,country_page_likes,new_like_count,rating_count,posts,unread_message_count,unread_notif_count,ratings" + "&access_token=" + facebookchannel.ChannelAccessToken;
            //,messenger_lead_forms
            var http = new HttpClient();
            var httpResponse = await http.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var facebook = JsonConvert.DeserializeObject<dynamic>(httpContent);
                var summary = new
                {
                    FollowersCount= facebook.followers_count,
                    About = facebook.about,
                    TalkingCount = facebook.talking_about_count,
                    RatingCount=facebook.rating_count,
                    PostsCount= facebook.posts.data.Count,
                    UnreadMessageCount= facebook.unread_message_count,
                    UnreadNotifCount = facebook.unread_notif_count,
                };
                result.Data=summary;
                result.Succeeded = true;
                return result;
            }
            result.Message = "Failed To get summary with status Code"+ httpResponse.StatusCode;

            return result;
        }
        [HttpGet("GetFacebookGroupSummary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<object>> GetFacebookGroupSummary([FromQuery] Guid channelId)
        {
            Response<object> result = new Response<object>();
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            string url = "https://graph.facebook.com/v18.0/" + facebookchannel.SocialChannelNetwokId + "?fields=member_count,description" + "&access_token=" + facebookchannel.UserAccessToken;
            string urlpost = "https://graph.facebook.com/v18.0/" + facebookchannel.SocialChannelNetwokId + "/feed" + "?access_token=" + facebookchannel.UserAccessToken;
            //,messenger_lead_forms
            var http = new HttpClient();
            var httpResponse = await http.GetAsync(url);
          
            if (httpResponse.IsSuccessStatusCode)
            {
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var facebook = JsonConvert.DeserializeObject<dynamic>(httpContent);
              
                var response = await http.GetAsync(urlpost);
                var content = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<dynamic>(content);
                var summary = new
                {
                    FollowersCount = facebook.member_count,
                    About = facebook.description,
                    PostsCount = posts.data.Count,

                };
                result.Data = summary;
                result.Succeeded = true;
                return result;
            }
            result.Message = "Failed To get summary with status Code" + httpResponse.StatusCode;

            return result;
        }
        //Get the comments from a post*/
        [HttpGet("GetLatestPostFieldsByPageId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<PostDetalisDto>> GetLatestPostFieldsByPageId([FromQuery] Guid channelId )
        {
            Response<PostDetalisDto> post = new Response<PostDetalisDto>();
            List<ReactionsDto> Reactions = new List<ReactionsDto>();
            var author_likes_post = false;
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            string url = "https://graph.facebook.com/v17.0/" + facebookchannel.SocialChannelNetwokId + "/feed?limit=1" + "&fields=id,message,full_picture,created_time,from,icon,permalink_url,place,properties,status_type,story,to,updated_time,with_tags,insights.metric(post_clicks, post_engaged_users),reactions.summary(true),shares,attachments" + "&access_token=" + facebookchannel.ChannelAccessToken;
            //insights.metric(post_clicks, post_engaged_users),
            var http = new HttpClient();
            var httpResponse = await http.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var facbookpost = JsonConvert.DeserializeObject<dynamic>(httpContent);
                MediaDto media=new MediaDto();
                if(facbookpost.data[0]?.reactions?.data.Count>0)
                {
                    foreach (var item in facbookpost.data[0]?.reactions?.data)
                    {
                        ReactionsDto reaction = new ReactionsDto();
                        reaction.ReactionId = item.id;
                        reaction.ReactionType = item.type;
                        reaction.Name = item.name;
                        if (reaction.ReactionType == ReactionTypeEnum.LIKE)
                        {
                             author_likes_post = facebookchannel.SocialChannelNetwokId == reaction.ReactionId;
                            
                        }
                      
                              Reactions.Add(reaction);
                    }
                }
               
                if (facbookpost.data[0]?.reactions?.summary?.total_count > facbookpost.data[0]?.reactions?.data.Count)
                {
                    author_likes_post = true;
                    ReactionsDto reaction = new ReactionsDto();
                    reaction.ReactionType = ReactionTypeEnum.LIKE;
                

                }
                 
               


                   var TotalCountReactions = facbookpost.data[0]?.reactions?.summary?.total_count ?? 0;
                   var TotalCountShares = facbookpost.data[0]?.shares?.count ?? 0;
                
                    post.Data = new PostDetalisDto
                    {
                        Caption = facbookpost.data[0].message,
                        PublicationDate = facbookpost.data[0].created_time,
                        MediaData = new List<MediaDto>(),
                        Comments = new List<CommentDto>(),
                        Reactions = Reactions,
                        TotalCountReactions = TotalCountReactions,
                        TotalCountShares = TotalCountShares,
                        PostIdAPI = facbookpost.data[0].id,
                        isLikedByAuthor= author_likes_post,
                     
                    };
                    var insightsData = facbookpost.data[0].insights?["data"] as JArray;

                    if (insightsData != null && insightsData.Count > 0)
                    {
                        var valuesArray = insightsData[0]?["values"] as JArray;

                        if (valuesArray != null && valuesArray.Count > 0)
                        {
                            post.Data.PostClicks = valuesArray[0]?["value"]?.ToObject<int>() ?? 0;
                        }

                        valuesArray = insightsData.Count > 1 ? insightsData[1]?["values"] as JArray : null;

                        if (valuesArray != null && valuesArray.Count > 0)
                        {
                            post.Data.PostEngagedUsers = valuesArray[0]?["value"]?.ToObject<int>() ?? 0;
                        }
                    }
               
               
                if (facbookpost.data[0].status_type == "added_photos")
                {
                    foreach (var attachement in facbookpost?.data[0]?.attachments?.data)
                    {
                        
                            
                            if (attachement.subattachments == null || attachement.subattachments.data.Count == 0)
                            {


                                media.Media_url = attachement.media.image.src;
                                media.Media_type = MediaTypeEnum.IMAGE;
                                post.Data.MediaData.Add(media);

                            }
                            else
                            {
                                var tupe = attachement.type ;
                                if (attachement.type == "album")
                                {
                                    foreach (var subattachment in attachement.subattachments.data)
                                    {
                                        MediaDto mediaCarousel = new MediaDto();
                                        if (subattachment.type == "photo") // Check the type of the subattachment
                                        {
                                            mediaCarousel.Media_url = subattachment.media.image.src;
                                            mediaCarousel.Media_type = MediaTypeEnum.IMAGE;
                                            post.Data.MediaData.Add(mediaCarousel);
                                        }
                                    }
                                }
                            }


                        


                    }
                }
                else if (facbookpost.data[0].status_type == "added_video")
                {
                    foreach (var attachement in facbookpost?.data[0]?.attachments?.data)
                    {
                        if (attachement.subattachments == null || attachement.subattachments.data.Count == 0)
                        {

                            media.Media_url = attachement.media.source;
                            media.Media_type = MediaTypeEnum.VIDEO;
                            post.Data.MediaData.Add(media);
                        }
                        else
                        {
                            if (attachement.type == "album")
                            {
                                foreach (var subattachment in attachement.subattachments.data)
                                {
                                    MediaDto mediaCarousel = new MediaDto();
                                    if (subattachment.type == "video_autoplay") // Check the type of the subattachment
                                    {
                                        mediaCarousel.Media_url = subattachment.media.source;
                                        mediaCarousel.Media_type = MediaTypeEnum.VIDEO;
                                        post.Data.MediaData.Add(mediaCarousel);
                                    }
                                }
                            }
                        }

                    }
                }




                //var Comments= await GetLastPostCommentsById(facbookpost.data[0].id.ToString(), facebookchannel);
                // if (Comments.Succeeded)
                // {

                //         post.Data.Comments = Comments.Data;



                //     post.Succeeded = true;
                // }
                // else
                // {
                //     post.Succeeded = false;
                //     post.Message = $"Failed to get Comments Post with {httpResponse.StatusCode}";
                // }
                post.Succeeded = true;
            }
            else
            {
                post.Succeeded = false;
                post.Message = $"Failed to get Last Post with {httpResponse.StatusCode}";
            }
                   

            return post;
        }
        [HttpPost("CreateReactionPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> CreateReactionPost(PostDetalisDto post, [FromQuery] Guid channelId)
        {
            Response<string> result = new Response<string>();
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            var requestUrl = $"https://graph.facebook.com/v17.0/{post.PostIdAPI}/likes?access_token={facebookchannel.ChannelAccessToken}";

            

           

           

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {facebookchannel.ChannelAccessToken}");




                var response = await client.PostAsync(requestUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Post liked successfully!";
                    return result;
                }
                result.Message = " can't liked Post";
                result.Message = "Failed to like post.";
                return result;

            }
        }
        [HttpPost("DeleteReactionPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeleteReactionPost(PostDetalisDto post, [FromQuery] Guid channelId)
        {
            Response<string> result = new Response<string>();
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            var requestUrl = $"https://graph.facebook.com/v17.0/{post.PostIdAPI}/likes?access_token={facebookchannel.ChannelAccessToken}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {facebookchannel.ChannelAccessToken}");
                var response = await client.DeleteAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Post disliked successfully!";
                    return result;
                }
                result.Message = "Failed to dislike post.";
                return result;

            }
        }
        [HttpPost("DeletePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeletePost(PostDetalisDto post, [FromQuery] Guid channelId)
        {
            Response<string> result = new Response<string>();
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            var requestUrl = $"https://graph.facebook.com/v18.0/{post.PostIdAPI}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {facebookchannel.ChannelAccessToken}");
                var response = await client.DeleteAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Post deleted successfully!";
                    return result;
                }
                result.Message = "Failed to deleted post.";
                return result;

            }
        }
        [HttpPost("CreateReactionComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> CreateReactionComment(CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            Response<string> result = new Response<string>();
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            var requestUrl = $"https://graph.facebook.com/v17.0/{comment.CommentId}/likes?access_token={facebookchannel.ChannelAccessToken}";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {facebookchannel.ChannelAccessToken}");




                var response = await client.PostAsync(requestUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Comment liked successfully!";
                    return result;
                }
                result.Message = "Failed to like comment.";

                return result;

            }
        }
        [HttpPost("DeleteReactionComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeleteReactionComment(CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            Response<string> result = new Response<string>();
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            var requestUrl = $"https://graph.facebook.com/v17.0/{comment.CommentId}/likes?access_token={facebookchannel.ChannelAccessToken}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {facebookchannel.ChannelAccessToken}");
                var response = await client.DeleteAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Comnent disliked successfully!";
                    return result;
                }
                result.Message = "Failed to dislike comment.";

                return result;

            }
        }
        //Get the comments from a post*/
        [HttpGet("GetAllPostFieldByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllPostFieldsByPostId(string PostId, string Token)
        {
            string url = "https://graph.facebook.com/" + PostId + "?fields=message,picture,created_time,from,icon,permalink_url,place,properties,status_type,story,to,updated_time,with_tags" + "&access_token=" + Token;

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(url);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }

        //Get the comments from a post*/
        [HttpGet("GetLastPostCommentsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<CommentDto>>> GetLastPostCommentsById([FromQuery] string PostAPIId, [FromQuery] Guid channelId)
        {
            Response < List < CommentDto >> Comments = new Response<List<CommentDto>>();
            Comments.Data = new List<CommentDto>();
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);
            var author_likes_comment = false;
            string urlComment = $"https://graph.facebook.com/v17.0/{PostAPIId}/comments?fields=like_count,message,from,comment_count,likes,created_time,permalink_url,attachment,reactions.summary(true)&access_token=" + facebookchannel.ChannelAccessToken;

           

            var http = new HttpClient();
            var httpResponse = await http.GetAsync(urlComment);
           
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                var AllComments = JsonConvert.DeserializeObject<dynamic>(httpContent);
                if (AllComments.data.Count > 0)
                {
                    Comments.Succeeded = true;


                    foreach (var data in AllComments.data)
                    {
                        List<ReactionsDto> reactions = new List<ReactionsDto>();
                                                    
                            if (data?.reactions?.data?.Count > 0)
                                {
                                    foreach (var item in data?.reactions?.data)
                                    {
                                        ReactionsDto reaction = new ReactionsDto();
                                        reaction.ReactionId = item.id;
                                        reaction.ReactionType = item.type;
                                        reaction.Name = item.name;
                                if (reaction.ReactionType == ReactionTypeEnum.LIKE)
                                {
                                    author_likes_comment = facebookchannel.SocialChannelNetwokId == reaction.ReactionId;

                                }
                                reactions.Add(reaction);
                                    }
                               }
                       
                        if (data?.reactions?.summary?.total_count > data?.reactions?.data?.Count)
                        {
                            author_likes_comment = true;

                        }


                        CommentDto comment = new CommentDto();
                        comment.CommentId = data.id.ToString();
                        comment.LikesCount = (int)(data?.reactions?.summary?.total_count ?? 0); ;
                        comment.CommentCount = (int)(data.comment_count ?? 0);
                        comment.CreatedTime = data.created_time;
                        comment.LinkUrl = data.permalink_url;
                        comment.FromName = data.from.name;
                        comment.FromId = data.from.id;
                        comment.FromPicture = "";
                        
                        comment.Message = data.message;
                        comment.Reactions = reactions;
                        comment.Replies = new List<CommentDto>();
                        comment.LikesCount = data?.reactions?.summary?.total_count;
                        comment.isLikedByAuthor = author_likes_comment;

                        if (data?.attachment?.type == "video_inline")
                        {
                            comment.VideoUrl = data?.attachment?.media?.source ?? "";
                        }
                        if (data?.attachment?.type == "photo")
                        {
                            comment.PhotoUrl = data?.attachment?.media?.image?.src ?? "";
                        }



                        if (comment.CommentCount > 0)
                        {
                           
                                comment.Replies = await GetRepliesForComment(comment.CommentId, facebookchannel.ChannelAccessToken.ToString());
                          
                            
                        }

                        Comments.Data.Add(comment);
                    }
                    



                    return Comments;
                }
                else
                {
                    Comments.Succeeded = true;
                    return Comments;

                }
            }
            Comments.Message = "failed To get All comment By Post";


            return Comments;
        }
        [HttpGet("GetRepliesForComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<List<CommentDto>> GetRepliesForComment(string commentId, string Token)
        {
            List<CommentDto> replies = new List<CommentDto>();
            
            string urlReplies = $"https://graph.facebook.com/v17.0/{commentId}/comments?fields=like_count,message,from,comment_count,likes,created_time,permalink_url,attachment,reactions&access_token=" + Token;

            var http = new HttpClient();
            var httpResponse = await http.GetAsync(urlReplies);

            if (httpResponse.IsSuccessStatusCode)
            {
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var commentData = JsonConvert.DeserializeObject<dynamic>(httpContent);
                

                if (commentData.data.Count > 0)
                {
                    foreach (var data in commentData.data)
                    {
                        List<ReactionsDto> reactions = new List<ReactionsDto>();
                        if (data?.reactions?.data?.Count > 0)
                        {
                            foreach (var item in data?.reactions?.data)
                            {
                                ReactionsDto reaction = new ReactionsDto();
                                reaction.ReactionId = item.id;
                               
                                reaction.ReactionType = item.type;
                                reaction.Name = item.name;
                                reactions.Add(reaction);
                            }
                        }

                        CommentDto reply = new CommentDto();
                        reply.CommentId = data.id;
                        reply.LikesCount = (int)(data.like_count ?? 0);
                        reply.CommentCount = (int)(data.comment_count ?? 0);
                        reply.CreatedTime = data.created_time;
                        reply.LinkUrl = data.permalink_url;
                        reply.FromName = data.from.name;
                        reply.FromId = data.from.id;
                        reply.FromPicture = "";
                        reply.Reactions = reactions;
                        reply.PhotoUrl = data.attachment?.media?.image?.src ?? "";
                        reply.Message = data.message;
                        reply.LikesCount = reactions.Count;
                        reply.Replies = new List<CommentDto>();

                        if (data?.attachment?.type == "video_inline")
                        {
                            reply.VideoUrl = data?.attachment?.media?.source ?? "";
                        }
                        if (data?.attachment?.type == "photo")
                        {
                            reply.PhotoUrl = data?.attachment?.media?.image?.src ?? "";
                        }
                        replies.Add(reply);
                    }
                }
            }

            return replies;
        }

        //Get the likes from a post*/
        [HttpGet("GetAllPostLikesByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllPostLikesByPostId(string PostId, string Token)
        {
            //    Get the ID, source and story of a post
            //string url = "https://graph.facebook.com/" + PostId + "?fields=id&fields=message&access_token=" + Token;
            //    Get the comments from a post
            //string url = "https://graph.facebook.com/" + PostId + "/comments?access_token="+ Token;
            //    Get the seen state for a post, including a total count summary
            string url = "https://graph.facebook.com/" + PostId + "/likes?summary=true&access_token=" + Token;


            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(url);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }

        //Get the reactions from a post*/
        [HttpGet("GetAllPostReactionsByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllPostReactionsByPostId(string PostId, string Token)
        {
            //    Get people who have reacted to this post, including a total count summary
            string url = "https://graph.facebook.com/" + PostId + "/reactions?summary=true&access_token=" + Token;


            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(url);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }

        //Get the seens from a post*/
        [HttpGet("GetAllPostSeensByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllPostSeensByPostId(string PostId, string Token)
        {
            //    Get people who have seen this post, including a total count summary
            string url = "https://graph.facebook.com/" + PostId + "/seen?summary=true&access_token=" + Token;


            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(url);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }

        [HttpPost("CreateComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<CommentDto>> CreateCommentWithPhoto([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();

            var facebookChannel = await GetFacebookChannelbyChannelId(channelId);
            string fileName = "";
            var data = "";
            // Create a new HttpClient
            using (var httpClient = new HttpClient())
            {
                // Construct the URL for posting a comment with a photo
                string url = $"https://graph.facebook.com/v18.0/{comment.PostId}/comments";

                // Create the content for the request
                var content = new MultipartFormDataContent();
                
              
                content.Add(new StringContent(comment.Message), "message");
                content.Add(new StringContent(facebookChannel.ChannelAccessToken), "access_token");

                // Add the binary data (photo) to the request
               
               
                if (comment.PhotoUrl != "")
                {
                    data = comment.PhotoUrl.Split(',')[1];
                    fileName = Guid.NewGuid().ToString("N") + ".jpg";
                    byte[] imageBytes = Convert.FromBase64String(data);
                    var photoContent = new ByteArrayContent(imageBytes);
                    photoContent.Headers.Add("Content-Type", "image/jpeg"); // Adjust the content type based on your photo format
                    photoContent.Headers.Add("Content-Disposition", $"form-data; name=\"source\"; filename=\"{fileName}\"");
                    content.Add(photoContent, "source", fileName);
                }
                if (comment.VideoUrl != null && comment.VideoUrl != "")
                {
                    data = comment.VideoUrl.Split(',')[1];
                    fileName = Guid.NewGuid().ToString("N") + ".mp4";
                    byte[] imageBytes = Convert.FromBase64String(data);
                    var photoContent = new ByteArrayContent(imageBytes);
                    photoContent.Headers.Add("Content-Type", "video/mp4"); // Adjust the content type based on your photo format
                    photoContent.Headers.Add("Content-Disposition", $"form-data; name=\"source\"; filename=\"{fileName}\"");
                    content.Add(photoContent, "source", fileName);
                }
              
                

                // Make the HTTP POST request
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle the successful response
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    CommentDto commentDto = new CommentDto()
                    {
                        CommentId=json.id,
                        Message = comment.Message,
                        FromId = comment.FromId,
                        PhotoUrl = comment?.PhotoUrl ?? "",
                        VideoUrl = comment?.VideoUrl ?? "",
                        Replies = new List<CommentDto>(),
                        Reactions = new List<ReactionsDto>(),
                        CreatedTime = DateTime.Now,
                        LikesCount = 0
                    };
                    result.Succeeded = true;
                    result.Data = commentDto;
                }
                else
                {
                    // Handle the case where the request was not successful
                    result.Message = $"Failed to create comment with status code {response.StatusCode}";
                }
            }

            return result;
        }

        //public async Task<Response<CommentDto>> CreateComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        //{
        //    Response<CommentDto> result = new Response<CommentDto>();

        //    var facebookchannel = await GetFacebookChannelbyChannelId(channelId);

        //    string url = $"https://graph.facebook.com/v18.0/{comment.PostId}/comments";

        //    var http = new HttpClient();
        //    var content = new Dictionary<string, string>
        //            {
        //                { "message", comment.Message },
        //                { "access_token", facebookchannel.ChannelAccessToken },
        //            };

        //    if (!string.IsNullOrEmpty(comment.PhotoUrl))
        //    {
        //        string photoUrl = $"https://graph.facebook.com/v18.0/{facebookchannel.SocialChannelNetwokId}/photos";
        //        var response = await http.PostAsync(photoUrl, new FormUrlEncodedContent(content));
        //        var httpContent = await response.Content.ReadAsStringAsync();
        //        var commentPost = JsonConvert.DeserializeObject<dynamic>(httpContent);
        //        content["attachment_id"] = commentPost.id;
        //    }

        //    if (!string.IsNullOrEmpty(comment.VideoUrl))
        //    {
        //        string videoUrl = $"https://graph.facebook.com/v18.0/{facebookchannel.SocialChannelNetwokId}/videos";
        //        var response = await http.PostAsync(videoUrl, new FormUrlEncodedContent(content));
        //        var httpContent = await response.Content.ReadAsStringAsync();
        //        var commentPost = JsonConvert.DeserializeObject<dynamic>(httpContent);
        //        content["attachment_id"] = commentPost.id;
        //    }

        //    var httpResponse = await http.PostAsync(url, new FormUrlEncodedContent(content));

        //    if (httpResponse.IsSuccessStatusCode)
        //    {
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        //        var facbookpost = JsonConvert.DeserializeObject<dynamic>(httpContent);
        //        result.Succeeded = true;
        //        result.Data = new CommentDto
        //        {
        //            Message = comment.Message,
        //            CommentId = facbookpost.id,
        //            VideoUrl = "",
        //        };
        //    }
        //    else
        //    {
        //        result.Message = $"Failed to create comment with status code: {httpResponse.StatusCode}";
        //    }

        //    return result;
        //}
       
        [HttpPost("DeleteComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeleteComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            Response<string> result = new Response<string>();
       
            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);

            string url = $"https://graph.facebook.com/v17.0/{comment.CommentId}?access_token=" + facebookchannel.ChannelAccessToken;

            var http = new HttpClient();
           
            var httpResponse = await http.DeleteAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                result.Succeeded = true;
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var facbookpost = JsonConvert.DeserializeObject<dynamic>(httpContent);

                result.Message = " Comment Delete successfully";
            }
            result.Message = "Failet To Created Comment wit status Code" + httpResponse.StatusCode;
            return result;
        }
        [HttpPost("CreateSubComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<CommentDto>> CreateSubComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();


            string fileName = "";
            var data = "";

            using (var httpClient = new HttpClient())
            {
                var facebookchannel = await GetFacebookChannelbyChannelId(channelId);

                string url = $"https://graph.facebook.com/v18.0/{comment.CommentId}/comments";
                // Construct the URL for posting a comment with a photo
              //  string url = $"https://graph.facebook.com/v18.0/{comment.PostId}/comments";

                // Create the content for the request
                var content = new MultipartFormDataContent();


                content.Add(new StringContent(comment.Message), "message");
                content.Add(new StringContent(facebookchannel.ChannelAccessToken), "access_token");

                // Add the binary data (photo) to the request


                if (comment.PhotoUrl != "")
                {
                    data = comment.PhotoUrl.Split(',')[1];
                    fileName = Guid.NewGuid().ToString("N") + ".jpg";
                    byte[] imageBytes = Convert.FromBase64String(data);
                    var photoContent = new ByteArrayContent(imageBytes);
                    photoContent.Headers.Add("Content-Type", "image/jpeg"); // Adjust the content type based on your photo format
                    photoContent.Headers.Add("Content-Disposition", $"form-data; name=\"source\"; filename=\"{fileName}\"");
                    content.Add(photoContent, "source", fileName);
                }
                if (comment.VideoUrl != ""&& comment.VideoUrl !=null)
                {
                    data = comment.VideoUrl.Split(',')[1];
                    fileName = Guid.NewGuid().ToString("N") + ".mp4";
                    byte[] imageBytes = Convert.FromBase64String(data);
                    var photoContent = new ByteArrayContent(imageBytes);
                    photoContent.Headers.Add("Content-Type", "video/mp4"); // Adjust the content type based on your photo format
                    photoContent.Headers.Add("Content-Disposition", $"form-data; name=\"source\"; filename=\"{fileName}\"");
                    content.Add(photoContent, "source", fileName);
                }



                // Make the HTTP POST request
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle the successful response
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    CommentDto commentDto = new CommentDto()
                    {
                        CommentId = json.id,
                        Message = comment.Message,
                        FromId = comment.FromId,
                        PhotoUrl = comment?.PhotoUrl ?? "",
                        VideoUrl = comment?.VideoUrl ?? "",
                        Replies = new List<CommentDto>(),
                        Reactions = new List<ReactionsDto>(),
                        CreatedTime = DateTime.Now,
                        LikesCount = 0
                    };
                    result.Succeeded = true;
                    result.Data = commentDto;
                }
                else
                {
                    // Handle the case where the request was not successful
                    result.Message = $"Failed to create comment with status code {response.StatusCode}";
                }
            }

            return result;
        }
        [HttpPut("UpdateComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<CommentDto>> UpdateComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();


            var facebookchannel = await GetFacebookChannelbyChannelId(channelId);

            string url = $"https://graph.facebook.com/v18.0/{comment.CommentId}";

            var http = new HttpClient();
            var content = new Dictionary<string, string>
            {
                { "message" , comment.Message },
                { "access_token", facebookchannel.ChannelAccessToken },
            };
            var httpResponse = await http.PostAsync(url, new FormUrlEncodedContent(content));
            //if (httpResponse.IsSuccessStatusCode)
            //{
            result.Succeeded = true;
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var facbookpost = JsonConvert.DeserializeObject<dynamic>(httpContent);
            CommentDto comentdto = new CommentDto()
            {
                
                Message = comment.Message,
                CommentId = comment.CommentId,
                PhotoUrl = comment.PhotoUrl,
                VideoUrl = comment.VideoUrl,
                Replies = comment.Replies,
                Reactions = new List<ReactionsDto>(),
                CreatedTime = DateTime.Now,
                LikesCount=comment.LikesCount

            };
            result.Data = comentdto;
            //}
            result.Message = "Failet To Created Comment wit status Code" + httpResponse.StatusCode;
            return result;
        }
        [HttpPost("PublishPostToFacebook")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> PublishPostToFacebook([FromBody]FacebookPostDto facebookPost, [FromQuery]Guid channelId)
        {
            var result = new Response<FacebookPostDto>();
            var post=facebookPost.Post;
            FacebookPostDto social_Post = facebookPost;
            var facebookChannel = await _mediator.Send(new GetFacebookChannelByChannelIdQuery(channelId));
            if (facebookChannel.Succeeded && facebookChannel.Data != null)
            {
                FacebookPostDto facebookPot_willCreated = facebookPost;
                facebookPot_willCreated.FacebookChannelId = facebookChannel.Data.Id;
                facebookPot_willCreated.Post = null;

                var Createdpost = await _mediator.Send(new CreateFacebookPostCommand { FacebookPostDto = facebookPot_willCreated });
                if (Createdpost.Succeeded && Createdpost.Data != null)
                {
                    social_Post.FacebookChannel = facebookChannel.Data;
                    social_Post.Post = post;
                    var facebookPost_published = await _mediator.Send(new PublishToFacebookCommand { FacebookPostDto = social_Post });

                    if (facebookPost_published != null)
                    {
                        Createdpost.Data.FacebookPostNetwokId = facebookPost_published;
                        Createdpost.Data.PublicationStatus = PublicationStatusEnum.Published;


                        var postPublished = await _mediator.Send(new UpdateFacebookPostCommand { FacebookPostDto = Createdpost.Data });

                        if (postPublished.Succeeded && postPublished.Data != null)
                        {
                            result = postPublished;

                        }
                        else
                        {
                            result.Succeeded = false;
                        }
                    }
                    else
                    {
                        result.Succeeded = false;
                    }
                }
                else
                {
                    result.Succeeded = false;
                }
            }
            else
            {
                result.Succeeded = false;
            }
            return Ok(result);
        }



        [HttpPost("PublishPhotoWithPostToFacebook")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> PublishPhotoWithPostToFacebook([FromBody] FacebookPostDto command)
        {
            var publishPost = new PublishToFacebookCommand();
            publishPost.FacebookPostDto = command;
            var entities = await _mediator.Send(publishPost);
            return Ok(entities);

        }





    }
}
