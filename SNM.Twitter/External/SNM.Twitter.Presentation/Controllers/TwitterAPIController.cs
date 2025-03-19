using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using SNM.Twitter.Domain.Twitter;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using SNM.Twitter.Domain.TwitterEntities;
using System.Collections.Generic;
using SNM.Twitter.Application.DTO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using SNM.Twitter.Application.Exceptions.Model;
using SNM.Twitter.Application.Features.Queries.TwitterChannels;
using MediatR;

namespace SNM.Twitter.Presentation.Controllers
{
    [ApiController]
    [Route("apitwitter/v1/[controller]")]
    public class TwitterAPIController : Controller
    {
        
        private readonly IConfiguration _config;
        //private readonly HttpClient _httpClient;
        private readonly IMediator _mediator;
        public TwitterAPIController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _config = config;
        }


        [HttpGet("GetUserInfos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUsersInfos(string screen_name)
        {
            var _bearertoken = _config.GetValue<string>("Twitter:Bearer");

            // Configuration de l'URL de l'API Twitter
            var url = $"https://api.twitter.com/1.1/users/show.json?screen_name={screen_name}";

            // Création d'un client HTTP pour effectuer la requête à l'API Twitter
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearertoken);

            // Envoi de la requête à l'API Twitter
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            return Json(responseContent);

        }
        //private const string RequestTokenUrl = "https://api.twitter.com/oauth/request_token";
        //private const string AccessTokenUrl = "https://api.twitter.com/oauth/access_token";
        //[HttpGet("GetToken")]
        //public async Task<string> GetAccessTokenAsync( string requestTokenSecret, string verifier)
        //{
           
        //    // Create a HttpClient instance
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Build the OAuth request to exchange request token for an access token
        //        var oauthNonce = Guid.NewGuid().ToString("N");
        //        var oauthTimestamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
        //        string consumerKey = "xXHYgVxqBjdS7CEwZR1DeRDWl";
        //        string consumerSecret = "6OzdWrFm1wCDa1vNw9Xz6Dv8cqXZW03ylLX84UnvcTs4FT09nn";
        //        var requestToken = await AuthFlow.InitAuthenticationAsync(consumerKey, consumerSecret);
        //        var requestTokenParams = new Dictionary<string, string>
        //            {
        //                { "oauth_consumer_key", consumerKey },
        //                { "oauth_nonce", Guid.NewGuid().ToString("N") },
        //                { "oauth_timestamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString() },
        //                { "oauth_signature_method", "HMAC-SHA1" },
        //                { "oauth_version", "1.0" },
        //                // Additional parameters as needed
        //            };
        //        var oauthSignature = GenerateSignature(
        //            "POST", AccessTokenUrl,
        //           requestTokenParams,
        //            consumerSecret, requestTokenSecret);

        //        // Include OAuth parameters in the request headers
        //        httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth oauth_nonce=\"{oauthNonce}\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"{oauthTimestamp}\", oauth_consumer_key=\"{consumerKey}\", oauth_token=\"{requestToken}\", oauth_verifier=\"{verifier}\", oauth_signature=\"{oauthSignature}\", oauth_version=\"1.0\"");

        //        // Send the POST request to exchange request token for access token
        //        var response = await httpClient.PostAsync(AccessTokenUrl, null);
        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        // Parse the response to extract the access token
        //       var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(responseContent);

        //        string accessToken = queryParams["oauth_token"];

        //        return accessToken;
        //    }
        //}
       
        [HttpGet("GetAuthTokenSecret")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<string> GetAuthTokenSecret()
        {
            // Define your consumer key and consumer secret
            string consumerKey = "xXHYgVxqBjdS7CEwZR1DeRDWl";
            string consumerSecret = "6OzdWrFm1wCDa1vNw9Xz6Dv8cqXZW03ylLX84UnvcTs4FT09nn";
            const string REQUEST_TOKEN_URL = "https://api.twitter.com/oauth/request_token"; // The actual URL may vary
            // Create a dictionary to hold the request token parameters
            var requestTokenParams = new Dictionary<string, string>
                    {
                        { "oauth_consumer_key", consumerKey },
                        { "oauth_nonce", Guid.NewGuid().ToString("N") },
                        { "oauth_timestamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString() },
                        { "oauth_signature_method", "HMAC-SHA1" },
                        { "oauth_version", "1.0" },
                        // Additional parameters as needed
                    };

            // Generate the OAuth signature and add it to the parameters
            requestTokenParams.Add("oauth_signature", GenerateSignature("POST", REQUEST_TOKEN_URL, requestTokenParams, consumerSecret, null));

            // Make the HTTP POST request to obtain the request token
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(REQUEST_TOKEN_URL, new FormUrlEncodedContent(requestTokenParams));
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent.ToString();
        }
        string GenerateSignature(string httpMethod, string baseUrl, Dictionary<string, string> parameters, string consumerSecret, string tokenSecret)
        {
            // Combine and sort parameters
            var combinedParams = parameters
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}")
                .ToList();

            // Concatenate the HTTP method, base URL, and sorted parameters
            var signatureBase = $"{httpMethod}&{Uri.EscapeDataString(baseUrl)}&{Uri.EscapeDataString(string.Join("&", combinedParams))}";

            // Create the signing key
            var signingKey = $"{Uri.EscapeDataString(consumerSecret)}&{Uri.EscapeDataString(tokenSecret ?? "")}";

            // Compute the HMAC-SHA1 signature
            using (var hasher = new HMACSHA1(Encoding.UTF8.GetBytes(signingKey)))
            {
                var hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(signatureBase));
                return Convert.ToBase64String(hashBytes);
            }
        }



        /*Get all comments UTILE*/
        [HttpGet("GetAllRetweetsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetRetweets(long tweetId, string accesstoken)
        {
            var _bearertoken = _config.GetValue<string>("Twitter:Bearer");

            // Configuration de l'URL de l'API Twitter
            var url = $"https://api.twitter.com/2/tweets/{tweetId}/retweeted_by?user.fields=created_at&expansions=pinned_tweet_id&tweet.fields=created_at";


            // Création d'un client HTTP pour effectuer la requête à l'API Twitter
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            // Envoi de la requête à l'API Twitter
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Traitement de la réponse de l'API Twitter
            if (response.IsSuccessStatusCode)
            {
                // Extraction des commentaires de la réponse JSON
                var retweets = JsonConvert.DeserializeObject<TwitterRetweetsResponse>(responseContent);

                // Renvoi des commentaires sous forme de liste de tweets
                return Ok(retweets.Data);
            }
            else
            {
                // Traitement des erreurs
                return Ok(responseContent);
            }
        }

       
        /*Get all likes*/
        [HttpGet("GetAllPostLikesByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetTweetLikes(string tweetId, string accesstoken)
        {
            var _bearertoken = _config.GetValue<string>("Twitter:Bearer");

            // Configuration de l'URL de l'API Twitter pour récupérer les likes du tweet
            var apiUrl = $"https://api.twitter.com/2/users/{tweetId}/timelines/reverse_chronological?tweet.fields=created_at&expansions=author_id&user.fields=created_at&max_results=5";

            // Ajout du bearer token à l'en-tête d'autorisation de la requête
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            // Envoi de la requête HTTP GET à l'API Twitter pour récupérer les likes du tweet
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Extraction des likes de la réponse JSON
                var responseContent = await response.Content.ReadAsStringAsync();
                var likes = JsonConvert.DeserializeObject<TwitterLikesResponse>(responseContent);

                // Renvoi des likes sous forme de liste de tweets
                return Ok(likes.Data);
            }
            else
            {
                // En cas d'erreur, renvoie du code d'état HTTP de l'API Twitter
                return StatusCode((int)response.StatusCode);
            }
        }
        [HttpGet("GetInstagramChannel")]
        //[HttpGet("{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<TwitterChannelDto> GetTwitterChannelbyChannelId(Guid channelId)
        {

            var getEntities = new GetTwitterChannelByIdQuery(channelId);
            var entities = await _mediator.Send(getEntities);

            return entities;
        }
        [HttpGet("GetLastTweet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<Response<PostDto>> GetLastTweet([FromQuery] Guid channelId)
        {

            Response<PostDto> post = new Response<PostDto>();
            List<ReactionsDto> Reactions = new List<ReactionsDto>();
            var twiiterChannel = await GetTwitterChannelbyChannelId(channelId);
            string url = $"https://api.twitter.com/2/users/145606458494803/tweets";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", twiiterChannel.AccessToken);
            var httpResponse = await httpClient.GetAsync(url);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                // var httpContent = await httpResponse.Content.ReadAsStringAsync();
                var tweet = JsonConvert.DeserializeObject<dynamic>(httpContent);
                MediaDto media = new MediaDto();
               

                post.Data = new PostDto
                {
                    Caption = tweet.data[0].text,
                    PublicationDate = tweet.data[0]?.timestamp,
                    MediaData = new List<MediaDto>(),
                    Comments = new List<CommentDto>(),
                    Reactions = Reactions,
                    TotalCountReactions = tweet.data[0].like_count,
                    PostClicks = tweet.data[0]?.insights.data[0]?["values"]?[0]?["value"] ?? 0,
                    PostEngagedUsers = tweet.data[0]?.insights.data[1]?["values"]?[0]?["value"] ?? 0
                };

                
                    //var Comments = await GetInstagramComments(instagrampost.data[0].id.ToString(), instagramchannel.UserAccessToken);
                    //if (Comments.Succeeded)
                    //{

                    //    post.Data.Comments = Comments.Data;



                    //    post.Succeeded = true;
                    //}
                    //else
                    //{
                    //    post.Succeeded = false;
                    //    post.Message = $"Failed to get Comments Post with {httpResponse.StatusCode}";
                    //}

                }
                else
                {
                    post.Succeeded = false;
                    post.Message = $"Failed to get Last Post with {httpResponse.StatusCode}";
                }

            
            return post;

        }

        /*Get all replies*/
        [HttpGet("GetAllQuotes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetReplies(string tweetId)
        {
            var _bearertoken = _config.GetValue<string>("Twitter:Bearer");
            var _httpClient = new HttpClient();
            var url = $"https://api.twitter.com/2/tweets/{tweetId}/quote_tweets?expansions=author_id&tweet.fields=created_at&user.fields=created_at";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearertoken);
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }
            var content = await response.Content.ReadAsStringAsync();
            var tweetResponse = JsonConvert.DeserializeObject<TweetSearchResponse>(content);
            var quotes = tweetResponse.data.ToList();
            return Ok(quotes);

        }



        /*Get all views UTILE*/
        [HttpGet("GetAllViews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetViews(string tweetId)
        {
            var _bearertoken = _config.GetValue<string>("Twitter:Bearer");
            var url = $"https://api.twitter.com/1.1/media/metrics/video_views.json?ids={tweetId}";
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _bearertoken);
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }
            string responseContent = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(responseContent);

            // Extract the number of views from the JSON response
            int views = jsonResponse["data"][0]["metrics"]["view_count"].Value<int>();
            return Ok(views);

        }



        //public metrics: UTILE
        [HttpGet("GetMetrics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetMetrics(string tweetId, string accessToken)
        {
            var _bearertoken = _config.GetValue<string>("Twitter:Bearer");
            var url = $"https://api.twitter.com/2/tweets/{tweetId}?tweet.fields=non_public_metrics,organic_metrics&media.fields=non_public_metrics,organic_metrics&expansions=attachments.media_keys";

            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Bearer", _bearertoken);
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }
            string responseContent = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(responseContent);

            // Extract the metrics from the JSON response
            int retweetcount = jsonResponse["data"][0]["organic_metrics"]["retweet_count"].Value<int>();
            int replycount = jsonResponse["data"][0]["organic_metrics"]["reply_count"].Value<int>();
            int likecount = jsonResponse["data"][0]["organic_metrics"]["like_count"].Value<int>();
            int urlClicksCount = jsonResponse["data"][0]["organic_metrics"]["url_link_clicks"].Value<int>();
            int userprofileClicks = jsonResponse["data"][0]["organic_metrics"]["url_link_clicks"].Value<int>();

            // Create a dictionary to store the metrics
            var metrics = new Dictionary<string, int>
                {
                    { "retweetcount", retweetcount },
                    { "replycount", replycount },
                    { "likecount", likecount },
                    { "quotecount", urlClicksCount },
                    {"userprofileClicks", userprofileClicks }
                };

            // Return the metrics in the response
            return Ok(metrics);

        }


        /*Get all bookmarked tweets*/
        [HttpGet("GetAllbookmarkedTweets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllbookmarkedTweets(string brandName)
        {
            var _bearertoken = _config.GetValue<string>("Twitter:Bearer");
            var _httpClient = new HttpClient();

            var nameUrl = $"https://api.twitter.com/2/users/by?username={brandName}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearertoken);

            var brandIdResponse = await _httpClient.GetAsync(nameUrl);

            if (brandIdResponse.IsSuccessStatusCode)
            {
                var IdContent = await brandIdResponse.Content.ReadAsStringAsync();
                var IdResponse = JsonConvert.DeserializeObject<TwitterBrand>(IdContent);
                var brandId = IdResponse.id;

                var url = $"https://api.twitter.com/2/users/{brandId}/bookmarks";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearertoken);
                var response = await _httpClient.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();
                var bookTweets = content.ToList();
                return Ok(bookTweets);

            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        /*Delete Tweet  UTILE*/
        [HttpDelete("DeleteTweetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTweetById(long tweetId, string accessToken)
        {

            // Configuration de l'URL de l'API Twitter
            var url = $"https://api.twitter.com/2/tweets/{tweetId}";

            var _httpClient = new HttpClient();
            // Création d'un client HTTP pour effectuer la requête à l'API Twitter
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Envoi de la requête à l'API Twitter
            var response = await _httpClient.DeleteAsync(url);

            // Traitement de la réponse de l'API Twitter
            if (response.IsSuccessStatusCode)
            {
                // Extraction des commentaires de la réponse JSON
                var responseContent = await response.Content.ReadAsStringAsync();

                // Renvoi des commentaires sous forme de liste de tweets
                return Ok(responseContent);
            }
            else
            {
                // Traitement des erreurs
                return StatusCode((int)response.StatusCode);
            }

        }
        
    }
}
