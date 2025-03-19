using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Exceptions.Model; 
using SNM.Instagram.Application.Features.Commands.FacebookChannels;
using SNM.Instagram.Application.Features.Commands.Instagram.InstagramAPI;
using SNM.Instagram.Application.Features.Commands.Instagram.Posts;
using SNM.Instagram.Application.Features.Commands.InstagramChannels;
using SNM.Instagram.Application.Features.Queries.InstagramChannels;
using SNM.Instagram.Domain;
using SNM.Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Xml;

namespace SNM.Instagram.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class InstagramPostAPIController : Controller
    {
        private readonly IMediator _mediator;


        public InstagramPostAPIController(IMediator mediator)
        {

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }
        [HttpGet("GetInstagramChannel")]
        //[HttpGet("{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<InstagramChannelDto> GetInstagramChannelbyChannelId(Guid channelId)
        {

            var getEntities = new GetInstagramChannelByChannelIdQuery(channelId);
            var entities = await _mediator.Send(getEntities);

            return entities;
        }
        [HttpGet("GetInstagramSummary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<object>> GetInstagramSummary([FromQuery] Guid channelId)
        {
            
            Response<object> result = new Response<object>();
            var instaramchannel = await GetInstagramChannelbyChannelId(channelId);
            string url = "https://graph.facebook.com/v18.0/" + instaramchannel.UserId + "?fields=followers_count,media_count,insights.metric(impressions,profile_views).period(day),media{comments_count,like_count},biography" + "&access_token=" + instaramchannel.UserAccessToken;

            //,messenger_lead_forms,audience_gender_age,audience_city,
            var http = new HttpClient();
            var httpResponse = await http.GetAsync(url);

            if (httpResponse.IsSuccessStatusCode)
            {
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var instagram = JsonConvert.DeserializeObject<dynamic>(httpContent);
                try
                {
                    var mediaData = (JArray)instagram.media?.data;

                    if (mediaData != null && mediaData.Any())
                    {
                        var topPost = mediaData
                                                .OrderByDescending(m => (int)m["like_count"])
                                                .ThenByDescending(m => (int)m["comments_count"])
                                                .FirstOrDefault();

                        var postId = topPost["id"]?.Value<string>();


                        Response<PostDetalisDto> post = await GetInstagramMediabyId(postId, instaramchannel.UserAccessToken);
                        var summary = new
                        {
                            postDetails = new PostDetalisDto(),
                            FollowersCount = (int)instagram.followers_count,
                            Comment= topPost["comments_count"]?.Value<string>(),
                            Impressions = ((int?)instagram.insights.data[0]?["values"]?[1]?["value"]) ?? 0,
                            PostsCount = (int)instagram.media_count,
                            ProfileViews = ((int?)instagram.insights.data[1]?["values"]?[1]?["value"]) ?? 0,

                            //UnreadMessageCount = instagram.unread_message_count,
                            //UnreadNotifCount = instagram.unread_notif_count,
                        };
                        if (post.Data != null && post.Succeeded)
                        {
                            summary.postDetails.Caption = post.Data.Caption;
                            summary.postDetails.PublicationDate = post.Data.PublicationDate;
                            summary.postDetails.MediaData = post.Data.MediaData;
                            summary.postDetails.TotalCountReactions = post.Data.TotalCountReactions;
                            summary.postDetails.PostClicks = post.Data.PostClicks;
                            summary.postDetails.TotalImpressions = post.Data.TotalImpressions;
                            summary.postDetails.PostEngagedUsers = post.Data.PostEngagedUsers;
                            summary.postDetails.PostIdAPI = post.Data.PostIdAPI;
                            summary.postDetails.TotalImpressions= post.Data.TotalImpressions;
                        }
                        result.Data = summary;
                        result.Succeeded = true;
                    }
                }
                catch(Exception ex)
                {

                }
                
              
              
                return result;
            }
            result.Message = "Failed To get summary with status Code" + httpResponse.StatusCode;

            return result;
        }
        [HttpGet("GetInstagramMediabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<Response<PostDetalisDto>> GetInstagramMediabyId( string  topPostId ,string UserAccessToken)
        {
            Response<PostDetalisDto> post = new Response<PostDetalisDto>();
            List<ReactionsDto> Reactions = new List<ReactionsDto>();
            var getpost = $"https://graph.facebook.com/v17.0/{topPostId}?fields=caption,username,media_type,media_url,like_count,insights.metric(impressions,engagement,taps_forward).period(day),owner,children,timestamp&access_token={UserAccessToken}";
            var httpClient = new HttpClient();

            var httpResponse = await httpClient.GetAsync(getpost);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                // var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var instagrampost = JsonConvert.DeserializeObject<dynamic>(httpContent);
                MediaDto media = new MediaDto();
                post.Data = new PostDetalisDto ();
                post.Data.Caption = instagrampost.caption;
                post.Data.PublicationDate = instagrampost?.timestamp;
                post.Data.MediaData = new List<MediaDto>();
                post.Data.Comments = new List<CommentDto>();
                // post.Data.Reactions = Reactions;
                post.Data.TotalImpressions = instagrampost.insights.data[0]?["values"]?[0]?["value"] ?? 0;
                post.Data.TotalCountReactions = instagrampost.like_count;
                post.Data.PostClicks = instagrampost.insights.data[2]?["values"]?[0]?["value"] ?? 0;
                post.Data.PostEngagedUsers = instagrampost.insights.data[1]?["values"]?[0]?["value"] ?? 0;
                post.Data.PostIdAPI = instagrampost.id;
                if (instagrampost.media_type == "IMAGE")
                {
                    media.Media_url = instagrampost.media_url;
                    media.Media_type = Domain.Enumeration.MediaTypeEnum.IMAGE;
                    post.Data.MediaData.Add(media);
                }
                else if (instagrampost.media_type == "VIDEO")
                {
                    media.Media_url = instagrampost.media_url;
                    media.Media_type = Domain.Enumeration.MediaTypeEnum.VIDEO;
                    post.Data.MediaData.Add(media);
                }
                else if (instagrampost.media_type == "CAROUSEL_ALBUM")
                {
                    foreach (var child in instagrampost.children.data)
                    {
                        string urlChild = $"https://graph.facebook.com/v17.0/{child.id}?fields=media_url,media_type&access_token={UserAccessToken}";

                        var client = new HttpClient();

                        var response = await client.GetAsync(urlChild);
                        var content = await response.Content.ReadAsStringAsync();
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            // var httpContent = await httpResponse.Content.ReadAsStringAsync();
                            var childmedia = JsonConvert.DeserializeObject<dynamic>(content);

                            MediaDto mediaCarousel = new MediaDto();

                            if (childmedia.media_type == "IMAGE")
                            {
                                mediaCarousel.Media_url = childmedia.media_url;
                                mediaCarousel.Media_type = Domain.Enumeration.MediaTypeEnum.IMAGE;
                                post.Data.MediaData.Add(mediaCarousel);
                            }
                            else if (childmedia.media_type == "VIDEO")
                            {
                                mediaCarousel.Media_url = childmedia.media_url;
                                mediaCarousel.Media_type = Domain.Enumeration.MediaTypeEnum.VIDEO;
                                post.Data.MediaData.Add(media);
                            }


                        }
                    }


                }
                post.Succeeded = true;
            }
            else
            {
                post.Succeeded = false;
                post.Message = $"Failed to get Last Post with {httpResponse.StatusCode}";
            }
            return post;

        }
            [HttpGet("GetInstagramMedia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<Response<PostDetalisDto>> GetInstagramMedia([FromQuery] Guid channelId)
        {

            Response<PostDetalisDto> post = new Response<PostDetalisDto>();
            List<ReactionsDto> Reactions = new List<ReactionsDto>();
            var instagramchannel = await GetInstagramChannelbyChannelId(channelId);
            string url = $"https://graph.facebook.com/v17.0/{instagramchannel.UserId}/media?fields=caption,username,media_type,media_url,like_count,insights.metric(shares,taps_forward,engagement),owner,children,replies,timestamp&limit=1&access_token={instagramchannel.UserAccessToken}";

            var httpClient = new HttpClient();

            var httpResponse = await httpClient.GetAsync(url);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                // var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var instagrampost = JsonConvert.DeserializeObject<dynamic>(httpContent);
                MediaDto media = new MediaDto();
                //var time = instagrampost.data[0]?.timestamp;
                //var PublicationDate = DateTime.ParseExact(time, "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                post.Data = new PostDetalisDto
                {
                    Caption = instagrampost.data[0].caption,
                    PublicationDate = instagrampost.data[0]?.timestamp,
                    MediaData = new List<MediaDto>(),
                    Comments = new List<CommentDto>(),
                    Reactions = Reactions,
                    
                    TotalCountReactions = instagrampost.data[0].like_count,
                    PostClicks = instagrampost.data[0]?.insights.data[0]?["values"]?[0]?["value"] ?? 0,
                    PostEngagedUsers = instagrampost.data[0]?.insights.data[1]?["values"]?[0]?["value"] ?? 0,
                    PostIdAPI = instagrampost.data[0].id,
                };

                if (instagrampost.data[0].media_type == "IMAGE")
                {
                    media.Media_url = instagrampost.data[0].media_url;
                    media.Media_type = Domain.Enumeration.MediaTypeEnum.IMAGE;
                    post.Data.MediaData.Add(media);
                }
                else if (instagrampost.data[0].media_type == "VIDEO")
                {
                    media.Media_url = instagrampost.data[0].media_url;
                    media.Media_type = Domain.Enumeration.MediaTypeEnum.VIDEO;
                    post.Data.MediaData.Add(media);
                }
                else if (instagrampost.data[0].media_type == "CAROUSEL_ALBUM")
                {
                    foreach (var child in instagrampost.data[0].children.data)
                    {
                        string urlChild = $"https://graph.facebook.com/v17.0/{child.id}?fields=media_url,media_type&access_token={instagramchannel.UserAccessToken}";

                        var client = new HttpClient();

                        var response = await client.GetAsync(urlChild);
                        var content = await response.Content.ReadAsStringAsync();
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            // var httpContent = await httpResponse.Content.ReadAsStringAsync();
                            var childmedia = JsonConvert.DeserializeObject<dynamic>(content);

                            MediaDto mediaCarousel = new MediaDto();

                            if (childmedia.media_type == "IMAGE")
                            {
                                mediaCarousel.Media_url = childmedia.media_url;
                                mediaCarousel.Media_type = Domain.Enumeration.MediaTypeEnum.IMAGE;
                                post.Data.MediaData.Add(mediaCarousel);
                            }
                            else if (childmedia.media_type == "VIDEO")
                            {
                                mediaCarousel.Media_url = childmedia.media_url;
                                mediaCarousel.Media_type = Domain.Enumeration.MediaTypeEnum.VIDEO;
                                post.Data.MediaData.Add(media);
                            }


                        }
                    }

                   
                }
                else
                {
                    post.Succeeded = false;
                    post.Message = $"Failed to get Last Post with {httpResponse.StatusCode}";
                
                }
                post.Succeeded = true;

            

            }
            return post;

        }


        [HttpGet("commentlike")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<int> GetInstagramCommentLikes(string igCommentId)
        {
            string apiVersion = "v16.0";
            string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
            string fields = "like_count";
            string url = $"https://graph.facebook.com/{apiVersion}/{igCommentId}?fields={fields}&access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);
                    int likeCount = jsonObject["like_count"];

                    return likeCount;
                }

                return 0;
            }
        }



        [HttpGet("comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<CommentDto>>> GetInstagramComments([FromQuery] string PostAPIId, [FromQuery] Guid channelId)
        {
            Response<List<CommentDto>> Comments = new Response<List<CommentDto>>();
            Comments.Data = new List<CommentDto>();
            var instagramchannel = await GetInstagramChannelbyChannelId(channelId);
            string urlComment = $"https://graph.facebook.com/{PostAPIId}/comments?fields=from,id,text,like_count,username,replies,comment_count,timestamp&access_token={instagramchannel.UserAccessToken}";

            var http = new HttpClient();
            var httpResponse = await http.GetAsync(urlComment);

            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                var AllComments = JsonConvert.DeserializeObject<dynamic>(httpContent);
                Comments.Succeeded = true;


                foreach (var data in AllComments.data)
                {
                    Response<Dictionary<string, string>> userPictureawait = await UserComment(data.from.id.ToString(), instagramchannel.UserAccessToken);

                    CommentDto comment = new CommentDto();
                    if (userPictureawait.Succeeded)
                    {
                        comment.FromPicture = userPictureawait?.Data["profile_picture_url"] ?? "";

                    }
                    comment.CommentId = data.id.ToString();
                    comment.LikesCount = (int)(data.like_count ?? 0); ;
                    comment.CommentCount = (int)(data.comment_count ?? 0);
                    comment.CreatedTime = data?.timestamp ?? DateTime.UtcNow;
                    comment.FromName = data.from.username;
                    comment.FromId = data.from.id;


                    comment.Message = data.text;
                    comment.Replies = new List<CommentDto>();
                    if (data?.replies?.data?.Count > 0)
                    {
                        try
                        {
                            comment.Replies = await GetRepliesForComment(comment.CommentId, instagramchannel.UserAccessToken.ToString());
                        }
                        catch (Exception ex)
                        {
                        }

                        // Fetch and set replies separately

                    }
                    Comments.Data.Add(comment);
                }



                return Comments;
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

            string urlReplies = $"https://graph.facebook.com/v17.0/{commentId}/replies?fields=from,id,text,like_count,username,timestamp&access_token=" + Token;

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
                        Response<Dictionary<string, string>> userPictureawait = await UserComment(data.from.id.ToString(), Token);

                        CommentDto reply = new CommentDto();
                        if (userPictureawait.Succeeded)
                        {
                            reply.FromPicture = userPictureawait?.Data["profile_picture_url"] ?? "";

                        }

                        reply.CommentId = data.id;
                        reply.LikesCount = (int)(data.like_count ?? 0);
                        reply.CommentCount = (int)(data.comment_count ?? 0);
                        reply.CreatedTime = data?.timestamp ?? DateTime.UtcNow;

                        reply.FromName = data.from.username;
                        reply.FromId = data.from.id;


                        reply.Message = data.text;



                        replies.Add(reply);
                    }
                }
            }

            return replies;
        }

        [HttpGet("replies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> GetInstagramCommentReplies(string replies)
        {
            string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
            string url = $"https://graph.facebook.com/{replies}/replies?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                List<InstagramComments> repliesList = new List<InstagramComments>();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);

                    foreach (var item in jsonObject["data"])
                    {
                        string id = item["id"];
                        string text = item["text"];
                        string timestamp = item["timestamp"];

                        InstagramComments reply = new InstagramComments
                        {
                            Id = id,
                            text = text,
                            timestamp = timestamp
                        };
                        repliesList.Add(reply);
                    }

                    return Ok(repliesList);
                }

                return BadRequest(content);
            }
        }
        [HttpGet("UserComment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<Response<Dictionary<string, string>>> UserComment(string igUserComment, string accesToken)
        {
            Response<Dictionary<string, string>> result = new Response<Dictionary<string, string>>();

            string url = $"https://graph.facebook.com/{igUserComment}?fields=profile_picture_url&access_token={accesToken}";

            var http = new HttpClient();
            try
            {
                var httpResponse = await http.GetAsync(url);

                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    var userComment = JsonConvert.DeserializeObject<dynamic>(httpContent);
                    result.Data = new Dictionary<string, string>
                            {
                                { "profile_picture_url", userComment.profile_picture_url.ToString() }
                            };
                    
                    result.Succeeded = true;
                    return result;
                }
                else
                {
                    result.Message = "Failed To get User Picture";
                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

        }



        [HttpPost("repliesinsta")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> CreateInstagramCommentReply(string igCommentId, [FromQuery] string message)
        {
            string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
            string url = $"https://graph.facebook.com/{igCommentId}/replies";

            var parameters = new Dictionary<string, string>
    {
        { "access_token", accessToken },
        { "message", message }
    };

            using (var httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(parameters);
                var response = await httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                return BadRequest(responseContent);
            }
        }

        //Get the comments from a post*/
        [HttpGet("GetAllPostFieldByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllPostFieldsByPostId(string PostId, string Token)
        {
            string url = "https://api.instagram.com/" + PostId + "?fields=message,picture,created_time,from,icon,permalink_url,place,properties,status_type,story,to,updated_time,with_tags" + "&access_token=" + Token;

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
        [HttpGet("GetAllPostCommentsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllPostCommentsByPostId(string PostId, string Token)
        {
            //    Get the ID, source and story of a post
            //string url = "https://api.instagram.com/" + PostId + "?fields=id&fields=message&access_token=" + Token;
            //    Get the comments from a post
            //string url = "https://api.instagram.com/" + PostId + "/comments?access_token="+ Token;
            //    Get the seen state for a post, including a total count summary
            string url = "https://api.instagram.com/" + PostId + "/comments?summary=true&access_token=" + Token;


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

        //Get the likes from a post*/
        [HttpGet("GetAllPostLikesByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllPostLikesByPostId(string PostId, string Token)
        {
            //    Get the ID, source and story of a post
            //string url = "https://api.instagram.com/" + PostId + "?fields=id&fields=message&access_token=" + Token;
            //    Get the comments from a post
            //string url = "https://api.instagram.com/" + PostId + "/comments?access_token="+ Token;
            //    Get the seen state for a post, including a total count summary
            string url = "https://api.instagram.com/" + PostId + "/likes?summary=true&access_token=" + Token;


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
            string url = "https://api.instagram.com/" + PostId + "/reactions?summary=true&access_token=" + Token;


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
            string url = "https://api.instagram.com/" + PostId + "/seen?summary=true&access_token=" + Token;


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
        ///
        [HttpPost("PublishPostToInstagram")]

        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> PublishPostToInstagram([FromBody] InstagramPostDto instagramPost, [FromQuery] Guid channelId)
        {

            var result = new Response<InstagramPostDto>();
            var post = instagramPost.PostDto;
            InstagramPostDto social_Post = instagramPost;
            //get
            var instagramchannel = await _mediator.Send(new GetInstagramChannelByIdQuery(channelId));
            if (instagramchannel != null)
            {
                InstagramPostDto instagram_willCreated = instagramPost;
                instagramPost.InstagramChannelDto = instagramchannel;
                instagram_willCreated.PostDto = null;
                //create
                var Createdpost = await _mediator.Send(new CreateInstagramPostCommand { instagramPostDto = instagram_willCreated });

                if (Createdpost.Succeeded && Createdpost.Data != null)
                {
                    instagramPost.InstagramChannelDto = instagramchannel;
                    social_Post.PostDto = post;

                    var Instagram_Publish = await _mediator.Send(new PublishPostToInstagramCommand { InstagramPostDto = social_Post });

                    if (Instagram_Publish.Succeeded != false)
                    {

                        JObject json = JObject.Parse(Instagram_Publish.Message);


                        string id = json["id"].Value<string>();


                        Createdpost.Data.PublicationStatus = Domain.Enumeration.PublicationStatusEnum.Published;
                        Createdpost.Data.InstagramPostId = id;
                        var postPublished = await _mediator.Send(new UpdatePostInstaagramCommand { InstagramPostDto = Createdpost.Data });
                        //  var postPublished = await _mediator.Send(new UpdateInstagramChannelCommand { InstagramChannelDto = instagramchannel });

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
                        result.Message = Instagram_Publish.Message;
                        result.Succeeded = false;
                    }
                }
            }
            else
            {
                result.Succeeded = false;
            }
            return Ok(result);
        }
        [HttpPost("uploadimage")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Generate a unique file name or use the original file name
                var fileName = Path.GetFileName(imageFile.FileName);

                // Get the path where you want to save the uploaded image
                var uploadPath = Path.Combine("C:\\Users\\Lenovo\\OneDrive\\Documents\\SNM\\Instagram\\External\\Instagram.Presentation", "uploads");

                // Create the uploads directory if it doesn't exist
                Directory.CreateDirectory(uploadPath);

                // Combine the path and file name
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // You can save the file path to a database or perform other actions here
                // For simplicity, we'll return the file path in this example
                return Content(filePath);
            }

            return BadRequest("No file was selected.");
        }

        [HttpPost("CreateComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<CommentDto>> CreateComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();


            var instagramchannel = await GetInstagramChannelbyChannelId(channelId);

            string url = $"https://graph.facebook.com/v17.0/{comment.PostId}/comments";

            var http = new HttpClient();
            var content = new Dictionary<string, string>
            {
                { "message" , comment.Message },
                { "access_token", instagramchannel.UserAccessToken },
            };
            var httpResponse = await http.PostAsync(url, new FormUrlEncodedContent(content));
            //if (httpResponse.IsSuccessStatusCode)
            //{
                result.Succeeded = true;
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<dynamic>(httpContent);
                CommentDto comentdto = new CommentDto()
                {
                    Message = comment.Message,
                    CommentId = json.id,
                    PhotoUrl = "",
                    VideoUrl = ""

                };
                result.Data = comentdto;
            //}
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


        var instagramchannel = await GetInstagramChannelbyChannelId(channelId);

        string url = $"https://graph.facebook.com/v17.0/{comment.CommentId}/replies";

        var http = new HttpClient();
        var content = new Dictionary<string, string>
            {
                { "message" , comment.Message },
                { "access_token", instagramchannel.UserAccessToken },
            };
        var httpResponse = await http.PostAsync(url, new FormUrlEncodedContent(content));
            if (httpResponse.IsSuccessStatusCode)
            {
                result.Succeeded = true;
        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        var json = JsonConvert.DeserializeObject<dynamic>(httpContent);
        CommentDto comentdto = new CommentDto()
        {
            
            Message = comment.Message,
            CommentId = json.id,
            PhotoUrl = "",
            VideoUrl = "",
            Replies = new List<CommentDto>(),
            Reactions = new List<ReactionsDto>(),
            CreatedTime = DateTime.Now,

        };
        result.Data = comentdto;
        }
        result.Message = "Failet To Created Comment wit status Code" + httpResponse.StatusCode;
        return result;
    }
}

}
