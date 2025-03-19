using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using SNS.Facebook.Application.Features.Commands.FacebookAPI;
using SNS.Facebook.Application.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SNS.Facebook.Application.Exceptions.Model;
using static System.Net.WebRequestMethods;
using System.Text;
using SNS.Facebook.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SNS.Facebook.Application.Features.Commands.FacebookChannels;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SNS.Facebook.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FacebookAPIController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _ChannelAPI;
        private readonly string _redirectUriGroup;
        public FacebookAPIController(IMediator mediator, IConfiguration config)
        {
            _config = config;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _clientId = _config.GetValue<string>("FacebookConfiguration:ClientId");
            _clientSecret = _config.GetValue<string>("FacebookConfiguration:ClientSecret");
            _redirectUri = _config.GetValue<string>("FacebookConfiguration:RedirectUri");
            _redirectUriGroup = _config.GetValue<string>("FacebookConfiguration:RedirectUriGroups");
            _ChannelAPI = _config.GetValue<string>("FacebookConfiguration:ChannelAPI");
        }



        /** Generate App Access Token **/
        [HttpGet("Get Facebook page list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<Channel>>> GetFacebookPageList( string AccessToken)
        {
            Response<List<Channel>> result=new Response<List<Channel>>();
            result.Data = new List<Channel>();
            string url = "https://graph.facebook.com/v16.0/me/accounts?fields=id,name,picture,link,access_token&access_token=" + AccessToken;

            var http = new HttpClient();
                
            var httpResponse = await http.GetAsync(url);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                var facebookPages = JsonConvert.DeserializeObject<dynamic>(httpContent);
                //result.Data= 
                foreach (var channel in facebookPages.data) {
                    var PageChannel = new Channel
                    {
                        SocialChannelId  = channel.id,
                        DisplayName = channel.name,
                        SocialAccessToken = channel.access_token,
                        Link = channel.link,                  
                        Photo = channel.picture?.data?.url,

                    };
                    result.Data.Add(PageChannel);
                }
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Message = "Faied to get list Pages";
            }




                return result;
        }

        /** Generate App Access Token **/
        [HttpGet("Get Facebook Groups list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<Channel>>> GetFacebookGroupsList( string AccessToken)
        {
            string url = "https://graph.facebook.com/v16.0/me/groups?admin_only=true&access_token=" + AccessToken;
         
            Response<List<Channel>> result = new Response<List<Channel>>();
            result.Data = new List<Channel>();
            var http = new HttpClient();
            var httpResponse = await http.GetAsync(url);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var facebookGroups = JsonConvert.DeserializeObject<dynamic>(httpContent);


                            //result.Data= 
                            foreach (var group in facebookGroups.data)
                            {

                                string groupurl = $"https://graph.facebook.com/v16.0/{group.id}?fields=picture,name,id,privacy&access_token=" + AccessToken;
                                var httpCient = new HttpClient();
                                var response = await httpCient.GetAsync(groupurl);
                                var Content = await response.Content.ReadAsStringAsync();
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var facebookGroup = JsonConvert.DeserializeObject<dynamic>(Content);
                                        var GroupChannel = new Channel
                                        {
                                            SocialChannelId = facebookGroup.id,
                                            DisplayName = facebookGroup.name,
                                            SocialAccessToken = AccessToken,
                                            Link = $"https://www.facebook.com/groups/{group.id}",
                                            Photo = facebookGroup.picture?.data?.url,

                                        };
                                        result.Data.Add(GroupChannel);
                                    }
                                    result.Succeeded = true;
                            }
                              
                        }
                        else
                        {
                            result.Succeeded = false;
                            result.Message = "Faied to get list Groups";
                        }




            return result;


        }

        /** Generate App Access Token **/
        [HttpGet("GetAppAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAppAccessToken(string APPID, string APPSECRET)
        {

            string url = "https://graph.facebook.com/oauth/access_token?client_id=" + APPID + "&client_secret=" + APPSECRET + "&grant_type=client_credentials";
            var rez = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    var httpResponse = await http.GetAsync(url);
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return httpContent;
                }
            });
            var rezJson = JObject.Parse(rez.Result);

            return Ok(rez.Result);
        }


       

        [HttpGet("GenerateFacebookAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<ChannelProfile>> GenerateFacebookAccessToken([FromQuery] string Code, [FromQuery] Guid brandId, [FromQuery] string name)
        {

            
            Response<ChannelProfile> result = new Response<ChannelProfile>();
            string url = "https://graph.facebook.com/v16.0/oauth/access_token?";
            var postData = new Dictionary<string, string>
                        {
                            { "client_id", _clientId },
                            { "client_secret", _clientSecret },
                            { "code", Code }
                        };
            string CreateProfileUrl = "";
            var http = new HttpClient();
            if (name == "Facebook Page")
            {
                CreateProfileUrl = _config["FacebookConfiguration:CreateProfileUrl"] + name + "&brandId=" + brandId;
                postData["redirect_uri"] = _redirectUri;
            }
            else if (name == "Facebook Group")
            {
                 CreateProfileUrl = _config["FacebookConfiguration:CreateProfileUrl"] + name + "&brandId=" + brandId;
                postData["redirect_uri"] = _redirectUriGroup;
            }
            var httpResponse = await http.PostAsync(url, new FormUrlEncodedContent(postData));
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(httpContent);
                string accesToken = jsonObject["access_token"].ToString();
                //   string userId = jsonObject["user_id"].ToString();
                var resultLivedAccessToken = await GetLongLivedAccessToken(accesToken);
                string access_token = resultLivedAccessToken.Data["access_token"].ToString(); ;
                if (resultLivedAccessToken.Succeeded)
                {
                    var profile = await GetUserFacebookPublicInfos(access_token,name);
                    if (profile.Succeeded != false) {
                        profile.Data.expires_in = resultLivedAccessToken.Data["expires_in"].ToString();
                       
                        var httpClientProfile = new HttpClient();
                        httpClientProfile.BaseAddress = new Uri(_config["FacebookConfiguration:BrandUrl"]);
                        var json = JsonConvert.SerializeObject(profile.Data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await httpClientProfile.PostAsync(CreateProfileUrl, content);
                        var httpContentProfile = await response.Content.ReadAsStringAsync(); // Use await to get the actual string content

                        if (response.IsSuccessStatusCode)
                        {
                            var DataResponse = JsonConvert.DeserializeObject<ChannelProfile>(httpContentProfile);
                            result.Data = DataResponse; result.Succeeded = true;

                            foreach (var item in DataResponse.Channel)
                            {
                                await _mediator.Send(new CreateFacebookChannelCommand
                                {
                                    FacebookChannelDto = new FacebookChannelDto { ChannelAccessToken = item.SocialAccessToken, UserAccessToken = access_token, SocialChannelNetwokId = item.SocialChannelId, ChannelAPI = _ChannelAPI, ChannelId = item.Id }
                                });
                            }
                        }
                    }
                    else
                    {
                        result=profile;
                    }
                    
                    

                }
                else
                {
                    result.Succeeded=false;
                    result.Message = resultLivedAccessToken.Message;
                }
            }

            else
            {
                result.Succeeded = false;
                result.Message = httpContent;
            }

            return result;



        }
        [HttpGet("GetUserFacebookPublicInfos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<ChannelProfile>> GetUserFacebookPublicInfos(string accesToken,string name)
        {
            string url = "https://graph.facebook.com/v16.0/me?fields=id,name,picture,permissions,link&access_token=" + accesToken;

            Response<ChannelProfile> result = new Response<ChannelProfile>();
            List<string> grantedPermissions =new List<string>();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var facebookAccount = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    string facebookId= facebookAccount.id.ToString();
                    foreach(var data in facebookAccount.permissions.data)
                    {
                       
                            grantedPermissions.Add(data.permission.ToString());
                        
                       
                     }

                    var scope = string.Join(", ", grantedPermissions);
                    result.Data = new ChannelProfile
                    {
                        ProfileUserId = facebookId,
                        ProfileLink = facebookAccount.link,
                        UserName = facebookAccount.name,
                        CoverPhoto = facebookAccount.picture?.data?.url,
                        AccessToken = facebookAccount.access_token,
                        Scope= scope,


                    };
                    if(name== "Facebook Page")
                    {
                        Response<List<Channel>> resultChannel = await GetFacebookPageList(accesToken);
                        if (resultChannel.Succeeded)
                        {

                            result.Data.Channel = resultChannel.Data;
                            result.Succeeded = true;
                        }
                        
                    }
                    else if (name == "Facebook Group")
                    {
                        Response<List<Channel>> resultChannel = await GetFacebookGroupsList(accesToken);
                        if (resultChannel.Succeeded)
                        {

                            result.Data.Channel = resultChannel.Data;
                            result.Succeeded = true;
                        }
                        

                    }
                  

                   


                }
                else
                {
                    result.Succeeded = false;
                    result.Message = "Failed To get Page ";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log them, or set an error message in the response.
                result.Succeeded = false;
                result.Message = ex.Message;
            }

            return result;
        }
        /** Generate Long-Lived Access Token **/
        [HttpGet("GetLongLivedAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<Dictionary<string, string>>> GetLongLivedAccessToken(string ShortLivedToken)
        {

            Response<Dictionary<string, string>> result = new Response<Dictionary<string, string>>();
            string url = "https://graph.facebook.com/v16.0/oauth/access_token?grant_type=fb_exchange_token&client_id=" +
                _clientId + "&client_secret=" + _clientSecret + "&set_token_expires_in_60_days=true&fb_exchange_token=" + ShortLivedToken;
            try
            {
                var http = new HttpClient();
                var httpResponse = await http.GetAsync(url);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();


                if (httpResponse.IsSuccessStatusCode)
                {

                    var responseJson = JsonConvert.DeserializeObject<dynamic>(httpContent);

                    result.Succeeded = true;
                    string accessToken = responseJson["access_token"].ToString(); // Convert to string
                    string expir_in = responseJson["expires_in"].ToString();
                    result.Data = new Dictionary<string, string>
                    {
                        {"access_token" ,accessToken},
                         {"expires_in",expir_in},
                    };

                    //   result = await GetRefreshToken(accessToken);


                }
                else
                {
                    result.Succeeded = false;
                    result.Message = "Failed to retrieve Facebook access token.";
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = "An error occurred: " + ex.Message;
            }

            return result;

        }
        


    }
}

