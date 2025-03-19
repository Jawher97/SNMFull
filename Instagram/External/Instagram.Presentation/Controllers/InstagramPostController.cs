using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SNM.Instagram.Application.Exceptions.Model;
using SNM.Instagram.Application.Features.Commands.DeleteInstagram;
using SNM.Instagram.Application.Features.Commands.Instagram.Posts;
using SNM.Instagram.Application.Features.Commands.InstagramChannels;
using SNM.Instagram.Application.Features.Commands.InstagramProfile;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SNM.Instagram.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InstagramPostController : Controller
    {
        private readonly IInstagramPublishService _publishService;

      

        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        ///IInstagramPublishService publishService,
        public InstagramPostController(IMediator mediator, IConfiguration config)
        {
            _config = config;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
           // _publishService = publishService;

        }

        [HttpPut, Route("UpdateInstagramPost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdatePostInstaagramCommand command)
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
            var command = new DeleteInstagramPostCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost(Name = "CreateInstagramProfile")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateInstagramProfile([FromBody] CreateInstagramProfileDataCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        }



        //mezelet
        //[HttpPost("InstagramPost")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> InstagramPost(string image_url, string caption)
        //{

        //    var post = await _publishService.PublishPostOnInstagramAsync(image_url, caption);
        //    return Ok(post);
        //}

        //[HttpPost("InstagramVideoPost")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> InstagramVideoPost(string video_url, string caption, string media_type)
        //{

        //    var post = await _publishService.PublishVideoPostOnInstagramAsync(video_url, caption, media_type);
        //    return Ok(post);
        //}

    }
}