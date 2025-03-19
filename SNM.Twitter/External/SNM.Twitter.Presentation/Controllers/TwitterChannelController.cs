using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Features.Commands.TwitterChannels;
using SNM.Twitter.Application.Features.Queries.Twitter;
using SNM.Twitter.Application.Features.Queries.TwitterChannels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SNM.Twitter.Presentation.Controllers
{
    [ApiController]
    // [Route("api/[controller]")]
    [Route("apitwitter/v1/[controller]")]
    public class TwitterChannelController : Controller
    {
        private readonly IMediator _mediator;

    public TwitterChannelController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }


        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<TwitterChannelDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TwitterChannelDto>>> GetAll()
        {
            var getEntities = new GetTwitterChannelsQuery();
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }

        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetById(Guid id)
        {
            var getEntity = new GetTwitterChannelByIdQuery(id);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }

        /*[HttpGet("GetAppAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetBearerToken(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            var token = new GetAccessToken(consumerKey, consumerSecret, accessToken, accessSecret);
            var bearertoken = await token.GetBearerToken();

            return Ok(bearertoken);
        }*/

        [HttpPost("CreateTwitterChannel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateTwiiterChannel([FromBody] CreateTwitterChannelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        }


        [HttpPut, Route("[controller]/Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateTwitterChannelCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete, Route("[controller]/Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteTwitterChannelCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
