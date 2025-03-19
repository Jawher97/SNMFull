using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Exceptions.Model;
using SNM.Instagram.Application.Features.Commands.FacebookChannels;
using SNM.Instagram.Application.Features.Commands.InstagramChannels;
using SNM.Instagram.Application.Features.Queries.InstagramChannels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SNM.Instagram.Presentation.Controllers
{
    [ApiController]
    // [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class InstagramChannelController : Controller
    {
        private readonly IMediator _mediator;

        public InstagramChannelController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<InstagramChannelDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InstagramChannelDto>>> GetAll()
        {
            var getEntities = new GetInstagramChannelsQuery();
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }

        [HttpGet("GetById/{id}")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetById(Guid id)
        {
            var getEntity = new GetInstagramChannelByIdQuery(id);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }




        [HttpPost("CreateInstagramChannel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateInstagramChannel([FromBody] CreateInstagramChannelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        }

        //[HttpPut(Name = "Update")]
        [HttpPut, Route("[controller]/Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateInstagramChannelCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        //[HttpDelete("{id}", Name = "Delete")]
        //[HttpDelete("{id:guid}")]
        [HttpDelete, Route("[controller]/Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteInstagramChannelCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet("GetByChannelId/{channelId}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetByChannelId(Guid channelId)
        {
            var getEntity = new GetInstagramChannelByChannelIdQuery(channelId);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }
        /** Generate Page Access Token **/

        //[HttpGet("GetPageAccessToken")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult> GetPageAccessToken(string PAGEID, string LongLivedAccessToken)
        //{

        //    string url = "https://graph.instagram.com/" + PAGEID + "?fields=access_token&access_token=" + LongLivedAccessToken;

        //    var rez = Task.Run(async () =>
        //    {
        //        using (var http = new HttpClient())
        //        {
        //            var httpResponse = await http.GetAsync(url);
        //            var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //            return httpContent;
        //        }
        //    });
        //    var rezJson = JObject.Parse(rez.Result);


        //    return Ok(rez.Result);
        //}
    }
}
