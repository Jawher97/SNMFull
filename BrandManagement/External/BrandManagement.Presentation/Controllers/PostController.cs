using SNM.BrandManagement.Application.Exceptions.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SNM.BrandManagement.Application.Features.Queries.Posts;
using SNM.BrandManagement.Application.Features.Commands.Posts;
using SNM.BrandManagement.Application.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SNM.BrandManagement.Presentation.Controllers
{

    [ApiController]
    // [Route("api/[controller]")]
    [Route("api/v1/[controller]/[action]")]
    public class PostController : Controller
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(typeof(IEnumerable<GetPostsViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GetPostsViewModel>>> GetAll()
        {
            var getEntities = new GetPostsQuery();
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
            var getEntity = new GetPostByIdQuery(id);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }

        [HttpPost(Name = "CreatePost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] PostDto command , [FromQuery] string ChannelTypeString)
        {
            var result = new Response<PostDto>();
            var mediaDto = command.MediaData;
            command.MediaData = null;
            var Createdpost = await _mediator.Send(new CreatePostCommand { PostDto = command });

            if (Createdpost.Succeeded && Createdpost.Data != null && mediaDto.Count()!=0)
            {
                mediaDto.ToList().ForEach(item => item.PostId = Createdpost.Data.Id);

                var mediaData = await _mediator.Send(new CreateMediaCommand { MediaData = mediaDto.ToList() , ChannelTypeString = ChannelTypeString });
                if (mediaData.Succeeded && mediaData.Data != null)
                {
                    Createdpost.Data.MediaData = mediaData.Data;
                    result.Succeeded = true;
                    result.Data = Createdpost.Data;
                }
                else
                {
                    result.Succeeded = false;
                }
            }
            else
            {
                result.Succeeded = true;
                result.Data = Createdpost.Data;
            }
            return Ok(result);

        }


        //[HttpPut(Name = "Update")]
        [HttpPut, Route("[controller]/Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdatePostCommand command)
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
            var command = new DeletePostCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
