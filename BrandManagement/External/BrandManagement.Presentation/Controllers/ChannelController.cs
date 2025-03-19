using SNM.BrandManagement.Application.Exceptions.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SNM.BrandManagement.Application.Features.Queries.Channels;
using SNM.BrandManagement.Application.Features.Commands.Channels;
using SNM.BrandManagement.Application.Features.Queries.ChannelTypes;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Application.DTO;

namespace SNM.BrandManagement.Presentation.Controllers
{

    [ApiController]
    // [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class ChannelController : Controller
    {
        private readonly IMediator _mediator;

        public ChannelController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(Name = "GetChannel")]
        [ProducesResponseType(typeof(IEnumerable<GetChannelsViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GetChannelsViewModel>>> GetAll()
        {
            var getEntities = new GetChannelsQuery();
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }

        //[HttpGet("{id}", Name = "GetById")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetById(Guid id)
        {
            var getEntity = new GetChannelByIdQuery(id);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }
        [HttpGet("GetByBrandId")]
        //[HttpGet("{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetByBrandId(Guid brandId)
        {
            var getEntities = new GetChannelsByBrandIdQuery(brandId);
            var entities = await _mediator.Send(getEntities);

            return Ok(entities);
        }
        [HttpGet("GetByChannelTypeId")]
        //[HttpGet("{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetByChannelTypeId( Guid channelypeId, Guid brandId)
        {
            var getEntities = new GetChannelByChannelIdQuery(channelypeId,  brandId);
            var entities = await _mediator.Send(getEntities);

            return Ok(entities);
        }



        [HttpPost(Name = "CreateChannel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateChannel([FromBody] CreateChannelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //[HttpPut(Name = "Update")]
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateActivatStatus([FromBody] ChannelDto[] socialChannels)
        {
            foreach (var channel in socialChannels)
            {
               

                await _mediator.Send(new UpdateChannelCommand() { channelDto =channel });
            }
            return NoContent();
        }

        //[HttpDelete("{id}", Name = "Delete")]
        //[HttpDelete("{id:guid}")]
        [HttpDelete, Route("Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteChannelCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete, Route("Delete/Channels")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteChannels([FromBody] List<Channel> channels)
        {
            
            foreach(var channel in channels)
            {
                var command = new DeleteChannelCommand() { Id = channel.Id };
                await _mediator.Send(command);
                
            }
            return NoContent();

        }
    }
}
