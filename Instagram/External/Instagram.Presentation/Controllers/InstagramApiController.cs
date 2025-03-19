using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Features.Commands.InstagramProfile;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Http.Headers;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Application.Exceptions.Model;
using AutoMapper;
using System.Text;
using SNM.Instagram.Application.Features.Commands.InstagramChannels;
using System.Linq;

namespace SNM.Instagram.Presentation.Controllers
{
    [Route("api/v1/[controller]")]
    public class InstagramAPIController : Controller
    {


        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _apiAccessToken;
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;
        private readonly string _ChannelAPI;

        public InstagramAPIController(IMediator mediator, IConfiguration config)
        {
            _config = config;
           
            _clientId = _config.GetValue<string>("InstagramConfiguration:ClientId");
            _clientSecret = _config.GetValue<string>("InstagramConfiguration:ClientSecret");
            _redirectUri = _config.GetValue<string>("InstagramConfiguration:RedirectUri");
            _apiAccessToken = _config.GetValue<string>("InstagramConfiguration:APIAcessToken");
            _ChannelAPI = _config.GetValue<string>("InstagramConfiguration:ChannelAPI");
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        //[HttpGet("GetInstagramAccessToken")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<Response<ChannelProfile>> GetInstagramAccessToken([FromQuery] string Code, [FromQuery] string brandId)
        //{
        //    var result = new Response<ChannelProfile>();

        //    string url = _apiAccessToken;
        //    var httpClient = new HttpClient();

        //    var postData = new Dictionary<string, string>
        //            {
        //                { "client_id",  _clientId },
        //                { "client_secret", _clientSecret },
        //                { "grant_type", "authorization_code" },
        //                { "redirect_uri", _redirectUri },
        //                { "code", Code }
        //            };

        //    var httpResponse = await httpClient.PostAsync(url, new FormUrlEncodedContent(postData));
        //    var httpContent = await httpResponse.Content.ReadAsStringAsync();
        //    if (httpResponse.IsSuccessStatusCode)
        //    {
        //        //var httpContent = await httpResponse.Content.ReadAsStringAsync();
        //        var jsonObject = JsonConvert.DeserializeObject<dynamic>(httpContent);
        //        string accesToken = jsonObject["access_token"];
        //        string userId= jsonObject["user_id"].ToString();
        //        var resultLivedAccessToken = await GetInstagramLongLivedAccessToken(accesToken);

        //        if (resultLivedAccessToken.Succeeded)
        //        {
        //            var profile = await GetUserPublicInfos(accesToken, userId);
        //            string CreateProfileUrl = _config["InstagramConfiguration:CreateProfileUrl"] + "Instagram Profile" + "&brandId=" + brandId;

        //            var httpClientProfile = new HttpClient();
        //            httpClientProfile.BaseAddress = new Uri(_config["InstagramConfiguration:BrandUrl"]);
        //            var json = JsonConvert.SerializeObject(profile.Data);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            HttpResponseMessage response = await httpClientProfile.PostAsync(CreateProfileUrl, content);
        //            var httpContentProfile = await response.Content.ReadAsStringAsync(); // Use await to get the actual string content

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var DataResponse = JsonConvert.DeserializeObject<ChannelProfile>(httpContentProfile);
        //                profile.Data = DataResponse;
        //                await _mediator.Send(new CreateInstagramChannelCommand
        //                {
        //                    InstagramChannelDto = new InstagramChannelDto { UserAccessToken = accesToken, UserId = profile.Data.ProfileUserId, ChannelAPI = _clientId, ChannelId = DataResponse.SocialChannels.First().Id }
        //                });
        //                return profile;
        //            }

        //        }
        //        else
        //        {
        //            result.Message = resultLivedAccessToken.Message;
        //        }
        //    }
        //    else
        //    {
        //        result.Succeeded = false;
        //    }

        //    return result;
        //}


        [HttpGet("GetInstagramAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<ChannelProfile>> GetInstagramAccessToken([FromQuery] string Code, [FromQuery] Guid brandId)
        {


            Response<ChannelProfile> result = new Response<ChannelProfile>();

            string url = "https://graph.facebook.com/v16.0/oauth/access_token?";

            var http = new HttpClient();

            var postData = new Dictionary<string, string> {
                    { "client_id", _clientId },
                    { "redirect_uri", _redirectUri },
                    { "client_secret", _clientSecret },

                    { "code", Code }
                };

            var httpResponse = await http.PostAsync(url, new FormUrlEncodedContent(postData));
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(httpContent);
                string accesToken = jsonObject["access_token"].ToString();
                //   string userId = jsonObject["user_id"].ToString();
                var resultLivedAccessToken = await GetLongLivedAccessToken(accesToken);
                string access_token = resultLivedAccessToken.Data["access_token"];
                if (resultLivedAccessToken.Succeeded)
                {
                    Response<ChannelProfile> FacebookProfile = await GetUserFacebookPublicInfos(access_token);
                   
                    var profile = await GetUserInstagramPublicInfos(access_token);
                    if(profile.Succeeded)
                    {
                        FacebookProfile.Data.Channel = profile.Data;
                        FacebookProfile.Data.expires_in = resultLivedAccessToken.Data["expires_in"].ToString();

                        FacebookProfile.Data.AccessToken = access_token;
                        string CreateProfileUrl = _config["InstagramConfiguration:CreateProfileUrl"] + "Instagram Profile" + "&brandId=" + brandId;
                  
                        var httpClientProfile = new HttpClient();
                        httpClientProfile.BaseAddress = new Uri(_config["InstagramConfiguration:BrandUrl"]);
                        var json = JsonConvert.SerializeObject(FacebookProfile.Data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await httpClientProfile.PostAsync(CreateProfileUrl, content);
                        var httpContentProfile = await response.Content.ReadAsStringAsync(); // Use await to get the actual string content

                        if (response.IsSuccessStatusCode)
                        {
                            var DataResponse = JsonConvert.DeserializeObject<ChannelProfile>(httpContentProfile);
                            result.Data = DataResponse;
                            result.Succeeded = true;
                            foreach (var channel in DataResponse.Channel)
                            {
                                await _mediator.Send(new CreateInstagramChannelCommand
                                {
                                    InstagramChannelDto = new InstagramChannelDto { UserAccessToken = accesToken, UserId = channel.SocialChannelId, ChannelAPI = _ChannelAPI, ChannelId = channel.Id }
                                });
                            }


                        }
                    }
                   else
                    {
                        result.Message = profile.Message;
                        result.Succeeded = false;
                    }
                



                }
                else
                {
                    result.Succeeded = false;

                    result.Message = resultLivedAccessToken.Message;
                }
            }

            else
            {
                result.Succeeded = false;
            }

            return result;



        }
        [HttpGet("GetUserFacebookPublicInfos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<ChannelProfile>> GetUserFacebookPublicInfos(string accesToken)
        {
            string url = "https://graph.facebook.com/v16.0/me?fields=id,name,picture,permissions,link&access_token=" + accesToken;

            Response<ChannelProfile> result = new Response<ChannelProfile>();
            List<string> grantedPermissions = new List<string>();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var facebookAccount = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    string facebookId = facebookAccount.id.ToString();
                    foreach (var data in facebookAccount.permissions.data)
                    {
                        grantedPermissions.Add(data.permission.ToString());

                    }
                    // string url = "https://graph.facebook.com/v16.0/me/albums?fields=picture&access_token=" + accesToken;
                    string Pictureurl = $"https://graph.facebook.com/v16.0/{facebookId}/picture?type=square&redirect=false&access_token=" + accesToken;
                    var Client = new HttpClient();
                    var httpresponse = await Client.GetAsync(Pictureurl);
                    var Content = await response.Content.ReadAsStringAsync();
                    var allbums = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    //albums.data[0].url;

                    var scope = string.Join(", ", grantedPermissions);
                    result.Data = new ChannelProfile
                    {
                        ProfileUserId = facebookId,
                        ProfileLink = facebookAccount.link,
                        UserName = facebookAccount.name,
                        CoverPhoto = facebookAccount.picture?.data?.url,
                        AccessToken = facebookAccount.access_token,
                        Scope = scope,


                    };
                   result.Succeeded = true;
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
        [HttpGet("GetUserInstagramPublicInfos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<List<Channel>>> GetUserInstagramPublicInfos(string accesToken)
        {
            string url = "https://graph.facebook.com/v16.0/me/accounts?access_token=" + accesToken;

            Response<List<Channel>> result = new Response<List<Channel>>();
            result.Data = new List<Channel>();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var listpage = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    //getPageId

                    var PageId = listpage.data[0].id.ToString();
                    // if i have multiple page so i have multiple instagram account 
                    foreach (var page in listpage.data)
                    {
                        string urlGetIdConnection = $"https://graph.facebook.com/v16.0/{page.id.ToString()}?fields=instagram_business_account&access_token={accesToken}"; ;
                        var Client = new HttpClient();
                        var httpresponse = await Client.GetAsync(urlGetIdConnection);
                        var httpresponseContent = await httpresponse.Content.ReadAsStringAsync();
                        if (httpresponse.IsSuccessStatusCode)
                        {
                            var instagramaccountId = JsonConvert.DeserializeObject<dynamic>(httpresponseContent);
                            var instagramIdConnection = instagramaccountId.instagram_business_account.id.ToString();
                            var getInstagramBusinessAccount = await GetInstagramBusinessAccount(instagramIdConnection, accesToken);
                            //if (result.Data.Channel == null)
                            //{
                            //result.Data.Channel = new List<Channel>();
                            //}
                            result.Data.Add(getInstagramBusinessAccount.Data);


                        }
                    }
                    result.Succeeded = true;

                }
                else
                {
                    result.Succeeded = false;
                    result.Message = "Failed To get Page ";
                }
            }
            catch (Exception ex)
            {
                
                result.Succeeded = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpGet("GetInstagramBusinessAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<Response<Channel>> GetInstagramBusinessAccount(string instagramIdConnection, string accesToken)
        {
            Response<Channel> result = new Response<Channel>();
            string url = $"https://graph.facebook.com/v16.0/{instagramIdConnection}?fields=username,biography,profile_picture_url,name,media_count,followers_count,follows_count,website&access_token=" + accesToken;
            var Client = new HttpClient();
            var httpresponse = await Client.GetAsync(url);
            var httpresponseContent = await httpresponse.Content.ReadAsStringAsync();
            if (httpresponse.IsSuccessStatusCode)
            {
                var profile = JsonConvert.DeserializeObject<dynamic>(httpresponseContent);

                result.Succeeded = true;
                result.Data = new Channel
                {
                    SocialChannelId = profile.id,
                    SocialAccessToken = accesToken,
                    Link = $"https://www.instagram.com/{profile.username}" ,

                    //Headline = profile.biography,
                    //CoverPhoto = profile.profile_picture_url,
                    DisplayName = profile.username,
                    Photo = profile.profile_picture_url,
                    // IsActivated=Domain.Enumeration.ActivationStatus.Activate

                };

            }
            else
            {
                result.Succeeded = false;
                result.Message = "Failed to get Instagram Account";

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


        //[HttpGet("GetUserPublicInfos")]
        //// [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<Response<ChannelProfile>> GetUserPublicInfos(string accesToken, string userid)
        //{
        //    var result = new Response<ChannelProfile>();
        //    var httpClient = new HttpClient();

        //    // string url = $" https://graph.facebook.com/v3.2/{userid}?fields=business_discovery.username(art_benr){{name,link,picture,id}}&access_token={accesToken}";
        //    string url = $" https://www.instagram.com/art_benr/?__a=1&access_token={accesToken}";


        //    try
        //    {
        //        var response = await httpClient.GetAsync(url);
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var profile = JsonConvert.DeserializeObject<dynamic>(responseContent);

        //            result.Succeeded = true;
        //            result.Data = new ChannelProfile
        //            {
        //                ProfileUserId = profile.id,
        //                AccessToken = accesToken,
        //                ProfileLink = profile.Website,
        //                UserName = profile.username,
        //                Headline = profile.Biography,
        //                CoverPhoto = profile.MediaCount,
        //                //expires_in = data["Expiration_Date"]
        //            };

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions, log them, or set an error message in the response.
        //        result.Succeeded = false;
        //        result.Message = ex.Message;
        //    }

        //    return result;
        //}



        ////getInstagramProfile(string shortLivedAccessToken)
        //[HttpGet("GetInstagramLongLivedAccessToken")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesDefaultResponseType]
        //public async Task<Response<Dictionary<string, string>>> GetInstagramLongLivedAccessToken(string shortLivedAccessToken)
        //{
        //    Response<Dictionary<string, string>> result = new Response<Dictionary<string, string>>();
        //    string url = $"https://graph.instagram.com/access_token" +
        //    $"?grant_type=ig_exchange_token" +
        //    $"&client_secret={_clientSecret}" +
        //    $"&access_token={shortLivedAccessToken}";


        //    try
        //    {
        //        var httpClient = new HttpClient();
        //        var httpResponse = await httpClient.GetAsync(url);
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        //        if (httpResponse.IsSuccessStatusCode)
        //        {

        //            var responseJson = JsonConvert.DeserializeObject<dynamic>(httpContent);

        //            result.Succeeded = true;
        //            string accessToken = responseJson["access_token"].ToString(); // Convert to string

        //                result = await GetRefreshToken(accessToken);


        //        }
        //        else
        //        {
        //            result.Succeeded = false;
        //            result.Message = "Failed to retrieve Instagram access token.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Succeeded = false;
        //        result.Message = "An error occurred: " + ex.Message;
        //    }

        //    return result;
        //}

        //[HttpGet("GetRefreshToken")]
        //// [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<Response<Dictionary<string, string>>> GetRefreshToken(string accessToken)
        //{
        //    Response<Dictionary<string, string>> result = new Response<Dictionary<string, string>>();
        //    string urlRefreshToken = $"https://graph.instagram.com/refresh_access_token" +
        //                                         $"?grant_type=ig_refresh_token" +
        //                                         $"&access_token={accessToken}" +
        //                                         $"&client_secret={_clientSecret}";
        //    using (var Client = new HttpClient())
        //    {
        //        var response = await Client.GetAsync(urlRefreshToken);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseContent = await response.Content.ReadAsStringAsync();
        //            // Parse the JSON response to get the refreshed access token
        //            // You can use a JSON deserialization library like Newtonsoft.Json
        //            // For simplicity, we'll just extract it using string manipulation here
        //            var refreshedAccessTokenObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
        //            string expiresInSeconds = refreshedAccessTokenObject["expires_in"].ToString();
        //            string refreshaccesstoken = refreshedAccessTokenObject["access_token"];
        //            result.Data = new Dictionary<string, string>
        //                {
        //                    { "Access_Token", accessToken },
        //                    { "Expiration_Date", expiresInSeconds.ToString() },
        //                     {"RefrechToken", refreshaccesstoken }
        //                };
        //            // result.Data.Add("RefrechToken", refreshedAccessToken);
        //            result.Succeeded= true;
        //            return result;
        //        }
        //        else
        //        {
        //            result.Succeeded = false;
        //            result.Message = "Failed to retrieve Instagram Refresh access token.";
        //            return result;
        //        }
        //    }
        //}






        [HttpGet("GetPageAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetPageAccessToken(string PAGEID, string LongLivedAccessToken)
        {

            string url = "https://graph.instagram.com/" + PAGEID + "?fields=access_token&access_token=" + LongLivedAccessToken;

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

    }
}

