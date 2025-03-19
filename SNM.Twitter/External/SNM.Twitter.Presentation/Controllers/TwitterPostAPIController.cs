using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Features.Commands.Twitter.Posts;
using SNM.Twitter.Application.Features.Commands.Twitter.twitterAPI;
using SNM.Twitter.Application.Features.Queries.TwitterChannels;
using SNM.Twitter.Application.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace SNM.Twitter.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TwitterPostAPIController : Controller
    {
        

        private readonly IMediator _mediator;
    

        public TwitterPostAPIController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }
        [HttpGet("Get")]
        [ProducesResponseType((int)HttpStatusCode.OK)]


        public async Task<TwitterChannelDto>GetTwitter( [FromQuery] Guid channelId)
        {

           
            var twitterchannelDto = await _mediator.Send(new GetTwitterChannelByIdQuery(channelId));
            return twitterchannelDto;
        }

        [HttpPost("PublishPostToTwitter")]
        [ProducesResponseType((int)HttpStatusCode.OK)]


        public async Task<IActionResult> PublishToTwitter([FromBody] TwitterPostDto twitterPostDto,[FromQuery] Guid channelId)
        {

            var result = new Response<TwitterPostDto>();
            var post = twitterPostDto.PostDto;
            TwitterPostDto social_Post = twitterPostDto;
            var twitterchannelDto = await _mediator.Send(new GetTwitterChannelByIdQuery(channelId));

            if (twitterchannelDto != null)
            {
                TwitterPostDto twitterPot_willCreated = twitterPostDto;
                twitterPot_willCreated.TwitterChannelId = twitterchannelDto.Id;
                twitterPot_willCreated.PostDto = null;
                var Createdpost = await _mediator.Send(new CreatePostTwitterCommand { twitterPostDto = twitterPot_willCreated });
                if (Createdpost.Succeeded && Createdpost.Data != null)
                {

                   
                    social_Post.TwitterChannelDto = twitterchannelDto;
                    social_Post.PostDto = post;
                    var twitterPost_published = await _mediator.Send(new PublishPostToTwitterCommand { TwitterPostDto = social_Post });

                    if (twitterPost_published.Succeeded != false)
                    {

                        Createdpost.Data.PublicationStatus = Domain.Enumeration.PublicationStatusEnum.Published;
                        Createdpost.Data.TwitterPostId = twitterPost_published.Data;
                       var postPublished = await _mediator.Send(new UpdatePostTwitterCommand { TwitterPostDto = Createdpost.Data });


                        if (postPublished.Succeeded && postPublished.Data != null)
                        {
                            result = postPublished;

                        }
                        else
                        {
                           

                            result.Succeeded = false;
                        }
                    }
                    else
                    {
                        result.Message = twitterPost_published.Message;
                        result.Succeeded = false;
                    }
                }
            }
            else
            {

                result.Succeeded = false;
            }
            return Ok(result);

        
    }
}

}