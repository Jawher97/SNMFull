using Microsoft.Extensions.Configuration;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;

namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterOauthRepository : ITwitterOAuthRepository
    {

        readonly string _consumerKey;
        readonly string _consumerKeySecret;
        private readonly IConfiguration _config;
        private readonly string _callbackUrl = "https://localhost:4200/twittercallback";

        private readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
       


        public TwitterOauthRepository() { }

        public TwitterOauthRepository(IConfiguration config)
        {
            _config = config;
            _consumerKey = _config.GetValue<string>("Twitter:ConsumerKey");
            _consumerKeySecret = _config.GetValue<string>("Twitter:ConsumerSecretKey");

        }

        public async Task<string> GetOAuthTokenAsync()
        {

            var request_url = $"https://api.twitter.com/oauth/request_token";
            var _httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, request_url);
            var httpMethod = request.Method.ToString();
            request.Headers.Add("Authorization", PrepareOAuth(httpMethod, request_url, null));
            var response = await _httpClient.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var queryString = HttpUtility.ParseQueryString(content);
            return queryString["oauth_token"];
        }


        string PrepareOAuth(string httpMethod , string URL, Dictionary<string, string> data)
        {
            // seconds passed since 1/1/1970
            var timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);

            // Add all the OAuth headers we'll need to use when constructing the hash
            Dictionary<string, string> oAuthData = new Dictionary<string, string>();
            oAuthData.Add("oauth_consumer_key", _consumerKey);
            oAuthData.Add("oauth_nonce", Guid.NewGuid().ToString());
            oAuthData.Add("oauth_signature_method", "HMAC-SHA1");
            oAuthData.Add("oauth_timestamp", timestamp.ToString());
            oAuthData.Add("oauth_callback", _callbackUrl);
            oAuthData.Add("oauth_version", "1.0");

            if (data != null) // add text data too, because it is a part of the signature
            {
                foreach (var item in data)
                {
                    oAuthData.Add(item.Key, item.Value);
                }
            }

            // Generate the OAuth signature and add it to our payload
            oAuthData.Add("oauth_signature", GenerateSignature(URL, oAuthData, httpMethod));

            // Build the OAuth HTTP Header from the data
            return GenerateOAuthHeader(oAuthData);
        }


        string GenerateSignature(string url, Dictionary<string, string> data, string httpMethod)
        {
            var _sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes($"{_consumerKeySecret}&"));

            var sigString = string.Join(
                "&",
                data.Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format("{0}&{1}&{2}",
                httpMethod,
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString)
                
            );

            return Convert.ToBase64String(
                _sigHasher.ComputeHash(
                    Encoding.UTF8.GetBytes(fullSigData)

                )
            );
        }

        string GenerateOAuthHeader(Dictionary<string, string> data)
        {
            return string.Format(
                "OAuth {0}",
                string.Join(
                    ", ",
                    data
                        .Where(kvp => kvp.Key.StartsWith("oauth_"))
                        .Select(
                            kvp => string.Format("{0}=\"{1}\"",
                            Uri.EscapeDataString(kvp.Key),
                            Uri.EscapeDataString(kvp.Value)
                            )
                        ).OrderBy(s => s)
                    )
                );
        }


        public async Task<TwitterProfileData> GetAccessTokenAsync(string oAuthToken, string oAuthVerifier)
        {
            var AccessTokenURL = $"https://api.twitter.com/oauth/access_token?oauth_verifier={oAuthVerifier}&oauth_token={oAuthToken}";

            var request = new HttpRequestMessage(HttpMethod.Post, AccessTokenURL);

            var _httpClient = new HttpClient();

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var queryString = HttpUtility.ParseQueryString(content);

            var oauth_token = queryString["oauth_token"];
            var oauth_token_secret = queryString["oauth_token_secret"];
            var userId = queryString["user_id"];
            var screen_name = queryString["screen_name"];

            TwitterProfileData userData = new TwitterProfileData
            {
                /*TwitterUserId = userId,
                oauth_token = oauth_token,
                oauth_token_secret = oauth_token_secret,
                screen_name = screen_name,*/
            };

            return userData;
        }











    }
}
