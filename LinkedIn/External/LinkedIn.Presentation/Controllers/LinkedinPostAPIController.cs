using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Exceptions.Model;
using SNM.LinkedIn.Application.Features.Queries.LinkedInChannels;
using SNM.LinkedIn.Application.Interfaces;

using System;
using System.Net;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LinkedinPostAPIController : ControllerBase
    {
        private readonly string _versionNumber;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _redirectUri;
        private readonly IMediator _mediator;

        private readonly ILinkedInAPIRepository<Guid> _repository;
        private readonly IConfiguration _config;




        public LinkedinPostAPIController(IMediator mediator,
        IConfiguration config, ILinkedInAPIRepository<Guid> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _config = config;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _clientId = _config.GetValue<string>("LinkedIn:ClientId");
            _clientSecret = _config.GetValue<string>("LinkedIn:ClientSecret");
            _redirectUri = _config.GetValue<string>("LinkedIn:RedirectUri");
            _versionNumber = _config.GetValue<string>("LinkedIn:versionNumber");



        }
        [HttpPost("CreateComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Response<CommentDto>> CreateComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);
            var status = await _repository.CreateComment(comment, entities.AccessToken);

            // Enregistrez tous les commentaires dans la base de données

            return status;
        }

        [HttpPost("DeleteComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);
            await _repository.DeleteComment(comment, entities.AccessToken);

            return Ok();
        }
        [HttpPost("CreateSubComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Response<CommentDto>> CreateSubComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);
            var status = await _repository.CreateSubComment(comment, entities.AccessToken);

            // Enregistrez tous les commentaires dans la base de données

            return status;
        }

        [HttpPut("UpdateComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]


        public async Task<Response<CommentDto>> EditComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);
            var status = await _repository.EditComment(comment, entities.AccessToken);


            return status;
        }

        [HttpPost("CreateReactionPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Response<string>> CreateReaction([FromBody] PostDetalisDto post, [FromQuery] Guid channelId)
        {
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);
            var status = await _repository.CreateReaction(post, entities.AccessToken);


            return status;
        }
        [HttpPost("CreateReactionComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Response<string>> CreateReactionComment(CommentDetailsDto comment, [FromQuery] Guid channelId)
        {

            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);

            var status = await _repository.CreateReactionComment(comment, entities.AccessToken);


            return status;
        }
      
        [HttpPost("DeleteReactionPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Response<string>> DeleteReaction([FromBody] PostDetalisDto post, [FromQuery] Guid channelId)
        {
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);
            var status=  await _repository.DeleteReaction(post, entities.AccessToken);
            return status;

        }
        [HttpPost("DeleteReactionComment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Response<string>> DeleteReactionComment([FromBody] CommentDetailsDto comment, [FromQuery] Guid channelId)
        {
           
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);
             var status= await _repository.DeleteReactionComment(comment, entities.AccessToken);
            return status;

        }
        [HttpPost("DeletePost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePost([FromBody] PostDetalisDto post, [FromQuery] Guid channelId)
        {
            var getEntities = new GetLinkedinChannelbyChannelId(channelId);
            var entities = await _mediator.Send(getEntities);

            await _repository.DeletePostAsync(post, entities.AccessToken);

            return Ok();
        }

    }
}