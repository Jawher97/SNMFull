using Microsoft.AspNetCore.Mvc;
using SNM.Twitter.Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http;
using SNM.Twitter.Infrastructure.Repositories;
using Newtonsoft.Json;
using SNM.Twitter.Application.DTO;
using System.Net.Http.Headers;
using System.Net.Http;
using SNM.Twitter.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using MediatR;
using System.Net;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Features.Commands.TwitterProfileDatas;
using Polly.Retry;
using SNM.Twitter.Domain.Twitter;
using System.Text;
using SNM.Twitter.Application.Features.Commands.Twitter.TwiiterChannels;
using System.Linq;
using System.Collections.Generic;

namespace SNM.Twitter.Presentation.Controllers
{
    [ApiController]
    [Route("apitwitter/v1/[controller]/[action]")]
    public class TwitterOAuthController : ControllerBase
    {
        private readonly ITwitterOAuth2Repository _twitterOAuthRepository;
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;
        private const string AuthorizeEndpoint = "https://twitter.com/i/oauth2/authorize";

        private const string OAuthEndpoint = "https://api.twitter.com/oauth2";
      
        public TwitterOAuthController(ITwitterOAuth2Repository twitterOAuthRepository, IConfiguration config, IMediator mediator)
        {
            _twitterOAuthRepository = twitterOAuthRepository;
            _config = config;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /*[HttpGet("GetCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Authorize()
        {

            // 2. Demander un jeton d'accès temporaire à l'utilisateur tiers.
            var oauth_token = await _twitterOAuthRepository.GetOAuthTokenAsync();

            // 3. Rediriger l'utilisateur tiers vers une page Twitter où il sera invité à autoriser votre application à accéder à son compte.
            var authorizeUrl = $"{AuthorizeEndpoint}?oauth_token={oauth_token}";

            return Ok(oauth_token);

        }

        [HttpGet("GetToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Callback([FromQuery] string oauth_token,[FromQuery] string oauth_verifier)
        {
            // 1. Récupérer les clés d'API et de consommateur de Twitter pour votre application.
            //var consumerKey = _config.GetValue<string>("Twitter:ConsumerKey");
            //var consumerSecret = _config.GetValue<string>("Twitter:ConsumerSecretKey");

            // 4. Récupérer le jeton d'accès permanent pour le compte tiers.
            var userprofile = await _twitterOAuthRepository.GetAccessTokenAsync(oauth_token, oauth_verifier);

            //var accessToken = await _twitterOAuthRepository.ExchangeCodeForTokenAsync(code);

            return Ok(userprofile);
        }*/


        [HttpGet("GetToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Response<ChannelProfile>> GetToken([FromQuery] string code,Guid brandId)
        {
            Response<ChannelProfile> ChannelProfile= new Response<ChannelProfile>();

            var profile = await _twitterOAuthRepository.ExchangeCodeForTokenAsync(code);
            if(profile.Succeeded && profile.Data != null)
            {
                ChannelProfile.Succeeded = true;
                ChannelProfile.Data = profile.Data;
               
                    Channel channel = new Channel()
                    {
                        DisplayName = profile.Data.UserName,
                        Photo = profile.Data.CoverPhoto,
                        BrandId = brandId,
                        Link = profile.Data.ProfileLink,
                        SocialChannelId = profile.Data.ProfileUserId



                    };
                    if (ChannelProfile.Data.channel == null)
                    {
                        ChannelProfile.Data.channel = new List<Channel>();
                    }
                    ChannelProfile.Data.channel.Add(channel);

                

                string CreateProfileUrl = _config["Twitter:CreateProfileUrl"] + "Twitter Profile" + "&brandId=" + brandId;
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_config["Twitter:BrandUrl"]);
                var json = JsonConvert.SerializeObject(ChannelProfile.Data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(CreateProfileUrl, content);
                var httpContent = await response.Content.ReadAsStringAsync(); // Use await to get the actual string content

                if (response.IsSuccessStatusCode)
                {
                    var DataResponse = JsonConvert.DeserializeObject<ChannelProfile>(httpContent);

                    //var GetChannelByChannelProfileId =   await _mediator.Send(new GetChannelByChannelProfileIdCommand{ Id = DataResponse.Id });


                    await _mediator.Send(new CreateTwitterChannelCommand
                    {
                        twitterChannelDto = new TwitterChannelDto
                        {
                            UserAccessToken = profile.Data.AccessToken,
                            TwitterTextAPI = _config?["Twitter:twitterTextApi"],
                            TwitterImageAPI = _config?["Twitter:twitterImageApI"],
                            ConsumerKey = _config?["Twitter:ConsumerKey"],
                            ConsumerSecret = _config?["Twitter:ConsumerSecretKey"],
                            AccessToken = _config?["Twitter:accessToken"],
                            AccessTokenSecret = _config?["Twitter:accessTokenSecret"],
                            ChannelId = DataResponse.channel?.FirstOrDefault()?.Id ?? Guid.Empty
                        }
                    });

                    //  var channelId = DataResponse.Channel.Last().Id;

                    ChannelProfile.Succeeded = true;
                }
                else
                {
                    ChannelProfile.Succeeded = false;
                    ChannelProfile.Message = "Failed To Create Twitter Channel";
                }
            }
            else
            {
                ChannelProfile.Succeeded = false;
                ChannelProfile.Message = profile.Message;
            }
            
            return ChannelProfile;
           // return Ok(profile);
        }

       // [HttpGet("GetUserPublicInfos")]
       //// [ProducesResponseType(StatusCodes.Status200OK)]
       // [ProducesResponseType((int)HttpStatusCode.OK)]
       // [ProducesDefaultResponseType]
       // public async Task<Response<ChannelProfile>> GetUserPublicInfos(string accessToken)
       // {
       //     var httpClient = new HttpClient();

       //     // Configuration de l'URL de l'API Twitter
       //     var url = "https://api.twitter.com/2/users/me?user.fields=created_at,description,entities,id,location,name,pinned_tweet_id,profile_image_url,protected,url,username,verified,withheld";

       //     // Ajouter le jeton d'accès à l'en-tête Authorization
       //     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

       //     // Envoi de la requête à l'API Twitter
       //     var response = await httpClient.GetAsync(url);
       //     var responseContent = await response.Content.ReadAsStringAsync();
       //     dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

       //     string twitterUserId = jsonObject["data"]["id"];
       //     string name = jsonObject["data"]["name"];
       //     string username = jsonObject["data"]["username"];
       //     string imageUrl = jsonObject["data"]["profile_image_url"];
       //     string description = jsonObject["data"]["description"];
       //     TwitterProfileData profile = new TwitterProfileData
       //     {
       //         TwitterUserId = twitterUserId,
       //         Name = name,
       //         UserName = username,

       //     };

           


       //     return profile;
       // }
        [HttpPost(Name = "CreateProfile")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateProfile(CreateTwitterProfileDataCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        }
    }
}

   