using SNM.LinkedIn.Application.Exceptions.Model;


using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Application.Features.Commands.LinkedeIn.Posts;
using SNM.LinkedIn.Application.DTO;

namespace SNM.LinkedIn.Presentation.Controllers
{
    [ApiController]
    [Route("apiLinkedIn/v1/[controller]")]
    public class LinkedInPostController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILinkedInAPIRepository<Guid> _repository;

        public LinkedInPostController(IMediator mediator, ILinkedInAPIRepository<Guid> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }



        //[HttpPost("CreateLinkedInPost")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<ActionResult<string>> CreateLinkedInPost([FromBody] PublishToLinkedInCommand command)
        //{

        //    var publishPost = new PublishToLinkedInCommand();
        //    publishPost.LinkedInChannelPost = command.LinkedInChannelPost;
        //    var entities = await _mediator.Send(publishPost);
        //    return Ok(entities);
        //}





        [HttpPost(Name = "CreateLinkedIn")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<LinkedInPostDto>> CreateLinkedIn([FromBody] CreatePostLinkedinCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdatePostLinkedinCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteLinkedInPostCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
      
    }
}