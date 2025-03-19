
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.Instagram.Application.Interfaces;


namespace SNM.Instagram.Application.Features.Commands.InstagramPostAPI
{


    public class InstagramInsightsService //: IInstagramInsightsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly IConfiguration _configuration;

        public InstagramInsightsService(IConfiguration configuration)
        {
            _accessToken = configuration["Instagram:AccessToken"];
            _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v16.0/") };
            _configuration = configuration;
        }

    }
}
