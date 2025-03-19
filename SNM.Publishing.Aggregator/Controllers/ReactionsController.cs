using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using System.Net;

namespace SNM.Publishing.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReactionsController : ControllerBase
    {
       
        private readonly IPostFacebookService _postFacebookService;
        private readonly IPostLinkedInService _postLinkedInService;
        private readonly IPostInstagramServices _postInstagramService;
        private readonly IPostTwitterServices _postTwitterService;



        public ReactionsController(
            IPostFacebookService postFacebookService,
            IPostLinkedInService postLinkedInService,
            IPostInstagramServices postInstagramService,
            IPostTwitterServices postTwitterService)
        {

            _postFacebookService = postFacebookService;
            _postLinkedInService = postLinkedInService;
            _postInstagramService = postInstagramService;
            _postTwitterService = postTwitterService;

        }
        [HttpPost("CreateReactionComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> CreateRectionComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<string> result = new Response<string>();
            if (channelTypeName == "LinkedIn")
            {
                var linkedin = await _postLinkedInService.CreateRectionComment(comment, channelId);
                result.Succeeded = linkedin.Succeeded;
              

            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.CreateRectionComment(comment, channelId);
             
                result.Succeeded = facebook.Succeeded;

            }
        
            return result;
        }
        [HttpPost("DeleteReactionComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeleteReactionComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<string> result = new Response<string>();
            if (channelTypeName == "LinkedIn")
            {
                var linkedin = await _postLinkedInService.DeleteReactionComment(comment, channelId);
                result.Succeeded = linkedin.Succeeded;
            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.DeleteReactionComment(comment, channelId);
                result.Succeeded = facebook.Succeeded;

            }
            return result;
        }

        [HttpPost("CreateReactionPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> CreateRectionPost([FromBody] PostDetalisDto post, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<string> result = new Response<string>();
            if (channelTypeName == "LinkedIn")
            {
                var linkedin = await _postLinkedInService.CreateRectionPost(post, channelId);
                result.Succeeded = linkedin.Succeeded;        
            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.CreateRectionPost(post, channelId);
            
                result.Succeeded = facebook.Succeeded;

            }
        

            return result;
        }
        [HttpPost("DeleteReactionPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeleteReactionPost([FromBody] PostDetalisDto post, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<string> result = new Response<string>();
            if (channelTypeName == "LinkedIn")
            {
                var linkedin = await _postLinkedInService.DeleteReactionPost(post, channelId);
                result.Succeeded = linkedin.Succeeded;
            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.DeleteReactionPost(post, channelId);
                result.Succeeded = facebook.Succeeded;

            }
            return result;
        }


    }
}
