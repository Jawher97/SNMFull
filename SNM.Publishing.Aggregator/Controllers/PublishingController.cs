using Microsoft.AspNetCore.Mvc;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using SNM.Publishing.Aggregator.Models.Enumeration;
using System.Linq.Expressions;
using System.Net;


namespace SNM.Publishing.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class PublishingController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IPostFacebookService _postFacebookService;
        private readonly IPostLinkedInService _postLinkedInService;
        private readonly IPostInstagramServices _postInstagramService;
        private readonly IPostTwitterServices _postTwitterService;



        public PublishingController(IBrandService brandService, 
            IPostFacebookService postFacebookService, 
            IPostLinkedInService postLinkedInService, 
            IPostInstagramServices postInstagramService, 
            IPostTwitterServices postTwitterService)
        {
            _brandService = brandService;
            _postFacebookService = postFacebookService;
            _postLinkedInService = postLinkedInService;
            _postInstagramService = postInstagramService;
            _postTwitterService= postTwitterService;

        }

        [HttpPost(Name = "PublishPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<string>>> PublishPost([FromBody]GenericPublishingPost genericPost)
        {
            var post =  PostDto.FromGenericModel(genericPost);
            var postGlobal = await _brandService.CreatePost(post , genericPost?.SocialChannels);
            Response<List<string>> resp = new Response<List<string>>();
            resp.Data = new List<string>();
            DateTime now = DateTime.Now;

            if (postGlobal.Succeeded && postGlobal.Data != null)
            {
                genericPost.PostId = postGlobal.Data.Id;
                
                    foreach (ChannelDto socialChannel in genericPost?.SocialChannels)
                    {
                        switch (socialChannel?.ChannelType?.Name)
                        {
                        //Facebook
                        case "Facebook Group":
                        case "Facebook Page":
                                {
                                    var facebooPost = FacebookPostDto.FromGenericModel(genericPost);
                                    facebooPost.Post = postGlobal.Data;
                                
                                if (postGlobal.Data.PublicationDate> now)
                                {
                                    var postfaceook = await _postFacebookService.PublishScheduleFacebookPost(facebooPost, socialChannel.Id);
                                    resp.Succeeded = postfaceook.Succeeded;
                                    resp.Data.Add("Post Facebook Published Schedule Successfully");
                                }
                                else
                                {
                                    var lc = await _postFacebookService.PublishFacebookPost(facebooPost, socialChannel.Id);
                                    resp.Succeeded = lc.Succeeded;
                                    resp.Data.Add("Post Facebook Published  Successfully");
                                }
                                   
                                break;
                                }
                        
                        case "LinkedIn Profile":                           
                        case "LinkedIn Page":
                            {
                                var linkedinPost = LinkedInPostDto.FromGenericModel(genericPost);
                                linkedinPost.PostDto = postGlobal.Data;
                                if (postGlobal.Data.PublicationDate > now)
                                {
                                   

                                    var lc = await _postLinkedInService.PublishSchedulePostLinkedIn(linkedinPost, socialChannel.Id);
                                    resp.Succeeded = lc.Succeeded;
                                    resp.Data.Add("Post LinkedIn Published Schedule Successfully");
                                }
                                else
                                {
                                    var lc = await _postLinkedInService.PublishPostLinkedIn(linkedinPost, socialChannel.Id);
                                    resp.Succeeded = lc.Succeeded;
                                    resp.Data.Add("Post LinkedIn Published  Successfully");
                                }
                                    

                            
                                    //LinkedIn
                                    
                               
                                break;
                            }


                        case "Instagram Profile":
                            {
                               
                               
                                var instagramPost = InstagramPostDto.FromGenericModel(genericPost);
                                instagramPost.PostDto = postGlobal.Data;
                                if (postGlobal.Data.PublicationDate > now)
                                {


                                    var lc = await _postInstagramService.PublishSchedulePostInstagram(instagramPost, socialChannel.Id);
                                    resp.Succeeded = lc.Succeeded;
                                    resp.Data.Add("Post Instagram Published Schedule Successfully");
                                }
                                else
                                {
                                    var lc = await _postInstagramService.PublishInstagramPost(instagramPost, socialChannel.Id);
                                    resp.Succeeded = lc.Succeeded;
                                    resp.Data.Add("Post Instagram Published  Successfully");
                                }
                                break;
                            }
                        case "Twitter Profile":
                            {
                                var twitterPost = TwitterPostDto.FromGenericModel(genericPost);
                                twitterPost.PostDto = postGlobal.Data;
                                if (postGlobal.Data.PublicationDate > now)
                                {
                                    var lc = await _postTwitterService.PublishSchedulePostTwitter(twitterPost, socialChannel.Id);
                                    resp.Succeeded = lc.Succeeded;
                                    resp.Data.Add("Post Twitter Published Schedule Successfully");
                                }
                                else
                                {
                                   

                                    var lc = await _postTwitterService.PublishTwitterPost(twitterPost, socialChannel.Id);
                                    resp.Succeeded = lc.Succeeded;
                                    resp.Data.Add("Post Twitter Published Successfully");
                                }
                                break;
                            }
                       default: { 

                                resp.Data.Add("No Channel Exist");
                                break;
                            }

                        }
                    
                }
             
            }
            

            return resp;
        }
        
        [HttpPost(Name = "DeletePost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeletePost([FromBody] PostDetalisDto post, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<string> result = new Response<string>();
            if (channelTypeName == "LinkedIn")
            {
                var linkedin = await _postLinkedInService.DeletePost(post, channelId);
                result.Succeeded = linkedin.Succeeded;
            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.DeletePost(post, channelId);
                result = facebook;

            }
            return result;
        }
    }
}
