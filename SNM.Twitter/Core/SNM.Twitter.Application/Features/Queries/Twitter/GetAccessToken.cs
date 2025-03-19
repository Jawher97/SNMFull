using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace SNM.Twitter.Application.Features.Queries.Twitter
{
    public class GetAccessToken
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;

        private readonly IConfiguration _config;

        public GetAccessToken(IConfiguration config)
        {
            _config = config;
            _consumerKey = _config.GetValue<string>("Twitter:ConsumerKey");
            _consumerSecret = _config.GetValue<string>("Twitter:ConsumerSecretKey");
        }

        public async Task<string> GetBearerToken()
        {
            string url = "https://api.twitter.com/oauth2/token";
            string credentials = $"{_consumerKey}:{_consumerSecret}";
            var base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Credentials}");
                var content = new StringContent("grant_type=client_credentials", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                    return responseObject["bearer_token"];
                }
                else
                {
                    throw new Exception($"Failed to get bearer token: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
    }
}
