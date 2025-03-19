using Microsoft.AspNetCore.Mvc;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using SNM.Publishing.Aggregator.Services;
using System.Net;

namespace SNM.Publishing.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentController : ControllerBase
    {
       
        private readonly IPostFacebookService _postFacebookService;
        private readonly IPostLinkedInService _postLinkedInService;
        private readonly IPostInstagramServices _postInstagramService;
        private readonly IPostTwitterServices _postTwitterService;



        public CommentController(
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


        [HttpPost("CommentPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<CommentDto>> CommentPost([FromBody] CommentDetailsDto comment , [FromQuery] Guid channelId , [FromQuery] string channelTypeName)
        {
            Response<CommentDto> result = new Response<CommentDto>();
            if (channelTypeName == "LinkedIn")
            {
                
                var linkedin = await _postLinkedInService.CreateComment( comment,channelId);
                result.Succeeded = linkedin.Succeeded;
                result.Data = linkedin.Data;

            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.CreateComment(comment, channelId);
                result.Data=facebook.Data;
                result.Succeeded = facebook.Succeeded;

            }
            if (channelTypeName == "Instagram Profile")
            {
                var instagram = await _postInstagramService.CreateComment(comment, channelId);
                result.Data = instagram.Data;
                result.Succeeded = instagram.Succeeded;

            }
            return result;
        }
        [HttpPost("DeleteComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<string>> DeleteComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<string> result = new Response<string>();
            if (channelTypeName == "LinkedIn" )
            {
                var linkedin = await _postLinkedInService.DeleteComment(comment, channelId);
                result.Succeeded = linkedin.Succeeded;
            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.DeleteComment(comment, channelId);
                result.Succeeded = facebook.Succeeded;

            }
            return result;
        }
        [HttpPut("UpdateComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<CommentDto>> UpdateComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<CommentDto> result = new Response<CommentDto>();
            if (channelTypeName == "LinkedIn")
            {
                var linkedin = await _postLinkedInService.UpdateComment(comment, channelId);
                result.Succeeded = linkedin.Succeeded;
                result.Data= linkedin.Data;
            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.UpdateComment(comment, channelId);
                result.Data = facebook.Data;
                result.Succeeded = facebook.Succeeded;

            }
            return result;
        }
        [HttpPost("CreateSubComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<CommentDto>> CreateSubComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId, [FromQuery] string channelTypeName)
        {
            Response<CommentDto> result = new Response<CommentDto>();
            if (channelTypeName == "LinkedIn")
            {
                var linkedin = await _postLinkedInService.CreateSubComment(comment, channelId);
                result.Succeeded = linkedin.Succeeded;
                result.Data = linkedin.Data;

            }
            if (channelTypeName == "Facebook")
            {
                var facebook = await _postFacebookService.CreateSubComment(comment, channelId);
                result.Data = facebook.Data;
                result.Succeeded = facebook.Succeeded;

            }
            if (channelTypeName == "Instagram Profile")
            {
                var facebook = await _postInstagramService.CreateSubComment(comment, channelId);
                result.Data = facebook.Data;
                result.Succeeded = facebook.Succeeded;

            }
            return result;
        }
    }
}
