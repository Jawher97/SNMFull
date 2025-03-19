using Microsoft.AspNetCore.Mvc;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using System.Net;

namespace SNM.Publishing.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GetLastPostsController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IPostFacebookService _postFacebookService;
        private readonly IPostLinkedInService _postLinkedInService;
        private readonly IPostInstagramServices _postInstagramService;
        private readonly IPostTwitterServices _postTwitterService;



        public GetLastPostsController(IBrandService brandService,
            IPostFacebookService postFacebookService,
            IPostLinkedInService postLinkedInService,
            IPostInstagramServices postInstagramService,
            IPostTwitterServices postTwitterService)
        {
            _brandService = brandService;
            _postFacebookService = postFacebookService;
            _postLinkedInService = postLinkedInService;
            _postInstagramService = postInstagramService;
            _postTwitterService = postTwitterService;

        }


        [HttpPost("GetLastPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<PostDetalisDto>>> GetLastPost([FromBody] List<ChannelDto> channels)
        {
            Response<List<PostDetalisDto>> resp = new Response<List<PostDetalisDto>>();
            resp.Data = new List<PostDetalisDto>();

            foreach (var socialChannel in channels)
            {
               switch (socialChannel?.ChannelType?.Name)
                {
                    //Facebook
                    case "Facebook Group":
                    case "Facebook Page":

                        {
                            var lc = await _postFacebookService.GeFacebookChannels(socialChannel);

                         
                            if (lc.Succeeded)
                            {
                                lc.Data.ChannelTypeName = "Facebook";
                                lc.Data.Photo = socialChannel.Photo;
                                lc.Data.Name = socialChannel.DisplayName;
                                lc.Data.ChannelId = socialChannel.Id;
                                resp.Data.Add(lc.Data);
                            }
                          
                            break;
                        }

                    case "Instagram Profile":
                        {
                            var instagram = await _postInstagramService.GeInstagramChannels(socialChannel);
                            if (instagram.Succeeded)
                            {
                                instagram.Data.ChannelTypeName = "Instagram Profile";
                                instagram.Data.Photo = socialChannel.Photo;
                                instagram.Data.Name = socialChannel.DisplayName;
                                instagram.Data.ChannelId = socialChannel.Id;
                                resp.Data.Add(instagram.Data);
                            }
                          
                            break;
                        }
                    case "LinkedIn Page":
                        {
                            var linkedin = await _postLinkedInService.GeLinkedinChannels(socialChannel);
                          
                            if (linkedin.Succeeded)
                            {
                                linkedin.Data.ChannelTypeName = "LinkedIn";
                                linkedin.Data.Photo = socialChannel.Photo;
                                linkedin.Data.Name = socialChannel.DisplayName;
                                linkedin.Data.ChannelId=socialChannel.Id;
                                resp.Data.Add(linkedin.Data);
                            }
                           
                            
                            break;
                        }
                        //else if (socialChannel?.ChannelType?.Name == "Twitter Profile")
                        //{
                        //    var twitter = await _postTwitterService.GeTwitterChannels(socialChannel);
                        //    PostDetalisDto postdetails = new PostDetalisDto
                        //    {
                        //        Caption = twitter.Data.Caption,
                        //        PublicationDate = twitter.Data.PublicationDate,
                        //        MediaData = twitter.Data.MediaData,
                        //        Photo = socialChannel.Photo,
                        //        Name = socialChannel.DisplayName,
                        //        Comments = twitter.Data.Comments,
                        //        Reactions = twitter.Data.Reactions,
                        //        PostEngagedUsers = twitter.Data.PostEngagedUsers,
                        //        PostClicks = twitter.Data.PostClicks,
                        //        TotalCountReactions = twitter.Data.TotalCountReactions,
                        //    };
                        //    resp.Data.Add(postdetails);
                        //}
                }


            }
            resp.Succeeded = true;

            return resp;
        }
    }

} 

