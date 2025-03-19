using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterOAuth2Repository : ITwitterOAuth2Repository
    {


        //private readonly string _clientId = "M25SWkJxcFVzU29WZEo4elhvZTA6MTpjaQ";
        //private readonly string _clientIdSecret = "pvFbe2LFHvNWLSd47UyNoldtwzbcenYnLshBRU2XDdDO8krEYE";
        private readonly string _redirectUri = "https://localhost:4200/twittercallback";
        readonly string _clientId;
        readonly string _clientIdSecret;
        private readonly IConfiguration _config;
        public TwitterOAuth2Repository(IConfiguration config)
        {
            _config = config;
            _clientId = _config.GetValue<string>("Twitter:ClientId");
            _clientIdSecret = _config.GetValue<string>("Twitter:ClientIdSecret");

        }
        public async Task<Response<ChannelProfile>> ExchangeCodeForTokenAsync(string code)
        {
            Response<ChannelProfile> channelProfile = new Response<ChannelProfile>(); 
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/2/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue(
                                    "Basic",
                                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientIdSecret}"))
                                );

            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code" , code },
                    { "grant_type" , "authorization_code" },
                    { "redirect_uri", _redirectUri },
                    { "code_verifier", "nothing" },
                   
                });

             request.Content = formContent;

             var response = await client.SendAsync(request);
             var responseJson = await response.Content.ReadAsStringAsync();
             var jsonObject = JsonConvert.DeserializeObject<dynamic>(responseJson);
            if (response.IsSuccessStatusCode)
            {
                
                    string accessToken = jsonObject["access_token"].ToString();


                    var channelprofileData = await GetUserPublicInfos(accessToken);
                if(channelprofileData != null) {
                    ChannelProfile profile = new ChannelProfile
                    {
                        AccessToken = jsonObject["access_token"].ToString(),
                        expires_in = jsonObject["expires_in"].ToString(),
                        RefreshToken = jsonObject["refresh_token"].ToString(),
                        Scope = jsonObject["scope"].ToString(),
                        UserName = channelprofileData.Data.UserName,
                        Description = channelprofileData.Data.Description,
                        CoverPhoto = channelprofileData.Data.CoverPhoto,
                        ProfileUserId = channelprofileData.Data.ProfileUserId,
                        ProfileLink= channelprofileData.Data.ProfileLink,   

                    };
                    channelProfile.Data = profile;
                    channelProfile.Succeeded = true;
                }
                else
                {
                    channelProfile= channelprofileData;
                }
                    
                
               
               

                

            }
            else
            {
                channelProfile.Succeeded = false;
                channelProfile.Message = "failed to get UserProfile";
            }


            return channelProfile;
        }
        public async Task<Response<ChannelProfile>> GetUserPublicInfos(string accessToken)
        {
            Response<ChannelProfile> channelProfile = new Response<ChannelProfile>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.GetAsync(_config["Twitter:ProfileUrl"]);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string username = jsonObject["data"]["username"].ToString();
                ChannelProfile profile = new ChannelProfile
                {
                    ProfileUserId = jsonObject["data"]["id"],
                    CoverPhoto = jsonObject["data"]["profile_image_url"],
                    UserName = jsonObject["data"]["name"],
                    Description = jsonObject["data"]["description"],
                    ProfileLink=$"https://twitter.com/{username}"

                };
                channelProfile.Data = profile;
                channelProfile.Succeeded = true;
            }
            else
            {
                channelProfile.Succeeded = true;
                channelProfile.Message = responseContent;
            }



            return channelProfile;
        }
    }
}
