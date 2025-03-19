using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Exceptions.Model;
using SNM.Instagram.Application.Features.Commands.CreateInstagram;
using SNM.Instagram.Application.Features.Commands.DeleteInstagram;
//using SNM.Instagram.Application.Features.Commands.InstagramAPI;
using SNM.Instagram.Application.Features.Commands.InstagramProfile;
using SNM.Instagram.Application.Features.Commands.UpdateInstagram;
using SNM.Instagram.Application.Features.Queries.GetEntities;
using SNM.Instagram.Application.Features.Queries.GetEntityById;
using SNM.Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SNM.Instagram.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InstagramController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;

        public InstagramController(IMediator mediator, IConfiguration config)
        {
            _config = config;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _clientId = _config.GetValue<string>("InstagramConfiguration:ClientId");
            _clientSecret = _config.GetValue<string>("InstagramConfiguration:ClientSecret");
            _redirectUri = _config.GetValue<string>("InstagramConfiguration:RedirectUri");
        }

        [HttpGet(Name = "GetInstagram")]
        [ProducesResponseType(typeof(IEnumerable<GetEntitiesViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GetEntitiesViewModel>>> GetAll()
        {
            var getEntities = new GetEntitiesQuery();
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }

        [HttpGet("GetInstagramAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetInstagramAccessToken([FromQuery] string Code)
        {
            Tuple<int, string> res;
            //string clientId = "558877136436062";
            //string clientSecret = "3b5009771ae1305c2e81d0e52c6fff8e";
            //string redirectUri = "https://localhost:4200/callback/";
            string code = Code;
            string url = "https://api.instagram.com/oauth/access_token/";

            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
            { "client_id", "558877136436062" },
            { "client_secret", _clientSecret },
            { "grant_type", "authorization_code" },
            { "redirect_uri", _redirectUri },
            { "code", Code }
        };

                var httpResponse = await http.PostAsync(url, new FormUrlEncodedContent(postData));
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                res = new Tuple<int, string>((int)httpResponse.StatusCode, httpContent);
            }

            var rezJson = JObject.Parse(res.Item2);
            CreateInstagramProfileDataCommand cms = new CreateInstagramProfileDataCommand();
            cms.InstagramProfileDataDto = new InstagramProfileDataDto();
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(res.Item2);
            string accesstoken  = jsonObject["access_token"];
           
            var profile = await GetUserPublicInfos(accesstoken);
            cms.InstagramProfileDataDto= profile;
            cms.InstagramProfileDataDto.InstagramUserId = jsonObject["user_id"]; 
           


          //  var result = CreateInstagramProfile(cms);
            return Content(res.Item2, "application/json");
        }

        //getInstagramProfile(string shortLivedAccessToken)
        [HttpGet("GetInstagramLongLivedAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> GetInstagramLongLivedAccessToken([FromQuery] string shortLivedAccessToken)
        {
            //string clientId = "261825306411994";
            //string clientSecret = "42e2bc47db7429002a2d89b74f51036e";
            string url = $"https://graph.instagram.com/access_token?grant_type=ig_exchange_token&client_secret={_clientSecret}&access_token={shortLivedAccessToken}";

            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.GetAsync(url);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseJson = JObject.Parse(httpContent);
                    var longLivedAccessToken = responseJson.Value<string>("access_token");
                    var expirationInSeconds = responseJson.Value<int>("expires_in");
                    var expirationDate = DateTime.UtcNow.AddSeconds(expirationInSeconds);

                    return Ok(new { Access_Token = longLivedAccessToken, Expiration_Date = expirationDate });
                }

                return BadRequest(httpContent);
            }
        }
    //   ApI
        [HttpDelete("deletecomment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteInstagramComment(string igCommentId)
        {
            string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
            string url = $"https://graph.facebook.com/{igCommentId}?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }

                return BadRequest(content);
            }
        }

        //API
        [HttpGet("GetInstagramMedia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> GetInstagramMedia(string access_token)
        {
            string[] fieldsArray = { "caption", "media_url", "media_type", "permalink", "thumbnail_url", "timestamp", "username" };
            string fields = string.Join(",", fieldsArray);
            string url = $"https://graph.instagram.com/me/media?fields={fields}&access_token={access_token}";

            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.GetAsync(url);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {

                    return Ok(httpContent);

                }

                return BadRequest(httpContent);
            }
        }
        //API
        [HttpGet("commentlike")]

            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesDefaultResponseType]
            public async Task<int> GetInstagramCommentLikes(string igCommentId)
            {
                string apiVersion = "v16.0";
                string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
                string fields = "like_count";
                string url = $"https://graph.facebook.com/{apiVersion}/{igCommentId}?fields={fields}&access_token={accessToken}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);
                        int likeCount = jsonObject["like_count"];

                        return likeCount;
                    }

                    return 0;
                }
            }
        //API

        [HttpGet("comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> GetInstagramComments(string igMediaId)
        {
            string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
            string url = $"https://graph.facebook.com/{igMediaId}/comments?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                List<InstagramComments> comments = new List<InstagramComments>();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);

                    foreach (var item in jsonObject["data"])
                    {
                        string id = item["id"];
                        string text = item["text"];
                        string timestamp = item["timestamp"];

                        InstagramComments comment = new InstagramComments
                        {
                            Id = id,
                            text = text,
                            timestamp = timestamp
                        };
                        var like_count = await GetInstagramCommentLikes(id);
                        comment.like_count = like_count;
                        comments.Add(comment);
                    }

                    return Ok(comments);
                }

                return BadRequest(content);
            }
        }

        //API

        [HttpGet("replies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> GetInstagramCommentReplies(string replies)
        {
            string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
            string url = $"https://graph.facebook.com/{replies}/replies?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                List<InstagramComments> repliesList = new List<InstagramComments>();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);

                    foreach (var item in jsonObject["data"])
                    {
                        string id = item["id"];
                        string text = item["text"];
                        string timestamp = item["timestamp"];

                        InstagramComments reply = new InstagramComments
                        {
                            Id = id,
                            text = text,
                            timestamp = timestamp
                        };
                        repliesList.Add(reply);
                    }

                    return Ok(repliesList);
                }

                return BadRequest(content);
            }
        }

        //API
        [HttpPost("repliesinsta")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> CreateInstagramCommentReply(string igCommentId, [FromQuery] string message)
        {
            string accessToken = "EAADuIPRhw9oBAJys0zRkB4Jq4iniBqgzmUVviMA68ykQVUZBY9ehcY6fqXPwQKNnWa3vVGPDjtbvfZA3SgAgq8oOOZAFr9KpmHwZCN7ZAoyrdIwLEZCxifIq9IhHcyBV6x7UBB7GXb1xriP4j4np2YNqUvZA2vZBeBWExxpkhosvZAxRZBQGAR8QEg";
            string url = $"https://graph.facebook.com/{igCommentId}/replies";

            var parameters = new Dictionary<string, string>
    {
        { "access_token", accessToken },
        { "message", message }
    };

            using (var httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(parameters);
                var response = await httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                return BadRequest(responseContent);
            }
        }



        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetById(Guid id)
        {
            var getEntity = new GetEntityByIdQuery(id);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }

        //[HttpPost("CreateInstagramPost")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<ActionResult<string>> CreateInstagramPost([FromBody] PublishToInstagramCommand command)
        //{
        //    var publishPost = new PublishToInstagramCommand();
        //    publishPost.InstagramChannelPostDto = command.InstagramChannelPostDto;
        //    var entities = await _mediator.Send(publishPost);
        //    return Ok(entities);

        //}

        //[HttpPost(Name = "CreateInstagram")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<ActionResult<Guid>> CreateInstagram([FromBody] CreateInstagramProfileDataCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        //}

        [HttpPut(Name = "Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateInstagramCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        //[HttpDelete("{id}", Name = "Delete")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    var command = new DeleteInstagramCommand() { Id = id };
        //    await _mediator.Send(command);
        //    return NoContent();
        //}

        //[HttpPost(Name = "CreateInstagramProfile")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<ActionResult<Guid>> CreateInstagramProfile([FromBody] CreateInstagramProfileDataCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        //}
        //API
        [HttpGet("GetUserPublicInfos")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<InstagramProfileDataDto> GetUserPublicInfos(string accessToken)
        {
            var httpClient = new HttpClient();

            // Configuration de l'URL de l'API Twitter
            var url = "https://graph.instagram.com/v17.0/me?fields=username,website,biography,media_count,followers_count&" + accessToken;

            // Ajouter le jeton d'accès à l'en-tête Authorization
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Envoi de la requête à l'API Twitter
            var response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

           

            InstagramProfileDataDto profile = new InstagramProfileDataDto
            {
                Id = Guid.NewGuid(),
                InstagramUserId = jsonObject["id"],
                AccessToken = accessToken,
                Website = jsonObject["website"],
                UserName = jsonObject["username"],
                Biography = jsonObject["biography"],
                MediaCount = jsonObject["media_count"],
                FollowersCount = jsonObject["followers_count"],

            };

          
            return profile;
        }
    }
}