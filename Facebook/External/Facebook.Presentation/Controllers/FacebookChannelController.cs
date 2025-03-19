using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Exceptions.Model;
using SNS.Facebook.Application.Features.Commands.FacebookChannels;
using SNS.Facebook.Application.Features.Queries.FacebookChannels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SNS.Facebook.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FacebookChannelController : Controller
    {
        private readonly IMediator _mediator;

        public FacebookChannelController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<FacebookChannelDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FacebookChannelDto>>> GetAll()
        {
            var getEntities = new GetFacebookChannelsQuery();
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }
        
        
        [HttpGet("GetAllByBrandId")]
        [ProducesResponseType(typeof(IEnumerable<FacebookChannelDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FacebookChannelDto>>> GetAllByBrandId([FromQuery] Guid id)
        {
            var getEntities = new GetFacebookChannelsByBrandIdQuery(id);
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }


        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetById(Guid id)
        {
            var getEntity = new GetFacebookChannelByIdQuery(id);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }

  

        [HttpPost("CreateFacebookChannel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateFacebookChannel([FromBody] CreateFacebookChannelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        }

        [HttpPut, Route("[controller]/Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateFacebookChannelCommand command)
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
            var command = new DeleteFacebookChannelCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

