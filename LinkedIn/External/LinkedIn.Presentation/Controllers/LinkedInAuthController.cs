using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Domain.Entities;
using SNM.LinkedIn.Infrastructure.DataContext;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SNM.LinkedIn.Application.Exceptions.Model;
using Polly.Retry;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using SNM.LinkedIn.Application.Features.Commands.LinkedeIn.LinkedInChannels;
using MediatR;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SNM.LinkedIn.Presentation.Controllers
{
    [ApiController]
    [Route("apiLinkedIn/v1/[controller]/[action]")]
    public class LinkedInAuthController : ControllerBase
    {
        private readonly string _versionNumber;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _redirectUri;
        private readonly IMediator _mediator;

        private readonly ILinkedInAPIRepository<Guid> _repository;
        private readonly IConfiguration _config;
        private ApplicationDbContext _dbContext;



        public LinkedInAuthController(IMediator mediator,
        IConfiguration config, ILinkedInAPIRepository<Guid> repository, ApplicationDbContext dbContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _config = config;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _clientId = _config.GetValue<string>("LinkedIn:ClientId");
            _clientSecret = _config.GetValue<string>("LinkedIn:ClientSecret");
            _redirectUri = _config.GetValue<string>("LinkedIn:RedirectUri");
            _versionNumber = _config.GetValue<string>("LinkedIn:versionNumber");
            _dbContext = dbContext;


        }

        //[HttpGet("AccessToken")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> AccessToken([FromQuery] string code)
        //{
        //    // Configurez l'URL de redirection pour votre application LinkedIn

        //    // Définissez les scopes dont vous avez besoin pour votre application
        //    //string[] scopes = { "openid", "profile",  "email" };
        //    string[] scopes = { "r_liteprofile", "r_emailaddress", "w_member_social", "rw_organization_admin", "r_organization_social", "w_organization_social", "r_organization_admin" };

        //    // Étape 1 : Obtenez le code d'autorisation
        //    var authorizationUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&state=987654321&client_id={_clientId}&redirect_uri={_redirectUri}&scope={string.Join(" ", scopes)}";

        //    Redirect(authorizationUrl);
        //    var tokendata = await _repository.LinkedINAuth(code);

        //    var profile_test = await GetLinkedInProfileId(tokendata.access_token);
        //    tokendata.LinkedInUserId = profile_test.LinkedInUserId;
        //    tokendata.CoverPhoto = profile_test.CoverPhoto;

        //    var memberinfos = await _repository.GetMemberProfile(tokendata.access_token, tokendata.LinkedInUserId);
        //    tokendata.FullName = memberinfos.FullName;
        //    tokendata.LinkedinProfileLink = memberinfos.LinkedinProfileLink;
        //    tokendata.Headline = memberinfos.Headline;
        //    tokendata.Id = Guid.NewGuid().ToString();
        //    tokendata.LinkedinUrn = "urn:li:person:" + tokendata.Id;

        //    await _dbContext.LinkedInProfileData.AddAsync(tokendata);
        //    await _dbContext.SaveChangesAsync();


        //    return Ok(tokendata);

        //}
        [HttpGet("GenerateProfileAccessToken")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<Response<ChannelProfile>> GenerateProfileAccessToken([FromQuery] string code, Guid brandId)
        {
            Response<ChannelProfile> Results = new Response<ChannelProfile>();
            //ChannelProfileDto channelProfileDto= null;
            var linkedInUser = await _repository.LinkedINAuth(code);
            //GetProfileId With Image
            var profileData = await _repository.GetProfileID(linkedInUser.Data.AccessToken);

            if (profileData.Succeeded != false)
            {
                profileData.Data.RefreshToken = linkedInUser.Data.RefreshToken;
                profileData.Data.expires_in = linkedInUser.Data.expires_in;
                profileData.Data.RefreshTokenExpiresIn = linkedInUser.Data.RefreshTokenExpiresIn;
                profileData.Data.Scope = linkedInUser.Data.Scope;

                var Pagedata = await _repository.GetOrgDetails(linkedInUser.Data.AccessToken, brandId);

                var data = (Dictionary<string, object>)Pagedata.Data;
                profileData.Data.Channel = (List<Channel>)data["Channel"];
               // var linkedinChannels = (List<LinkedInChannelDto>)data["LinkedinChannel"];
                if (Pagedata.Succeeded != false)
                {

                    //addChanneProfile
                    string CreateProfileUrl = _config["LinkedIn:CreateProfileUrl"] + "LinkedIn Page" + "&brandId=" + brandId;


                    var httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri(_config["LinkedIn:BrandUrl"]);
                    var json = JsonConvert.SerializeObject(profileData.Data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");


                    HttpResponseMessage response = await httpClient.PostAsync(CreateProfileUrl, content);
                    var httpContent = await response.Content.ReadAsStringAsync(); // Use await to get the actual string content

                    if (response.IsSuccessStatusCode)
                    {
                        var DataResponse = JsonConvert.DeserializeObject<ChannelProfile>(httpContent);

                     


                        var channelId = DataResponse.Channel.FirstOrDefault(linkedinchannel => linkedinchannel.ChannelType.Name == "LinkedIn Profile").Id;

                        await _mediator.Send(new CreateLinkedInChannelCommand
                        {
                            linkedInChannelDto = new LinkedInChannelDto { Id = new Guid(), AccessToken = linkedInUser.Data.AccessToken, Author_urn = profileData.Data.ProfileUserId, ClientId = _clientId, ClientSecret = _clientSecret, ChannelId = channelId }
                        });


                        //// linkedinProfile
                        
                        foreach (var Channel in DataResponse.Channel.Where(linkedinchannel=> linkedinchannel.ChannelType.Name== "LinkedIn Page"))
                        {

                            await _mediator.Send(new CreateLinkedInChannelCommand { linkedInChannelDto = new LinkedInChannelDto { ClientSecret = _clientSecret, Author_urn = "urn:li:organization:" + Channel.SocialChannelId, AccessToken = linkedInUser.Data.AccessToken, ClientId = _clientId, ChannelId = Channel.Id } }
                                );
                       

                        }
                        Results.Succeeded = true;
                        Results.Data = DataResponse;
                        var allChannels = DataResponse.Channel;
                        var allChannelsExceptLast = allChannels.Take(allChannels.Count - 1);

                        Results.Data.Channel = allChannelsExceptLast.ToList();

                    }
                }
                else
                {
                    Results.Succeeded = false;
                    Results.Message = Pagedata.Message;
                }
            }
            else
            {
                Results.Succeeded = false;
                Results.Message = profileData.Message;

            }





            //await _dbContext.LinkedInProfileData.AddAsync(linkedInUser);
            //await _dbContext.SaveChangesAsync();

            //    string LinkedInUserUrl = "https://api.linkedin.com/v2/me";
            //    //var linkedInUserProfile = await ExecuteGetAsync<LinkedInUserDto>(LinkedInUserUrl, linkedInUser.access_token);
            //    linkedInUser.Channels = data;
            return Results;

        }
        private async Task<T> ExecuteGetAsync<T>(string url, string accessToken)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException(
                    $"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

            // Parse the Results
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var results = System.Text.Json.JsonSerializer.Deserialize<T>(content, options);

            if (results == null)
            {
                throw new HttpRequestException(
                    $"Unable to deserialize the response from the HttpResponseMessage: {content}.");
            }

            return results;
        }
        [HttpGet("RefreshAccessToken")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshAccessToken([FromQuery] string refreshtoken)
        {

            var tokens = _repository.refreshAccessToken(refreshtoken);

            return Ok(tokens);

        }

        //[HttpGet("GetLinkedInProfileId")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<Response<LinkedInProfileData>> GetLinkedInProfileId(string accesstoken)
        //{
        //Response<LinkedInProfileData> result = new Response<LinkedInProfileData>();
        //    var data = await _repository.GetProfileID(accesstoken);

        //    return data;
        //}


        //[HttpGet("GetMemberProfileId")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<MemberProfile> GetMemberProfileId(string accesstoken, string person_id)
        //{
        //    var data = await _repository.GetMemberProfile(accesstoken, person_id);

        //    return data;
        //}

        //[HttpGet("GetOrgDetails")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetOrgDetails(string accesstoken)
        //{
        //    var data = await _repository.GetOrgDetails(accesstoken);

        //    return Ok(data);
        //}


        /*Get all posts statistics: vue d'ensemble des statistiques cumulés de tous les posts de la page*/
        [HttpGet("GetAllPostsStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]




        [HttpPut("EditPost")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditPost(string accessToken, string commentary, string postId)
        {

            var status = await _repository.EditPost(accessToken, commentary, postId);

            // Enregistrez tous les commentaires dans la base de données
            await _dbContext.SaveChangesAsync();
            return Ok(status);
        }


        [HttpPost("CreateReshare")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ResharePost([FromQuery] string accessToken, [FromQuery] string author_urn, [FromQuery] string shareId, [FromQuery] string commentary)
        {

            var status = await _repository.ResharePost(accessToken, author_urn, shareId, commentary);

            return Ok(status);
        }


        [HttpPost("DisableComments")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DisableComments([FromQuery] string accessToken, [FromQuery] string shareUrn, [FromQuery] string orgUrn)
        {

            var status = await _repository.DisableCommentsOnCreatedPost(accessToken, shareUrn, orgUrn);

            return Ok(status);
        }


        //[HttpDelete("DeletePost")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> DeletePost([FromQuery] string accessToken, [FromQuery] string postId)
        //{

        //    await _repository.DeletePostAsync(accessToken, postId);

        //    return Ok();
        //}



        //[HttpGet("CreateSubComment")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> CreateSubComment([FromQuery] string accessToken, [FromQuery] string comment, [FromQuery] string author_urn, [FromQuery] string commentUrn)
        //{

        //    var status = await _repository.CreateSubComment(accessToken, comment, author_urn, commentUrn);

        //    // Enregistrez tous les commentaires dans la base de données
        //    await _dbContext.SaveChangesAsync();
        //    return Ok(status);
        //}

        

        //[HttpPut("EditComment")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> EditComment([FromQuery] string accessToken, [FromQuery] string commentary, [FromQuery] string shareUrn, [FromQuery] string commentId, [FromQuery] string actorUrn)
        //{

        //    var status = await _repository.EditComment(accessToken, commentary, shareUrn, commentId, actorUrn);

        //    // Enregistrez tous les commentaires dans la base de données
        //    await _dbContext.SaveChangesAsync();
        //    return Ok(status);
        //}


        //[HttpDelete("DeleteComment")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> DeleteComment([FromQuery] string accessToken, [FromQuery] string shareUrn, [FromQuery] string actorUrn, [FromQuery] string commentId)
        //{

        //    await _repository.DeleteComment(accessToken, shareUrn, actorUrn, commentId);

        //    return Ok();
        //}

        //[HttpPost("CreateReaction")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> CreateReaction([FromQuery] string accessToken, [FromQuery] string authorUrn, [FromQuery] string entityUrn)
        //{

        //    var status = await _repository.CreateReaction(accessToken, authorUrn, entityUrn);


        //    return Ok(status);
        //}



        //[HttpDelete("DeleteReaction")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //public async Task<IActionResult> DeleteReaction([FromQuery] string accessToken, [FromQuery] string authorUrn, [FromQuery] string entityUrn)
        //{

        //    await _repository.DeleteReaction(accessToken, authorUrn, entityUrn);
        //    return Ok();

        //}


        [HttpPost("CreateLinkedInArticle")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostArticleToLinkedIn(string accessToken, string author_id, [FromForm] LinkedInArticleDto article)
        {

            var status = await _repository.CreateArticle(accessToken, author_id, article);

            return Ok(status);
        }

    }




}

