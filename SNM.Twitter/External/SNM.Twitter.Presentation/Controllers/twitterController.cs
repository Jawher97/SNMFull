using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Features.Commands.Createtwitter;
using SNM.Twitter.Application.Features.Commands.Deletetwitter;
using SNM.Twitter.Application.Features.Commands.Updatetwitter;
using SNM.Twitter.Application.Features.Queries.Twitter;
using SNM.Twitter.Application.Features.Commands.TwitterAPI;
using Microsoft.Extensions.Configuration;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Domain.Entities;
using SNM.Twitter.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace SNM.Twitter.Presentation.Controllers
{
    [ApiController]
    [Route("apitwitter/v1/[controller]")]
    public class twitterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITwitterAPIRepository<Guid> _repository;
        private readonly IConfiguration _config;
        private ApplicationDbContext _dbContext;

        public twitterController(IMediator mediator, IConfiguration config, ITwitterAPIRepository<Guid> repository, ApplicationDbContext dbContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dbContext = dbContext;

        }

        [HttpGet("GetBearerToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetBearerToken()
        {

            var token = new GetAccessToken(_config);
            var bearertoken = await token.GetBearerToken();

            return Ok(bearertoken);
        }

      

        [HttpPost(Name = "Createtwitter")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> Createtwitter([FromBody] CreatetwitterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        }



        [HttpPost("CreateTwitterPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        /*public async Task<IActionResult> CreateTwitterPost([FromForm] TwitterPostDto post, string oauth_token, string oauth_token_secret)
        {
            var response = await _repository.PublishToTwitter(post, oauth_token, oauth_token_secret);
            return Ok(response);
        }*/
        public async Task<ActionResult<TwitterPost>> CreateTwitterPost([FromBody] Post post)
        {
            var twitterpost = await _repository.PublishToTwitterv2(post);

            await _dbContext.AddAsync(twitterpost);
            await _dbContext.SaveChangesAsync();

            return Ok(twitterpost);

        }

        [HttpGet("GetTwitterPosts")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TwitterPost>>> GetTwitterPosts()
        {
            var all = await _dbContext.TwitterPost.ToListAsync();

            return all;
        }

        [HttpGet("GetUserBanner")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> GetUserBanner(string oauth_token, string oauth_token_secret, string screen_name)
        {
            var response = await _repository.GetUserBanner(oauth_token, oauth_token_secret, screen_name);
            return Ok(response);
        }


        [HttpPost("RetweetPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> RetweetPost(string tweetId, string accessToken)
        {

            var response = await _repository.Retweet(tweetId, accessToken);

            return Ok(response);

        }

        [HttpPut(Name = "Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdatetwitterCommand command)
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
            var command = new DeletetwitterCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}