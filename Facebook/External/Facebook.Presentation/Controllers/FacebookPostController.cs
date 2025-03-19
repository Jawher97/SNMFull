using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using SNS.Facebook.Application.Features.Commands.Posts;
using SNS.Facebook.Application.Exceptions.Model;

namespace SNS.Facebook.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FacebookPostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacebookPostController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<GetEntitiesViewModel>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<IEnumerable<GetEntitiesViewModel>>> GetFacebookPosts()
        //{
        //    var getEntities = new GetEntitiesQuery();
        //    var entities = await _mediator.Send(getEntities);
        //    return Ok(entities);
        //}

        //[HttpGet("{id}", Name = "GetById")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult> GetById(Guid id)
        //{
        //    var getEntity = new GetEntityByIdQuery(id);
        //    var entity = await _mediator.Send(getEntity);

        //    return Ok(entity);
        //}



        /* Create Facebook Post in DataBase*/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> CreateFacebookPost([FromBody] CreateFacebookPostCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid?>(result.Data.Id, $"Successfully created with Id: {result}"));
        }


        /* Update Facebook Post in DataBase*/
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateFacebookPost([FromBody] UpdateFacebookPostCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }


        /* Delete Facebook Post in DataBase*/
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteFacebookPost(Guid id)
        {
            var command = new DeleteFacebookPostCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }


     


      



    }

}