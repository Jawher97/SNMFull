using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Application.DTO;
using System.Linq;
using SNM.LinkedIn.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SNM.LinkedIn.Infrastructure.DataContext;
using System.Web;
using System.Net;
using MediatR;
using static System.Collections.Specialized.BitVector32;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts;
using SNM.LinkedIn.Application.Features.Queries.LikedInProfileData;
using SNM.LinkedIn.Application.Features.Queries.LinkedInChannels;
using SNM.LinkedIn.Application.Features.Commands.LinkedeIn.LinkedinAPI;
using System.Xml.Linq;
using Elasticsearch.Net;
using SNM.LinkedIn.Domain.Enumeration;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SNM.LinkedIn.Presentation.Controllers
{
    [ApiController]
    [Route("apiLinkedIn/v1/[controller]")]
   
    public class LinkedInAPIController : Controller
    {
        private readonly string _versionNumber;
        private readonly ILinkedInAPIRepository<Guid> _repository;
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;
        private ApplicationDbContext _dbContext;

        public LinkedInAPIController(IConfiguration config, ILinkedInAPIRepository<Guid> repository, ApplicationDbContext dbContext, IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));            
            _config = config;
            _repository = repository;
            _versionNumber = _config.GetValue<string>("LinkedIn:versionNumber");
            _dbContext = dbContext;
        }


        /*Get organization Details*/
        [HttpGet("GetOrganizationId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetOrganizationId(string name, string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizations?q=vanityName&vanityName={name}";

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }


        /*Get organization Details*/
        [HttpGet("GetOrganizationBrands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetOrganizationBrands(string id, string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizations?q=parentOrganization&parent=urn:li:organization:{id}";

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }


        [HttpGet("GetOrganizationFollowersCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetOrganizationFollowersCount(string orgurn, string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityFollowerStatistics?q=organizationalEntity&organizationalEntity={orgurn}";

            var rez = Task.Run(async () =>
            {

                var httpResponse = await httpClient.GetAsync(apiUrl);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;

            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }


        [HttpGet("RetrieveOrganizationStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RetrieveStatistics(string orgUrn, string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationPageStatistics?q=organization&organization=orgUrn";

            var response = await httpClient.GetAsync(apiUrl);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return Ok(response);
            }

            return Ok(responseJson);
        }


        [HttpGet("GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPost(string accessToken, string shareurn)
        {
            var encodedUrn = Uri.EscapeDataString(shareurn);
           
            var apiUrl = $"https://api.linkedin.com/rest/posts/{encodedUrn}?viewContext=AUTHOR";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var rez = Task.Run(async () =>
                {

                   // client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                    var httpResponse = await client.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;

                });
                var rezJson = JObject.Parse(rez.Result);

                return Ok(rez.Result);
            }
        }
        [HttpGet("GetLinkedinChannel")]
        //[HttpGet("{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<LinkedInChannel> GetLinkedinChannel([FromBody] Guid channelId)
        {
            
                var getEntities = new GetLinkedinChannelbyChannelId(channelId);
                var entities = await _mediator.Send(getEntities);

                
            
            return entities;
        }

        [HttpGet("GetLatestPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<Response<PostDetalisDto>> GetLatestPost([FromQuery] Guid channelId)
        {
          
         
                    Response<PostDetalisDto> result = new Response<PostDetalisDto>();
                    var  linkedinchannel = await GetLinkedinChannel(channelId);
                    var apiUrl = $"https://api.linkedin.com/rest/posts?author={WebUtility.UrlEncode(linkedinchannel.Author_urn)}&q=author&count=10&sortBy=(value:CREATED)";
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", linkedinchannel.AccessToken);
                    client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                    client.DefaultRequestHeaders.Add("X-RestLi-Method", "FINDER");
                    client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                    var httpResponse = await client.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var responseJson = JObject.Parse(httpContent);
                        var elements = responseJson["elements"];

                        if (elements.Count() > 0)
                        {
                            var firstPost = elements.First;

                            string postId = firstPost["id"].ToString();
                            long milliseconds = firstPost["publishedAt"].Value<long>();

                            DateTime createdAt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                .AddMilliseconds(milliseconds);

                            string authorUrn = firstPost["author"].ToString();
                            string commentary = firstPost["commentary"].ToString();

                    /* Post Details*/

                    PostDetalisDto post = new PostDetalisDto
                    {
                        
                        PublicationDate = createdAt,
                        Caption = commentary,
                        MediaData = new List<MediaDto>(),
                        Comments = new List<CommentDto>(),
                        Reactions = new List<ReactionsDto>(),
                        PostClicks = 0,
                        PostIdAPI = postId,
                        FromId=linkedinchannel.Author_urn,
                        PostEngagedUsers =0
                            };




                            //if (!Convert.ToBoolean(firstPost["content"]))

                           var mediaId = firstPost?["content"]?["media"]?["id"]?.ToString() ?? "";
                           var MultiplImage = firstPost?["content"]?["multiImage"]?["images"]?.ToString() ?? "";
                            if(MultiplImage != "")
                            {
                                foreach(var mediaLinkedin in firstPost?["content"]?["multiImage"]?["images"].ToList() )
                                {
                                    var id = mediaLinkedin["id"].ToString();
                                    if (id.ToString().StartsWith("urn:li:image"))
                                    {
                                        MediaDto media = new MediaDto();
                                        media.Media_url = await GetImageUrl(id, linkedinchannel.AccessToken);
                                        media.Media_type = Domain.Enumeration.MediaTypeEnum.IMAGE;
                                        post.MediaData.Add(media);
                                        // latestPost.Photo = latestPost.MediaUrn;

                                    }
                                    else if (id.StartsWith("urn:li:video"))
                                    {
                               
                                        MediaDto media = new MediaDto();
                                        media.Media_url = await GetVideoUrl(id, linkedinchannel.AccessToken);
                                        media.Media_type = Domain.Enumeration.MediaTypeEnum.VIDEO;
                                        post.MediaData.Add(media);
                                    }
                                }
                                   
                             }
                        if (mediaId != "")
                            {

                                if (mediaId.StartsWith("urn:li:image"))
                                {
                                    MediaDto media = new MediaDto();
                                    media.Media_url = await GetImageUrl(mediaId, linkedinchannel.AccessToken);
                                            media.Media_type = Domain.Enumeration.MediaTypeEnum.IMAGE;
                                            post.MediaData.Add(media);
                                            // latestPost.Photo = latestPost.MediaUrn;

                                        }
                                        else if (mediaId.StartsWith("urn:li:video"))
                                        {
                                    MediaDto media = new MediaDto();
                                    media.Media_url = await GetVideoUrl(mediaId, linkedinchannel.AccessToken);
                                    media.Media_type = Domain.Enumeration.MediaTypeEnum.VIDEO;
                                    post.MediaData.Add(media);
                                }
                            }

                      //  await RetrieveSocialActions(postId, linkedinchannel.AccessToken);
                       //Response<List<CommentDto>> comments = await RetrieveComments(postId, linkedinchannel.AccessToken,linkedinchannel.Author_urn);

                        
                       //     if (comments.Succeeded)
                       //     {
                       //          post.Comments = comments.Data;
                        
                      
                       //     }
                    Response<List<ReactionsDto>> reactions=await RetrieveLikes(postId, linkedinchannel.AccessToken);
                    if (reactions.Succeeded)
                    {
                        post.Reactions = reactions.Data;
                        post.TotalCountReactions= reactions.Data.Count;
                        foreach(var likes in post.Reactions)
                        {
                            if(linkedinchannel.Author_urn== likes.FromUserId)
                            {
                                post.isLikedByAuthor = true;

                            }
                           

                        }
                       
                    }
                  //  var insight = await RetrieveSocialActions(postId, linkedinchannel.AccessToken);
                   
                    result.Data = post;
                    result.Succeeded = true;

                            }
                        else
                        {
                            result.Succeeded = true;
                            result.Message = "No post Exist";
                        }


                    
                
                
                




                    /*else
                    {
                        latestPost.mediaUrn = null; latestPost.mediaUrl = null;
                    }*/

                    /*Creator Member Infos*/

                    //if (memberUrn.StartsWith("urn:li:organization"))
                    //{
                    //    int lastColonIndex = memberUrn.LastIndexOf(':');
                    //    string author_id = memberUrn.Substring(lastColonIndex + 1);

                    //    var profile = await _repository.GetCompanyProfile(accessToken, author_id);
                    //    latestPost.author_username = profile.FullName;
                    //    latestPost.author_profilelink = "https://" + profile.LinkedinProfileLink;
                    //}
                    //else if (memberUrn.StartsWith("urn:li:person"))
                    //{
                    //    int lastColonIndex = memberUrn.LastIndexOf(':');
                    //    string author_id = memberUrn.Substring(lastColonIndex + 1);

                    //    var profile = await _repository.GetMemberProfile(accessToken, author_id);
                    //    latestPost.author_username = profile.FullName;
                    //    latestPost.author_profilelink = "https://" + profile.LinkedinProfileLink;
                    //}

                    ///*Insights*/
                    //if (memberUrn.StartsWith("urn:li:person"))
                    //{


                    //    var insight = await RetrieveSocialActions(latestPost.postId, accessToken);

                    //    latestPost.insight.totalLikes = insight.totalLikes;
                    //    latestPost.insight.totalComments = insight.totalComments;
                    //    latestPost.insight.isLikedByAuthor = insight.isLikedByAuthor;

                    //    if (latestPost.Video != null)
                    //    {
                    //        var videoViews = await VideoAnalytics(latestPost.postId, accessToken);
                    //        latestPost.insight.videoViews = videoViews;
                    //    }
                    //}

                    //if (memberUrn.StartsWith("urn:li:organization"))
                    //{
                    //    LinkedInInsight insight = new LinkedInInsight();

                    //    insight = await GetPostStatistics(memberUrn, latestPost.postId, accessToken);

                    //    latestPost.insight.totalLikes = insight.totalLikes;
                    //    latestPost.insight.totalComments = insight.totalComments;
                    //    latestPost.insight.shareCount = insight.shareCount;
                    //    latestPost.insight.clickCount = insight.clickCount;
                    //    latestPost.insight.impressionCount = insight.impressionCount;

                    //    if (latestPost.Video != null)
                    //    {
                    //        var videoViews = await VideoAnalytics(latestPost.postId, accessToken);
                    //        latestPost.insight.videoViews = videoViews;
                    //    }
                    //}



                    //await _dbContext.AddAsync(latestPost);
                    //await _dbContext.SaveChangesAsync();


                    //return Ok(latestPost);


                


            }
            else
            {
                result.Succeeded = false;
                result.Message = $"Failed To Get Post with status code {httpResponse.StatusCode}";
            }
            return result;
        }


        [HttpPost("PublishToLinkedIn")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> PublishToLinkedIn([FromBody] LinkedInPostDto linkedinpost, [FromQuery] Guid channelId)
        {


            var result = new Response<LinkedInPostDto>();

            var post = linkedinpost.PostDto;
            LinkedInPostDto social_Post = linkedinpost;
            var linkedInChannelDto = await _mediator.Send(new GetLinkedInChannelByIdQuery(channelId));
                if (linkedInChannelDto != null)
                {
                LinkedInPostDto linkedinPost_willCreated = linkedinpost;
                linkedinPost_willCreated.LinkedInChannelId = linkedInChannelDto.Id;
                linkedinPost_willCreated.PostDto = null;
                var Createdpost = await _mediator.Send(new CreatePostLinkedinCommand { linkedInPostDto = linkedinPost_willCreated });
                if (Createdpost.Succeeded && Createdpost.Data != null)
                {
                    /// on peut eliminer     social_Post.LinkedinChannel = linkedinChannel;
                    social_Post.PostDto = post;
                    var response = await _mediator.Send(new PublishPostToLinkedinCommand { LinkedInPostDto = social_Post, LinkedInChannelDto = linkedInChannelDto });
                    //   var linkedinpostId = await _repository.PublishToLinkedIn(post, linkedInChannelDto);
                    if (response.Succeeded != false)
                    {
                        Createdpost.Data.PublicationStatus = Domain.Enumeration.PublicationStatusEnum.Published;
                        Createdpost.Data.PostUrn = response.Data;
                        //Createdpost.Data.MediaUrn = linkedinpost.MediaUrn;
                        //Createdpost.Data.MediaUrl = linkedinpost.MediaUrl;
                        var postPublished = await _mediator.Send(new UpdatePostLinkedinCommand { LinkedinPostDto = Createdpost.Data });


                        if (postPublished.Succeeded && postPublished.Data != null)
                        {
                            result = postPublished;

                        }
                        else
                        {

                            result.Succeeded = false;
                            throw new Exception("result:"+response.Errors.ToString());
                        }
                    }
                    else
                    {
                        //  result.Message = linkedinpostId;
                        result.Succeeded = false;
                        throw new Exception("result:" + response.Data.ToString());
                    }
                }
            }
            else
            {

                result.Succeeded = false;
            }
            return Ok(result);

            //if (linkedinpost.mediaUrn != null)
            //{
            //    if (linkedinpost.mediaUrn.StartsWith("urn:li:image"))
            //    {

            //        linkedinpost.mediaUrl = await GetImageUrl(linkedinpost.mediaUrn, linkedInProfileDto.access_token);
            //        linkedinpost.photo = linkedinpost.mediaUrn;

            //    }
            //    else if (linkedinpost.mediaUrn.StartsWith("urn:li:video"))
            //    {
            //        linkedinpost.mediaUrl = await GetVideoUrl(linkedinpost.mediaUrn, linkedInProfileDto.access_token);
            //        linkedinpost.video = linkedinpost.mediaUrn;
            //    }

            //}

            //if (post.author_urn.StartsWith("urn:li:person"))
            //{

            //    if (linkedinpost.video != null)
            //    {
            //        var videoViews = await VideoAnalytics(linkedinpost.postId, linkedInProfileDto.access_token);
            //        linkedinpost.insight.videoViews = videoViews;
            //    }

            //    var insight = await RetrieveSocialActions(linkedinpost.postId, linkedInProfileDto.access_token);

            //    linkedinpost.insight.totalLikes = insight.totalLikes;
            //    linkedinpost.insight.totalComments = insight.totalComments;
            //    linkedinpost.insight.isLikedByAuthor = insight.isLikedByAuthor;

            //    int lastColonIndex = post.author_urn.LastIndexOf(':');
            //    string author_id = post.author_urn.Substring(lastColonIndex + 1);

            //    var profile = await _repository.GetMemberProfile(linkedInProfileDto.access_token, author_id);
            //    linkedinpost.author_username = profile.FullName;
            //    linkedinpost.author_profilelink = "https://" + profile.LinkedinProfileLink;



            //}

            //if (post.author_urn.StartsWith("urn:li:organization"))
            //{
            //    LinkedInInsight insight = new LinkedInInsight();

            //    insight = await GetPostStatistics(post.author_urn, linkedinpost.postId, linkedInProfileDto.access_token);

            //    linkedinpost.insight.totalLikes = insight.totalLikes;
            //    linkedinpost.insight.totalComments = insight.totalComments;
            //    linkedinpost.insight.shareCount = insight.shareCount;
            //    linkedinpost.insight.clickCount = insight.clickCount;
            //    linkedinpost.insight.impressionCount = insight.impressionCount;

            //    int lastColonIndex = post.author_urn.LastIndexOf(':');
            //    string author_id = post.author_urn.Substring(lastColonIndex + 1);

            //    var profile = await _repository.GetCompanyProfile(linkedInProfileDto.access_token, author_id);
            //    linkedinpost.author_username = profile.FullName;
            //    linkedinpost.author_profilelink = "https://" + profile.LinkedinProfileLink;


            //    if (linkedinpost.video != null)
            //    {
            //        var videoViews = await VideoAnalytics(linkedinpost.postId, linkedInProfileDto.access_token);
            //        linkedinpost.insight.videoViews = videoViews;
            //    }


            //linkedinpost.createdAt = DateTime.Now;

            //await _dbContext.AddAsync(linkedinpost);
            //await _dbContext.SaveChangesAsync();


        }

        #region Get latest linkedIn post
        //[HttpGet("GetLastsLinkedInPost")]
        //[ProducesDefaultResponseType]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> GetLastsLinkedInPost( [FromQuery] string memberUrn)
        //{
        //    //var status = await _repository.get(accessToken, reaction.Actor, reaction.Root);
        //   // var all = await _dbContext.LinkedInPost.Include(x => x.insight).ToListAsync();

        //    //if (all.Count == 0)
        //    //{
        //    //    var post = await GetLatestPost(accessToken, memberUrn);
        //    //    return Ok(post);
        //    //}
        //    //else
        //    {
        //        return Ok(all);
        //    }
            
        //}
        #endregion
        //[HttpGet("GetLinkedinPosts")]
        //[ProducesDefaultResponseType]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> GetLinkedInPost([FromQuery] string accessToken, [FromQuery] string memberUrn)
        //{
        //    var all = await _dbContext.LinkedInPost.Include(x => x.insight).ToListAsync();

        //    if (all.Count == 0)
        //    {
        //        var post = await GetLatestPost(accessToken, memberUrn);
        //        return Ok(post);
        //    }
        //    else
        //    {
        //        return Ok(all);
        //    }
            
        //}

        [HttpGet("GetVideoUrl")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetVideoUrl(string mediaUrn, string accessToken)
        {
            var url = $"https://api.linkedin.com/rest/videos/{mediaUrn}";

            var client = new HttpClient();

            using (var http = client)
            {
               

                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                http.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);


                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var resJson = JObject.Parse(content);
                var mediaUrl = resJson["downloadUrl"].ToString();

                return mediaUrl;
            }
        }

        [HttpGet("GetImageUrl")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<string> GetImageUrl(string mediaUrn, string accessToken)
        {
            var url = $"https://api.linkedin.com/rest/images/{mediaUrn}";

            var client = new HttpClient();

            using (var http = client)
            {


                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                http.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);


                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var resJson = JObject.Parse(content);
                var mediaUrl = resJson["downloadUrl"].ToString();

                return mediaUrl;
            }
        }


        /*Video analytics*/
        [HttpGet("VideoAnalytics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<int> VideoAnalytics([FromQuery] string postUrn, [FromQuery]  string accesstoken)
        {
            /*
             * Type of analytics returned:
                VIDEO_VIEW Video views with play-pause cycles for at least 3 seconds. Auto-looping videos are counted as one when loaded. Each subsequent auto-looped play does not increase this metric. Analytics data for this metric will not be available after six months.
                VIEWER: Unique viewers who made engaged plays on the video. Auto-looping videos are counted as one when loaded. Each subsequent auto-looped play does not increase this metric. Analytics data for this metric will not be available after six months.
                TIME_WATCHED: The time the video was watched in milliseconds. Video auto-looping will continue to increase this metric for each subsequent play.
                TIME_WATCHED_FOR_VIDEO_VIEWS: The time watched in milliseconds for video play-pause cycles that are at least 3 seconds. Video auto-looping will continue to increase this metric for each subsequent play. Analytics data for this metric will be available for six months.
            */


            var apiUrl = $"https://api.linkedin.com/rest/videoAnalytics?q=entity&entity={postUrn}&type=VIDEO_VIEW";

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                    http.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                    var httpResponse = await http.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result).ToString();
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            if (jsonObject["elements"] is JArray elementsArray && elementsArray.HasValues)
            {
                int videoViews = jsonObject["elements"][0]["value"];
                return videoViews;
            }
            else
            {
                int videoViews = 0;
                return videoViews;
            }

            
        }



        /*Retrieve Comments and likes: vue d'ensemble du nombre réactions*/
        [HttpGet("RetrieveCommentsAndLikeSummary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Response<List<ReactionsDto>>> RetrieveSocialActions(string entityUrn, string accesstoken)
        {
            Response<List<ReactionsDto>> result = new Response<List<ReactionsDto>>();
            result.Data = new List<ReactionsDto>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/v2/socialActions/{entityUrn}/likes"; //post or comment
            var httpResponse = await httpClient.GetAsync(apiUrl);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseJson = JObject.Parse(httpContent).ToString();
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseJson);

                foreach (var element in jsonObject["elements"])
                {
                    string actorUrn = element["lastModified"]["actor"];
                    var reactionType = element["reactionType"];

                    if (actorUrn.StartsWith("urn:li:person"))
                    {
                        int lastColonIndex = actorUrn.LastIndexOf(':');
                        string ActorId = actorUrn.Substring(lastColonIndex + 1);

                        var profile = await _repository.GetLinkedInProfilePicture(accesstoken, ActorId);
                      
                            ReactionsDto reaction = new ReactionsDto()
                            {
                                Name = profile.Data.UserName,
                                ReactionType = ReactionTypeEnum.LIKE,
                                Picture = profile.Data?.CoverPhoto ?? "",
                                FromUserId = profile.Data.ProfileUserId


                            };
                            result.Data.Add(reaction);
                    
                    }
                    else if (actorUrn.StartsWith("urn:li:organization"))
                    {
                        int lastColonIndex = actorUrn.LastIndexOf(':');
                        string ActorId = actorUrn.Substring(lastColonIndex + 1);

                        var profile = await _repository.GetCompanyProfile(accesstoken, ActorId);

                        ReactionsDto reaction = new ReactionsDto()
                        {
                            Name = profile.Data.DisplayName,
                            ReactionType = ReactionTypeEnum.LIKE,

                            Picture = profile.Data.Photo,
                            FromUserId = profile.Data.SocialChannelId
                        };
                        result.Data.Add(reaction);

                    }
                }
                result.Succeeded = true;
                return result;
            }
            return result;
            //var rez = Task.Run(async () =>
            //{
            //    var httpResponse = await httpClient.GetAsync(apiUrl);
            //    var httpContent = await httpResponse.Content.ReadAsStringAsync();
            //    return httpContent;

            //});

            //var rezJson = JObject.Parse(rez.Result).ToString();
            //dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            //int commentscounts = jsonObject["commentsSummary"]["aggregatedTotalComments"];
            //int likescounts = jsonObject["likesSummary"]["totalLikes"];
            //bool isLikedByAuthor = Convert.ToBoolean(jsonObject["likesSummary"]["likedByCurrentUser"]);



            //LinkedInInsight insight = new LinkedInInsight
            //{
            //    totalComments = commentscounts,
            //    totalLikes = likescounts,
            //    isLikedByAuthor = isLikedByAuthor

            //};

            //return insight;
        }



        [HttpGet("RetrieveLikes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<ReactionsDto>>> RetrieveLikes(string entityUrn, string accesstoken)
        {
            Response<List<ReactionsDto>> result = new Response<List<ReactionsDto>>();
            result.Data = new List<ReactionsDto>();
            var apiUrl = $"https://api.linkedin.com/rest/reactions/(entity:{WebUtility.UrlEncode(entityUrn)})?q=entity";

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var httpResponse = await client.GetAsync(apiUrl);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
             if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseJson = JObject.Parse(httpContent).ToString();
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseJson);

              

                foreach (var element in jsonObject["elements"])
                {
                    string actorUrn = element["lastModified"]["actor"];
                    var reactionType = element["reactionType"];
                  
                    if (actorUrn.StartsWith("urn:li:person"))
                    {
                        int lastColonIndex = actorUrn.LastIndexOf(':');
                        string ActorId = actorUrn.Substring(lastColonIndex + 1);

                        var profile = await _repository.GetLinkedInProfilePicture(accesstoken, ActorId);
                        try {
                            ReactionsDto reaction = new ReactionsDto()
                            {
                                Name = profile.Data.UserName,
                                ReactionType = reactionType,

                                Picture = profile.Data?.CoverPhoto ?? "",
                                FromUserId = profile.Data.ProfileUserId


                            };
                            result.Data.Add(reaction);
                        }
                        catch (Exception ex){
                        }   
                        


                      

                    }
                    else if (actorUrn.StartsWith("urn:li:company"))
                    {
                        int lastColonIndex = actorUrn.LastIndexOf(':');
                        string ActorId = actorUrn.Substring(lastColonIndex + 1);

                        var profile = await _repository.GetCompanyProfile(accesstoken, ActorId);

                        ReactionsDto reaction = new ReactionsDto()
                        {
                            Name = profile.Data.DisplayName,
                            ReactionType = reactionType,

                            Picture = profile.Data.Photo,
                            FromUserId = profile.Data.SocialChannelId


                        };


                        result.Data.Add(reaction);

                    }
                }
                result.Succeeded = true;
                return result;
            }
            else
            {
                result.Succeeded = false;
                result.Message = "Failed To get Likes";
                return result;
            }



           


        }


        [HttpGet("RetrieveLikesbyAutheur")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<ReactionsDto>> RetrieveLikesbyAutheur(string entityUrn, string accesstoken, string authUrn)
        {
            Response<ReactionsDto> result = new Response<ReactionsDto>();
            result.Data = new ReactionsDto();
            var apiUrl = $"https://api.linkedin.com/rest/reactions/(actor:{authUrn},entity:{entityUrn})";

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var httpResponse = await client.GetAsync(apiUrl);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseJson = JObject.Parse(httpContent).ToString();
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseJson);


                    string actorUrn = jsonObject["created"]["actor"];
                    var reactionType = jsonObject["reactionType"];

                    if (actorUrn.StartsWith("urn:li:person"))
                    {
                        int lastColonIndex = actorUrn.LastIndexOf(':');
                        string ActorId = actorUrn.Substring(lastColonIndex + 1);
                        var profile = await _repository.GetLinkedInProfilePicture(accesstoken, ActorId);
                       
                            ReactionsDto reaction = new ReactionsDto()
                            {
                                Name = profile.Data.UserName,
                                ReactionType = reactionType,

                                Picture = profile.Data?.CoverPhoto ?? "",
                                FromUserId = profile.Data.ProfileUserId


                            };
                    result.Data = reaction;
                       
                    }
                    else if (actorUrn.StartsWith("urn:li:company"))
                    {
                        int lastColonIndex = actorUrn.LastIndexOf(':');
                        string ActorId = actorUrn.Substring(lastColonIndex + 1);
                        var profile = await _repository.GetCompanyProfile(accesstoken, ActorId);
                        ReactionsDto reaction = new ReactionsDto()
                        {
                            Name = profile.Data.DisplayName,
                            ReactionType = reactionType,
                            Picture = profile.Data.Photo,
                            FromUserId = profile.Data.SocialChannelId


                        };
                       result.Data = reaction;

                    }
                
                result.Succeeded = true;
                return result;
            }
            else
            {
                result.Succeeded = false;
                result.Message = "Failed To get Likes";
                return result;
            }






        }
      

        /*Retrieve Comments: avoir un rapport sur l'avis des utilisateurs sur le post*/
        [HttpGet("RetrieveComments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<CommentDto>>> RetrieveComments([FromQuery] string PostAPIId, [FromQuery] Guid channelId)
        {
            Response<List<CommentDto>> result = new Response<List<CommentDto>>();
            var linkedinchannel = await GetLinkedinChannel(channelId);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", linkedinchannel.AccessToken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
            var apiUrl = $"https://api.linkedin.com/rest/socialActions/{PostAPIId}/comments"; //post or comment
            string photo =  "";
            var totalLikes = 0;
            var httpResponse = await httpClient.GetAsync(apiUrl);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(httpContent);

                                List<CommentDto> comments = new List<CommentDto>();
                                var length = jsonObject["elements"].Count;
                                foreach (var element in jsonObject["elements"])
                                {

                                                                    //var CommentId = element["id"];
                                                                    //string Activityurn = element["$URN"];
                                                                    //string CommentUrn = element["object"];
                                                                    string ActorUrn = element["actor"];
                                                                        if (element.ContainsKey("likesSummary"))
                                                                        {
                                                                             totalLikes = element["likesSummary"]["totalLikes"];
                                                                            // You can use totalLikes in your logic here
                                                                        }

                                                                    var createdTimeInt = element["created"]["time"];
                                                                    long createdTime = (long)createdTimeInt;
                                                                    DateTime publishTime = DateTimeOffset.FromUnixTimeMilliseconds(createdTime).UtcDateTime;
                                                                    var lastModifiedTime = element["lastModified"]["time"];
                                                                    //var Comment = element["message"]?["text"];

                                                                    if (element["content"] != null)
                                                                    {
                                                                        var contentArray = element["content"][0];

                                                                        if (contentArray != null && contentArray["url"] != null)
                                                                        {
                                                                            photo = contentArray["url"].ToString();
                                                                        }
                                                                    }

                                                                    if (ActorUrn.StartsWith("urn:li:person"))
                                                                    {
                                                                        int lastColonIndex = ActorUrn.LastIndexOf(':');
                                                                        string ActorId = ActorUrn.Substring(lastColonIndex + 1);

                                                                        var profile = await _repository.GetLinkedInProfilePicture(linkedinchannel.AccessToken, ActorId);


                                                                        CommentDto comment = new CommentDto
                                                                        {
                                                                            FromName = profile.Data.UserName,
                                                                            FromPicture = profile.Data.CoverPhoto,
                                                                            LinkUrl = profile.Data.ProfileLink,
                                                                            FromId = profile.Data.ProfileUserId,
                                                                            PhotoUrl = photo,
                                                                            CommentId = element["id"].ToString(),

                                                                            // ActivityUrn = Activityurn,
                                                                            //ActorUrn = ActorUrn,
                                                                            Message = element["message"]?["text"].ToString(),
                                                                            //mediaUrl = url,
                                                                            CreatedTime = publishTime,
                                                                            //Updated_at = lastModifiedTime,
                                                                            Replies = new List<CommentDto>(),
                                                                            Reactions = new List<ReactionsDto>(),
                                                                            CommentUrn = element["$URN"].ToString(),
                                                                            LikesCount = totalLikes,

                                                                        };
                     




                                                                        
                                                                            Response<List<ReactionsDto>> reactions =  await RetrieveSocialActions(element["$URN"].ToString(), linkedinchannel.AccessToken);
                                                                            if (reactions.Succeeded)
                                                                            {
                                                                                comment.Reactions = reactions.Data;
                                                                                // comment.LikesCount = reactions.Data.Count;
                                                                                foreach (var likes in comment.Reactions)
                                                                                {
                                                                                    if (ActorUrn == likes.FromUserId)
                                                                                    {
                                                                                        comment.isLikedByAuthor = true;

                                                                                    }


                                                                                }

                                                                            
                                                                          
                                                                            var replies = await RetrieveSubComments(element["$URN"].ToString(), linkedinchannel.AccessToken, linkedinchannel.Author_urn);
                                                                            comment.Replies = replies.Data;
                                                                            comment.CommentCount = replies.Data.Count;

                                                                        }
                                                                        // comment.ActorUserName = profile.FullName;
                                                                        //comment.Actorprofilelink = profile.LinkedinProfileLink;           
                                                                        //comment.Actorprofilelink = "https://" + profile.LinkedinProfileLink;
                                                                        //comment.Actorheadline = profile.Headline;
                                                                        //comment.PostId = entityUrn;

                                                                        /*Insight*/
                                                                      //  var insight = await RetrieveSocialActions(Activityurn, linkedinchannel.AccessToken);

                                                                        //comment.LikesCount = insight.totalLikes;
                                                                        //comment.CommentCount = insight.totalComments;

                                                                        //  comment.isLikedByAuthor = insight.isLikedByAuthor;


                                                                        comments.Add(comment);


                                                                    }
                                                                    else if (ActorUrn.StartsWith("urn:li:organization"))
                                                                    {
                                                                        int lastColonIndex = ActorUrn.LastIndexOf(':');
                                                                        string ActorId = ActorUrn.Substring(lastColonIndex + 1);

                                                                        var profile = await _repository.GetCompanyProfile(linkedinchannel.AccessToken, ActorId);

                                                                        CommentDto comment = new CommentDto
                                                                        {
                                                                            FromName = profile.Data.DisplayName,
                                                                            FromPicture = profile.Data.Photo,
                                                                            LinkUrl = profile.Data.Link,
                                                                            FromId = profile.Data.SocialChannelId,
                                                                            PhotoUrl = photo,
                                                                            CommentId = element["id"].ToString(),

                                                                            // ActivityUrn = Activityurn,
                                                                            //ActorUrn = ActorUrn,
                                                                            Message = element["message"]?["text"].ToString(),
                                                                            CommentUrn = element["$URN"].ToString(),
                                                                            //mediaUrl = url,
                                                                            CreatedTime = publishTime,
                                                                            Reactions = new List<ReactionsDto>(),
                                                                            LikesCount = totalLikes,
                                                                        //Updated_at = lastModifiedTime,
                                                                        //SubCommentsList= await RetrievePostSubComments(Activityurn,accesstoken)

                                                                         };





                                                                       
                                                                            Response<List<ReactionsDto>> reactions = await RetrieveSocialActions(element["$URN"].ToString(), linkedinchannel.AccessToken);
                                                                            if (reactions.Succeeded)
                                                                            {
                                                                                comment.Reactions = reactions.Data;
                                                                                // comment.LikesCount = reactions.Data.Count;
                                                                                foreach (var likes in comment.Reactions)
                                                                                {
                                                                                    if (ActorUrn == likes.FromUserId)
                                                                                    {
                                                                                        comment.isLikedByAuthor = true;

                                                                                    }


                                                                                }

                                                                            }
                                                                        
                       
                                                                        var replies = await RetrieveSubComments(element["$URN"].ToString(), linkedinchannel.AccessToken, linkedinchannel.Author_urn);
                                                                        comment.Replies = replies.Data;
                                                                        comment.CommentCount = replies.Data.Count;
                    
                                                                        //comment.ActorUserName = profile.FullName;
                                                                        //comment.Actorprofilelink = "https://" + profile.LinkedinProfileLink;
                                                                        //comment.PostId = entityUrn;

                                                                        /*Insight*/
                                                                     //  var insight = await RetrieveSocialActions(Activityurn, linkedinchannel.AccessToken);

                                                                        //comment.LikesCount = insight.totalLikes;
                                                                        //comment.CommentCount = insight.totalComments;


                                                                        //comment.isLikedByAuthor = insight.isLikedByAuthor;

                                                                        comments.Add(comment);

                                                                      }


                                                                               
                                }
                result.Data = comments;
                result.Succeeded = true;
                return result;


            }
                
                result.Succeeded = false;
                result.Message = "failed To Get Comments with status" + httpResponse.StatusCode;
                return result;

               
        }


        [HttpGet("RetrieveSubComments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType] 
        public async Task<Response<List<CommentDto>>> RetrieveSubComments([FromQuery] string entityUrn, [FromQuery] string accesstoken,string Author_urn)
        {
            Response<List<CommentDto>> result = new Response<List<CommentDto>>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
            var apiUrl = $"https://api.linkedin.com/rest/socialActions/{entityUrn}/comments"; //post or comment
            var httpResponse = await httpClient.GetAsync(apiUrl);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(httpContent);
                if (jsonObject["elements"].Count > 0)
                {
                    List<CommentDto> comments = new List<CommentDto>();
                    foreach (var element in jsonObject["elements"])
                    {
                        
                            var CommentId = element["id"];
                            string Activityurn = element["$URN"];
                            string CommentUrn = element["object"];
                            string ActorUrn = element["actor"];
                            var createdTimeInt = element["created"]["time"];
                            long createdTime = (long)createdTimeInt;
                            DateTime publishTime = DateTimeOffset.FromUnixTimeMilliseconds(createdTime).UtcDateTime;
                            var lastModifiedTime = element["lastModified"]["time"];
                            var Comment = element["message"]["text"];
                            if (ActorUrn.StartsWith("urn:li:person"))
                            {
                                int lastColonIndex = ActorUrn.LastIndexOf(':');
                                string ActorId = ActorUrn.Substring(lastColonIndex + 1);

                                var profile = await _repository.GetLinkedInProfilePicture(accesstoken, ActorId);


                                CommentDto comment = new CommentDto
                                {
                                    FromName = profile.Data.UserName,
                                    FromPicture = profile.Data.CoverPhoto,
                                    LinkUrl = profile.Data.ProfileLink,
                                    FromId = profile.Data.ProfileUserId,

                                    CommentId = CommentId,
                                    // ActivityUrn = Activityurn,
                                    //ActorUrn = ActorUrn,
                                    Message = Comment,
                                    LikesCount = 0,
                                    //mediaUrl = url,
                                    CreatedTime = publishTime,
                                    //Updated_at = lastModifiedTime,
                                    Replies = new List<CommentDto>()

                                };

                                // comment.ActorUserName = profile.FullName;
                                //comment.Actorprofilelink = profile.LinkedinProfileLink;           
                                //comment.Actorprofilelink = "https://" + profile.LinkedinProfileLink;
                                //comment.Actorheadline = profile.Headline;
                                //comment.PostId = entityUrn;

                                /*Insight*/
                                //var insight = await RetrieveSocialActions(Activityurn, accesstoken);

                                //comment.LikesCount = insight.totalLikes;
                                //comment.CommentCount = insight.totalComments;
                                // comment.insight.isLikedByAuthor = insight.isLikedByAuthor;

                                if (Activityurn != "")
                                {

                                //likesUser
                                //Response<ReactionsDto> reaction = await RetrieveLikesbyAutheur(entityUrn, accesstoken, profile.Data.ProfileUserId);
                                //if (reaction.Succeeded)
                                //{
                                //    comment.isLikedByAuthor = true;
                                //    comment.Reactions.Add(reaction.Data);

                                //}
                                Response<List<ReactionsDto>> reactions = await RetrieveSocialActions(Activityurn, accesstoken);
                                if (reactions.Succeeded)
                                {
                                    comment.Reactions = reactions.Data;
                                    // comment.LikesCount = reactions.Data.Count;
                                    foreach (var likes in comment.Reactions)
                                    {
                                        if (ActorUrn == likes.FromUserId)
                                        {
                                            comment.isLikedByAuthor = true;

                                        }


                                    }

                                }
                                var replies = await RetrieveSubComments(Activityurn, accesstoken, Author_urn);
                                    comment.Replies = replies.Data;
                                    comment.CommentCount = replies.Data.Count;
                                }
                                comments.Add(comment);


                            }
                            else if (ActorUrn.StartsWith("urn:li:organization"))
                            {
                                int lastColonIndex = ActorUrn.LastIndexOf(':');
                                string ActorId = ActorUrn.Substring(lastColonIndex + 1);

                                var profile = await _repository.GetCompanyProfile(accesstoken, ActorId);

                                CommentDto comment = new CommentDto
                                {
                                    FromName = profile.Data.DisplayName,
                                    FromPicture = profile.Data.Photo,
                                    LinkUrl = profile.Data.Link,
                                    FromId = profile.Data.SocialChannelId,

                                    CommentId = CommentId,
                                    // ActivityUrn = Activityurn,
                                    //ActorUrn = ActorUrn,
                                    Message = Comment,
                                    //mediaUrl = url,
                                    CreatedTime = publishTime,
                                    LikesCount = 0
                                    //Updated_at = lastModifiedTime,
                                    //SubCommentsList= await RetrievePostSubComments(Activityurn,accesstoken)

                                };

                                //  comment.ActorUserName = profile.FullName;
                                //comment.Actorprofilelink = "https://" + profile.LinkedinProfileLink;
                                // comment.PostId = entityUrn;

                                /*Insight*/
                                //var insight = await RetrieveSocialActions(Activityurn, accesstoken);
                                if (Activityurn != "")
                                {
                                   
                                    var replies = await RetrieveSubComments(Activityurn, accesstoken, Author_urn);
                                    comment.Replies = replies.Data;
                                    comment.CommentCount = replies.Data.Count;
                             
                                //likesUser

                                Response<List<ReactionsDto>> reactions = await RetrieveSocialActions(Activityurn, accesstoken);
                                if (reactions.Succeeded)
                                {
                                    comment.Reactions = reactions.Data;
                                    // comment.LikesCount = reactions.Data.Count;
                                    foreach (var likes in comment.Reactions)
                                    {
                                        if (ActorUrn == likes.FromUserId)
                                        {
                                            comment.isLikedByAuthor = true;

                                        }


                                    }

                                }
                            }

                                
                                 // comment.CommentCount = insight.totalComments;
                                //comment.insight.isLikedByAuthor = insight.isLikedByAuthor;

                                comments.Add(comment);

                            }

                    }
                        result.Data = comments;
                        result.Succeeded = true;
                        return result;


                    }
               
                else
                {
                    result.Succeeded = true;
                    result.Data = new List<CommentDto>();
                    return result;
                }
            }
            
            result.Succeeded = false;
            result.Message = "failed To Get Comments with status" + httpResponse.StatusCode;
            return result;

        }
        [HttpGet("RetrievePostSubComments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<List<LinkedInComment>> RetrievePostSubComments(string entityUrn, string accesstoken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/v2/socialActions/{entityUrn}/comments"; //post or comment

            var rez = Task.Run(async () =>
            {

                var httpResponse = await httpClient.GetAsync(apiUrl);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;

            });
            var rezJson = JObject.Parse(rez.Result).ToString();

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            List<LinkedInComment> comments = new List<LinkedInComment>();

            foreach (var element in jsonObject["elements"])
            {
                var CommentId = element["id"];
                var Activityurn = element["$URN"];
                string ActorUrn = element["actor"];
                var createdTime = element["created"]["time"];
                var lastModifiedTime = element["lastModified"]["time"];
                var Comment = element["message"]["text"];
                var parentId = element["parentComment"];


                if (ActorUrn.StartsWith("urn:li:person"))
                {
                    int lastColonIndex = ActorUrn.LastIndexOf(':');
                    string ActorId = ActorUrn.Substring(lastColonIndex + 1);

                    var profile = await _repository.GetMemberProfile(accesstoken, ActorId);


                    LinkedInComment comment = new LinkedInComment
                    {
                        CommentId = CommentId,
                        ActivityUrn = Activityurn,
                        ActorUrn = ActorUrn,
                        Comment = Comment,
                        //mediaUrl = url,
                        Created_at = createdTime,
                        Updated_at = lastModifiedTime,
                        parentId = parentId,

                    };

                    //comment.ActorUserName = profile.FullName;
                    //comment.Actorprofilelink = profile.LinkedinProfileLink;           
                   // comment.Actorprofilelink = "https://" + profile.LinkedinProfileLink;
                   // comment.Actorheadline = profile.Headline;

                    /*Insight*/
                    var insight = await RetrieveSocialActions(comment.ActivityUrn, accesstoken);

                    //comment.insight.totalLikes = insight.totalLikes;
                    //comment.insight.totalComments = insight.totalComments;
                    //comment.insight.isLikedByAuthor = insight.isLikedByAuthor;

                    comments.Add(comment);


                }
                else if (ActorUrn.StartsWith("urn:li:organization"))
                {
                    int lastColonIndex = ActorUrn.LastIndexOf(':');
                    string ActorId = ActorUrn.Substring(lastColonIndex + 1);

                    var profile = await _repository.GetCompanyProfile(accesstoken, ActorId);

                    LinkedInComment comment = new LinkedInComment
                    {
                        CommentId = CommentId,
                        ActivityUrn = Activityurn,
                        ActorUrn = ActorUrn,
                        Comment = Comment,
                        //mediaUrl = url,
                        Created_at = createdTime,
                        Updated_at = lastModifiedTime,
                        parentId=parentId,

                    };
                //    comment.ActorUserName = profile.FullName;
                 //   comment.Actorprofilelink = "https://" + profile.LinkedinProfileLink;

                    /*Insight*/
                    var insight = await RetrieveSocialActions(comment.ActivityUrn, accesstoken);

                    //comment.insight.totalLikes = insight.totalLikes;
                    //comment.insight.totalComments = insight.totalComments;
                    //comment.insight.isLikedByAuthor = insight.isLikedByAuthor;

                    comments.Add(comment);

                }

            }



            return comments;

        }



        [HttpGet("GetAnalyticsData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAnalyticsData([FromQuery] string OrgUrn, [FromQuery] string accesstoken)
        { 
            var views = await GenerateViewData(OrgUrn, accesstoken);
            var comments = await GenerateCommentData(OrgUrn, accesstoken);
            var impressions = await GenerateImpressionData(OrgUrn, accesstoken);
            var share = await GenerateShareData(OrgUrn, accesstoken);

            Analytics analytics = new Analytics
            {
                viewsdata = views,
                commentsdata = comments,
                impressionsdata = impressions,
                sharedata = share
            };

            return Ok(analytics);
        }



        [HttpGet("GenerateViewData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<List<DataPoint>> GenerateViewData(string OrgUrn, string accesstoken)
        {
            DateTime now = DateTime.Now;
            List<DataPoint> viewdata = new List<DataPoint>();

            for (int i = 1; i < 12; i++)
            {
                DateTime monthAgo = now.AddMonths(-i);

                for (int j = 1; j <= 28; j += 3)
                {
                    DateTime date = monthAgo.AddDays(j);
                    DateTime startdate = date;
                    DateTime enddate = date.AddDays(2);

                    int y = await GetViewsTimeBoundStats(OrgUrn, accesstoken, startdate, enddate);

                    viewdata.Add(new DataPoint { X = date, Y = y });
                }
            }

            return viewdata;
        }






        [HttpGet("GenerateCommentData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<List<DataPoint>> GenerateCommentData(string OrgUrn, string accesstoken)
        {
            DateTime now = DateTime.Now;
            List<DataPoint> commentdata = new List<DataPoint>();

            for (int i = 1; i < 3; i++)
            {
                DateTime monthAgo = now.AddMonths(-i);

                for (int j = 1; j <= 28; j += 7)
                {
                    DateTime date = monthAgo.AddDays(j);
                    DateTime startdate = date;
                    DateTime enddate = date.AddDays(6);

                    int y = await GetCommentTimeBoundStats(OrgUrn, accesstoken, startdate, enddate);

                    commentdata.Add(new DataPoint { X = date, Y = y });
                }
            }

            return commentdata;
        }


        [HttpGet("GenerateShareData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<List<DataPoint>> GenerateShareData(string OrgUrn, string accesstoken)
        {
            DateTime now = DateTime.Now;
            List<DataPoint> sharedata = new List<DataPoint>();

            for (int i = 1; i < 3; i++)
            {
                DateTime monthAgo = now.AddMonths(-i);

                for (int j = 1; j <= 28; j += 7)
                {
                    DateTime date = monthAgo.AddDays(j);
                    DateTime startdate = date;
                    DateTime enddate = date.AddDays(6);

                    int y = await GetShareTimeBoundStats(OrgUrn, accesstoken, startdate, enddate);

                    sharedata.Add(new DataPoint { X = date, Y = y });
                }
            }

            return sharedata;
        }


        [HttpGet("GenerateImpressionData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<List<DataPoint>> GenerateImpressionData(string OrgUrn, string accesstoken)
        {
            DateTime now = DateTime.Now;
            List<DataPoint> impressiondata = new List<DataPoint>();

            for (int i = 1; i < 3; i++)
            {
                DateTime monthAgo = now.AddMonths(-i);

                for (int j = 1; j <= 28; j += 7)
                {
                    DateTime date = monthAgo.AddDays(j);
                    DateTime startdate = date;
                    DateTime enddate = date.AddDays(6);

                    int y = await GetImpressionTimeBoundStats(OrgUrn, accesstoken, startdate, enddate);

                    impressiondata.Add(new DataPoint { X = date, Y = y });
                }
            }

            return impressiondata;
        }





        /*Retrieve Followers Statistics: nombre de vrais et faux followers de la page*/
        [HttpGet]
        public async Task<int> GetViewsTimeBoundStats(string OrgUrn, string accesstoken, DateTime startDate, DateTime endDate)
        {
            var start = (long)(startDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var end = (long)(endDate - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationPageStatistics?q=organization&organization={OrgUrn}&timeIntervals.timeGranularityType=DAY&timeIntervals.timeRange.start={start}&timeIntervals.timeRange.end={end}";

            var rez = Task.Run(async () =>
            {

                var httpResponse = await httpClient.GetAsync(apiUrl);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;

            });
            var rezJson = JObject.Parse(rez.Result).ToString();

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            int views = jsonObject["elements"][0]["totalPageStatistics"]["views"]["allPageViews"]["pageViews"];

            return views;
        }


        [HttpGet("GetCommentTimeBoundStats")]
        public async Task<int> GetCommentTimeBoundStats(string OrgUrn, string accesstoken, DateTime startDate, DateTime endDate)
        {
            var start = (long)(startDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var end = (long)(endDate - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={OrgUrn}&timeIntervals.timeGranularityType=DAY&timeIntervals.timeRange.start={start}&timeIntervals.timeRange.end={end}";
            var rez = Task.Run(async () =>
            {

                var httpResponse = await httpClient.GetAsync(apiUrl);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;

            });
            var rezJson = JObject.Parse(rez.Result).ToString();

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            int commentscounts = jsonObject["elements"][0]["totalShareStatistics"]["commentCount"];

            return commentscounts;
        }


        [HttpGet("GetShareTimeBoundStats")]
        public async Task<int> GetShareTimeBoundStats(string OrgUrn, string accesstoken, DateTime startDate, DateTime endDate)
        {
            var start = (long)(startDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var end = (long)(endDate - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={OrgUrn}&timeIntervals.timeGranularityType=DAY&timeIntervals.timeRange.start={start}&timeIntervals.timeRange.end={end}";
            var rez = Task.Run(async () =>
            {

                var httpResponse = await httpClient.GetAsync(apiUrl);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;

            });
            var rezJson = JObject.Parse(rez.Result).ToString();

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            int shareCount = jsonObject["elements"][0]["totalShareStatistics"]["shareCount"];

            return shareCount;
        }

        [HttpGet("GetImpressionTimeBoundStats")]
        public async Task<int> GetImpressionTimeBoundStats(string OrgUrn, string accesstoken, DateTime startDate, DateTime endDate)
        {
            var start = (long)(startDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var end = (long)(endDate - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={OrgUrn}&timeIntervals.timeGranularityType=DAY&timeIntervals.timeRange.start={start}&timeIntervals.timeRange.end={end}";
            var rez = Task.Run(async () =>
            {

                var httpResponse = await httpClient.GetAsync(apiUrl);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;

            });
            var rezJson = JObject.Parse(rez.Result).ToString();

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            int impressionCount = jsonObject["elements"][0]["totalShareStatistics"]["impressionCount"];

            return impressionCount;
        }



        /*nombre de vrais et faux followers acquis pendant une période donnée*/
        [HttpGet("RetrieveFollowersStatisticsWithinaYear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> TimeBoundFollowerStatistics(string OrgURN, string accesstoken, DateTime startDate, DateTime endDate)
        {
            var start = (long)(startDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var end = (long)(endDate - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityFollowerStatistics?q=organizationalEntity&organizationalEntity=urn%3Ali%3Aorganization%3A{OrgURN}&timeIntervals=(timeRange:(start:{start},end:{end}),timeGranularityType:DAY)";

            var rez = Task.Run(async () =>
            {
               
                    var httpResponse = await httpClient.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }






        /*Get statistics of a specific post: nombre de clics, taux d'engagement, impressions, reposts, comments, etc*/
        [HttpGet("GetPostStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<LinkedInInsight> GetPostStatistics(string orgUrn, string postUrn, string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            if(postUrn.StartsWith("urn:li:share"))
            {
                var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={orgUrn}&shares[0]={postUrn}";
                var rez = Task.Run(async () =>
                {

                    var httpResponse = await httpClient.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;

                });

                var rezJson = JObject.Parse(rez.Result).ToString();
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

                var shareCount = jsonObject["elements"][0]["totalShareStatistics"]["shareCount"];
                var clickCount = jsonObject["elements"][0]["totalShareStatistics"]["clickCount"];
                var likeCount = jsonObject["elements"][0]["totalShareStatistics"]["likeCount"];
                var impressionCount = jsonObject["elements"][0]["totalShareStatistics"]["impressionCount"];
                var commentCount = jsonObject["elements"][0]["totalShareStatistics"]["commentCount"];

                LinkedInInsight insight = new LinkedInInsight
                {
                    totalComments = commentCount,
                    totalLikes = likeCount,
                    clickCount = clickCount,
                    impressionCount = impressionCount,
                    shareCount = shareCount
                };

                return insight;
            }
            else if(postUrn.StartsWith("urn:li:ugcPost"))
            {
                var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={orgUrn}&ugcPosts[0]={postUrn}";
                var rez = Task.Run(async () =>
                {

                    var httpResponse = await httpClient.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;

                });

                var rezJson = JObject.Parse(rez.Result).ToString();
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

                if (jsonObject["elements"] is JArray elementsArray && elementsArray.HasValues)
                {
                    var shareCount = jsonObject["elements"][0]["totalShareStatistics"]["shareCount"];
                    var clickCount = jsonObject["elements"][0]["totalShareStatistics"]["clickCount"];
                    var likeCount = jsonObject["elements"][0]["totalShareStatistics"]["likeCount"];
                    var impressionCount = jsonObject["elements"][0]["totalShareStatistics"]["impressionCount"];
                    var commentCount = jsonObject["elements"][0]["totalShareStatistics"]["commentCount"];

                    LinkedInInsight insight = new LinkedInInsight
                    {
                        totalComments = commentCount,
                        totalLikes = likeCount,
                        clickCount = clickCount,
                        impressionCount = impressionCount,
                        shareCount = shareCount
                    };

                    return insight;
                }
                else
                {
                    var shareCount = 0;
                    var clickCount = 0;
                    var likeCount = 0;
                    var impressionCount = 0;
                    var commentCount = 0;

                    LinkedInInsight insight = new LinkedInInsight
                    {
                        totalComments = commentCount,
                        totalLikes = likeCount,
                        clickCount = clickCount,
                        impressionCount = impressionCount,
                        shareCount = shareCount
                    };

                    return insight;
                }

            }
            else
            {
                return null;
            }
            

        }



        /*Get Locations urn*/
        [HttpGet("GetLocationsUrn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetLocationsUrn(string accesstoken, string query)  
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");

            var apiUrl = $"https://api.linkedin.com/v2/geoTypeahead?q=search&query={query}"; //query represents a city, country, continent or a place keyword

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }

        /*Get Seniorities urn*/
        [HttpGet("GetSenioritiesUrn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetSenioritiesUrn(string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var apiUrl = "https://api.linkedin.com/v2/seniorities";

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }

        /*Get skills urn*/
        [HttpGet("GetSkillsUrn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetSkillsUrn(string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var apiUrl = "https://api.linkedin.com/v2/skills?locale.language=en&locale.country=US";

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }

        /*Get industries urn*/
        [HttpGet("GetIndustriesUrn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetIndustriesUrn(string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var apiUrl = "https://api.linkedin.com/v2/industries";

            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(apiUrl);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }



        #region Reaction


        //[HttpPost("CreateReaction")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> CreateReaction([FromBody] LinkedInReactionDto reaction)
        //{
        //    //var linkedInChannel = await _mediator.Send(new GetLinkedInChannelByIdQuery(facebookPost.FacebookChannelId));
        //    var accessToken = "AQUFOc1EJvBJOllBnNj5YOVdtYgJHKjVaUhoLGcizxZoKT6vTytGECvnfnrlolxQafa_iQNl15odxVyZX2KSLOvRrCszl27Ctzq4NI8130b8nXj8u6q5M4m09dZ9nV9KNkFiFVB1nE2Ylgm1V_ko4Ml7eDp97gvH_mGHWqNEDpaTudSpzbT4LfMVz3EuhekCclCCjT6lM1P-eOw9pOiHSTIazFF_iafT4Ef65L5Ynh0q90_SQap2aChzFFXnjSSMymffpdmTHMZtd3GdZrpOZbX_jfF6xS9Ckw2dia3dRi5_0OLhnWL4iRQeuNNfhDNIJnS7mXVgiYNCC7Jo-w22Kw4tm1V-eA";
        //    var status = await _repository.CreateReaction(accessToken, reaction.Actor, reaction.Root);


        //    return Ok(status);
        //}
        //[HttpPost("CreateReaction")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> CreateReaction([FromBody] LinkedInReactionDto reaction)
        //{
        //    //var linkedInChannel = await _mediator.Send(new GetLinkedInChannelByIdQuery(facebookPost.FacebookChannelId));
        //    var accessToken = "AQUFOc1EJvBJOllBnNj5YOVdtYgJHKjVaUhoLGcizxZoKT6vTytGECvnfnrlolxQafa_iQNl15odxVyZX2KSLOvRrCszl27Ctzq4NI8130b8nXj8u6q5M4m09dZ9nV9KNkFiFVB1nE2Ylgm1V_ko4Ml7eDp97gvH_mGHWqNEDpaTudSpzbT4LfMVz3EuhekCclCCjT6lM1P-eOw9pOiHSTIazFF_iafT4Ef65L5Ynh0q90_SQap2aChzFFXnjSSMymffpdmTHMZtd3GdZrpOZbX_jfF6xS9Ckw2dia3dRi5_0OLhnWL4iRQeuNNfhDNIJnS7mXVgiYNCC7Jo-w22Kw4tm1V-eA";
        //    var status = await _repository.CreateReaction(accessToken, reaction.Actor, reaction.Root);


        //    return Ok(status);
        //}
        //[HttpDelete("DeleteReaction")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> DeleteReaction([FromQuery] string accessToken, [FromQuery] string authorUrn, [FromQuery] string entityUrn)
        //{
        //    //var accessToken = "AQUFOc1EJvBJOllBnNj5YOVdtYgJHKjVaUhoLGcizxZoKT6vTytGECvnfnrlolxQafa_iQNl15odxVyZX2KSLOvRrCszl27Ctzq4NI8130b8nXj8u6q5M4m09dZ9nV9KNkFiFVB1nE2Ylgm1V_ko4Ml7eDp97gvH_mGHWqNEDpaTudSpzbT4LfMVz3EuhekCclCCjT6lM1P-eOw9pOiHSTIazFF_iafT4Ef65L5Ynh0q90_SQap2aChzFFXnjSSMymffpdmTHMZtd3GdZrpOZbX_jfF6xS9Ckw2dia3dRi5_0OLhnWL4iRQeuNNfhDNIJnS7mXVgiYNCC7Jo-w22Kw4tm1V-eA";

        //    await _repository.DeleteReaction(accessToken, authorUrn, entityUrn);
        //    return Ok();

        //}
        //[HttpDelete("DeleteReactionµComment")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> DeleteReactionµComment(CommentDetailsDto comment)
        //{
        //    //var accessToken = "AQUFOc1EJvBJOllBnNj5YOVdtYgJHKjVaUhoLGcizxZoKT6vTytGECvnfnrlolxQafa_iQNl15odxVyZX2KSLOvRrCszl27Ctzq4NI8130b8nXj8u6q5M4m09dZ9nV9KNkFiFVB1nE2Ylgm1V_ko4Ml7eDp97gvH_mGHWqNEDpaTudSpzbT4LfMVz3EuhekCclCCjT6lM1P-eOw9pOiHSTIazFF_iafT4Ef65L5Ynh0q90_SQap2aChzFFXnjSSMymffpdmTHMZtd3GdZrpOZbX_jfF6xS9Ckw2dia3dRi5_0OLhnWL4iRQeuNNfhDNIJnS7mXVgiYNCC7Jo-w22Kw4tm1V-eA";

        //    await _repository.DeleteReaction(accessToken, authorUrn, entityUrn);
        //    return Ok();

        //}
        #endregion



    }
}
