using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Domain.Entities;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterAPIRepository : ITwitterAPIRepository<Guid>
    {
        readonly string _consumerKey;
        readonly string _consumerKeySecret;


        private readonly IConfiguration _config;


        readonly string _TwitterTextAPI;
        readonly string _TwitterMediaAPI;
        readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        readonly int _limit;

        public TwitterAPIRepository(IConfiguration config,
                                    int limit = 280)
        {
            _config = config;
            _TwitterTextAPI = "https://api.twitter.com/1.1/statuses/update.json";
            _TwitterMediaAPI = "https://upload.twitter.com/1.1/media/upload.json";
            _consumerKey = _config.GetValue<string>("Twitter:ConsumerKey");
            _consumerKeySecret = _config.GetValue<string>("Twitter:ConsumerSecretKey");
            _limit = limit;


        }



        public TwitterAPIRepository()
        {
        }

        public Task<string> GetUserBanner(string oauth_token, string oauth_token_secret, string screen_name)
        {
            throw new NotImplementedException();
        }

        public async Task<TwitterPost> PublishToTwitterv2(Post post)
        {

            


            var requestUrl = "https://api.twitter.com/2/tweets";

            var message = new
            {
               // text = CutTweetToLimit(post.Message)

            };

            var requestJson = JsonConvert.SerializeObject(message);

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", post.Caption);

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, requestContent);
                var content = await response.Content.ReadAsStringAsync();
                var responseJson = await response.Content.ReadAsStringAsync();
                var res = responseJson.ToString();

                if (response.IsSuccessStatusCode)
                {

                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseJson);
                    string twitterPostId = jsonObject["data"]["id"];
                   // string Message = post.Message;


                    TwitterPost twitterPost = new TwitterPost
                    {
                        //TwitterPostId = twitterPostId,
                        // Message = Message, 
                        //created_at = DateTime.Now,
                    };



                    return twitterPost;
                }
                else
                {
                    return null;
                }
            }
        }


        public Task<Tuple<int, string>> Retweet(string tweetId, string accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> PublishToTwitter(TwitterPostDto post, string oauth_token, string oauth_token_secret)
        {
            throw new NotImplementedException();
        }


        //public async Task<string> GetUserBanner(string oauth_token, string oauth_token_secret, string screen_name)
        //{
        //    var httpClient = new HttpClient();

        //    // Set request parameters
        //    var requestUrl = $"https://api.twitter.com/1.1/users/profile_banner.json?screen_name={screen_name}";

        //    httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(requestUrl, null, "GET", oauth_token, oauth_token_secret));

        //    var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        //    var response = await httpClient.SendAsync(request);

        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    return responseContent;

        //}


        //    public async Task<string> PublishToTwitter(TwitterPostDto post, string oauth_token, string oauth_token_secret)
        //    {
        //        try
        //        {
        //            // first, upload the image
        //            string mediaID = string.Empty;
        //            var rezImage = Task.Run(async () =>
        //            {
        //                var response = await TweetImage(post.Media.FileName, oauth_token, oauth_token_secret);
        //                return response;
        //            });
        //            var rezImageJson = JObject.Parse(rezImage.Result.Item2);

        //            if (rezImage.Result.Item1 != 200)
        //            {
        //                try // return error from JSON
        //                {
        //                    return $"Error uploading image to Twitter. {rezImageJson["errors"][0]["message"].Value<string>()}";
        //                }
        //                catch (Exception ex) // return unknown error
        //                {
        //                    // log exception somewhere
        //                    return "Unknown error uploading image to Twitter";
        //                }
        //            }
        //            mediaID = rezImageJson["media_id_string"].Value<string>();

        //            // second, send the text with the uploaded image
        //            var rezText = Task.Run(async () =>
        //            {
        //                var response = await TweetText(CutTweetToLimit(post.Text), mediaID, oauth_token, oauth_token_secret);
        //                return response;
        //            });
        //            var rezTextJson = JObject.Parse(rezText.Result.Item2);

        //            if (rezText.Result.Item1 != 200)
        //            {
        //                try // return error from JSON
        //                {
        //                    return $"Error sending post to Twitter. {rezTextJson["errors"][0]["message"].Value<string>()}";
        //                }
        //                catch (Exception ex) // return unknown error
        //                {
        //                    // log exception somewhere
        //                    return "Unknown error sending post to Twitter";
        //                }
        //            }

        //            return "OK";
        //        }
        //        catch (Exception ex)
        //        {
        //            // log exception somewhere
        //            return "Unknown error publishing to Twitter";
        //        }
        //    }

        //    #region Some OAuth magic
        //    string PrepareOAuth(string URL, Dictionary<string, string> data, string httpMethod, string oauth_token, string oauth_token_secret)
        //    {
        //        // seconds passed since 1/1/1970
        //        var timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);

        //        // Add all the OAuth headers we'll need to use when constructing the hash
        //        Dictionary<string, string> oAuthData = new Dictionary<string, string>();
        //        oAuthData.Add("oauth_consumer_key", _consumerKey);
        //        oAuthData.Add("oauth_signature_method", "HMAC-SHA1");
        //        oAuthData.Add("oauth_timestamp", timestamp.ToString());
        //        oAuthData.Add("oauth_nonce", Guid.NewGuid().ToString());
        //        oAuthData.Add("oauth_token", oauth_token);
        //        oAuthData.Add("oauth_version", "1.0");

        //        if (data != null) // add text data too, because it is a part of the signature
        //        {
        //            foreach (var item in data)
        //            {
        //                oAuthData.Add(item.Key, item.Value);
        //            }
        //        }

        //        // Generate the OAuth signature and add it to our payload
        //        oAuthData.Add("oauth_signature", GenerateSignature(URL, oAuthData, httpMethod, _consumerKeySecret, oauth_token_secret));

        //        // Build the OAuth HTTP Header from the data
        //        return GenerateOAuthHeader(oAuthData);
        //    }

        //    /// <summary>
        //    /// Generate an OAuth signature from OAuth header values
        //    /// </summary>
        //    string GenerateSignature(string url, Dictionary<string, string> data, string httpMethod, string _consumerKeySecret, string oauth_token_secret)
        //    {

        //        var _sigHasher = new HMACSHA1(
        //            new ASCIIEncoding().GetBytes($"{_consumerKeySecret}&{oauth_token_secret}")
        //        );

        //        var sigString = string.Join(
        //                "&",
        //                data
        //                    .Union(data)
        //                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
        //                    .OrderBy(s => s)
        //            );

        //        var fullSigData = string.Format("{0}&{1}&{2}",
        //            httpMethod,
        //            Uri.EscapeDataString(url),
        //            Uri.EscapeDataString(sigString)
        //        );


        //        return Convert.ToBase64String(
        //            _sigHasher.ComputeHash(
        //                new ASCIIEncoding().GetBytes(fullSigData.ToString())
        //            )
        //        );

        //    }

        //    /// <summary>
        //    /// Generate the raw OAuth HTML header from the values (including signature)
        //    /// </summary>
        //    string GenerateOAuthHeader(Dictionary<string, string> data)
        //    {
        //        return string.Format(
        //            "OAuth {0}",
        //            string.Join(
        //                ", ",
        //                data
        //                    .Where(kvp => kvp.Key.StartsWith("oauth_"))
        //                    .Select(
        //                        kvp => string.Format("{0}=\"{1}\"",
        //                        Uri.EscapeDataString(kvp.Key),
        //                        Uri.EscapeDataString(kvp.Value)
        //                        )
        //                    ).OrderBy(s => s)
        //                )
        //            );
        //    }
        //    #endregion

        //    /// <summary>
        //    /// Cuts the tweet text to fit the limit
        //    /// </summary>
        //    /// <returns>Cutted tweet text</returns>
        //    /// <param name="tweet">Uncutted tweet text</param>
           string CutTweetToLimit(string tweet)
            {
                if (tweet.Length <= _limit)
                {
                    return tweet;
                }

    StringBuilder sb = new StringBuilder();
    string[] words = tweet.Split(' ');
                foreach (string word in words)
                {
                    if (sb.Length + word.Length + 1 <= _limit)
                    {
                        sb.Append(word + " ");
                    }
                    else
{
    break;
}
                }

                return sb.ToString().TrimEnd() + "...";
            }





        //    /*public async Task<Tuple<int, string>> Init(TwitterPostDto post)
        //    {
        //        var httpClient = new HttpClient();

        //        // Set request parameters
        //        var requestUrl = "https://upload.twitter.com/1.1/media/upload.json";

        //        string mediaType = post.Media.ContentType;


        //        var content = new MultipartFormDataContent();
        //        byte[] imgdata = System.IO.File.ReadAllBytes(post.Media.FileName);
        //        var mediaContent = new ByteArrayContent(imgdata);

        //        var mediaData = Convert.ToBase64String(imgdata);
        //        mediaContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");  //image/jpeg or video/mp4
        //        content.Add(mediaContent, "media");
        //        content.Add(new StringContent("command=INIT&media_data="+mediaData+"&media_type="+mediaType+"&total_bytes="+ post.Media.Length));



        //        httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(requestUrl, null, "POST"));

        //        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        //        request.Content = content;

        //        var response = await httpClient.SendAsync(request);

        //        var responseContent = await response.Content.ReadAsStringAsync();


        //        return new Tuple<int, string>(
        //            (int)response.StatusCode,
        //            responseContent
        //            );
        //    }


        //    public async Task<Tuple<int, string>> Append(TwitterPostDto post)
        //    {
        //        string mediaID = string.Empty;

        //        var rez = Task.Run(async () =>
        //        {
        //            var response = await Init(post);
        //            return response;
        //        });
        //        var rezJson = JObject.Parse(rez.Result.Item2);

        //        mediaID = rezJson["media_id_string"].Value<string>();

        //        var httpClient = new HttpClient();

        //        // Set request parameters
        //        var requestUrl = "https://upload.twitter.com/1.1/media/upload.json";

        //        byte[] imgdata = System.IO.File.ReadAllBytes(post.Media.FileName);
        //        var imageContent = new ByteArrayContent(imgdata);
        //        imageContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

        //        var multipartContent = new MultipartFormDataContent();
        //        multipartContent.Add(imageContent, "media");
        //        multipartContent.Add( new StringContent("command=APPEND&media_id="+mediaID+ "&segment_index=0&media_data="+mediaID));

        //        httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(requestUrl, null, "POST"));

        //        var response = await httpClient.PostAsync(requestUrl, multipartContent);
        //        var responseContent = await response.Content.ReadAsStringAsync();

        //                return new Tuple<int, string>(
        //                    (int)response.StatusCode,
        //                    responseContent
        //                    );

        //    }


        //    public async Task<HttpResponseMessage> Finalize(TwitterPostDto post)
        //    {
        //        string mediaID = string.Empty;

        //        var rez = Task.Run(async () =>
        //        {
        //            var response = await Append(post);
        //            return response;
        //        });
        //        var rezJson = JObject.Parse(rez.Result.Item2);

        //        mediaID = rezJson["media_id"].Value<string>();

        //        var httpClient = new HttpClient();

        //        // Set request parameters
        //        var requestUrl = "https://upload.twitter.com/1.1/media/upload.json";

        //        var content = new MultipartFormDataContent();

        //        content.Add(new StringContent("command=FINALIZE&media_id=" + mediaID));

        //        httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(requestUrl, null, "POST"));

        //        var response = await httpClient.PostAsync(requestUrl, content);
        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        return response;
        //    }*/


        //    public async Task<TwitterPost> PublishToTwitterv2(Post post)
        //    {

        //        /*string mediaIdString = string.Empty;

        //        var rezMedia = Task.Run(async () =>
        //        {
        //            var response = await Init(post);
        //            return response;
        //        });
        //        var rezMediaJson = JObject.Parse(rezMedia.Result.Item2);

        //        if (rezMedia.Result.Item1 != 200)
        //        {
        //            try // return error from JSON
        //            {
        //                return $"Error uploading image to Twitter. {rezMediaJson["errors"][0]["message"].Value<string>()}";
        //            }
        //            catch (Exception ex) // return unknown error
        //            {
        //                // log exception somewhere
        //                return $"Unknown error uploading photo to Twitter. {ex.Message}";
        //            }
        //        }

        //        mediaIdString = rezMediaJson["media_id_string"].Value<string> ();*/


        //        var requestUrl = "https://api.twitter.com/2/tweets";

        //        var message = new
        //        {
        //            text = CutTweetToLimit(post.Message)

        //        };

        //        var requestJson = JsonConvert.SerializeObject(message);

        //        using (var client = new HttpClient())
        //        {

        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", post.AccessToken);

        //            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        //            var response = await client.PostAsync(requestUrl, requestContent);
        //            var content = await response.Content.ReadAsStringAsync();
        //            var responseJson = await response.Content.ReadAsStringAsync();
        //            var res = responseJson.ToString();

        //            if (response.IsSuccessStatusCode)
        //            {

        //                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(responseJson);
        //                string twitterPostId = jsonObject["data"]["id"];
        //                string Message = post.Message;


        //                TwitterPost twitterPost = new TwitterPost
        //                {
        //                    TwitterPostId = twitterPostId,
        //                    // Message = Message, 
        //                    //created_at = DateTime.Now,
        //                };



        //                return twitterPost;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }






        //    /// <summary>
        //    /// Send a tweet with some image attached
        //    /// </summary>
        //    /// <returns>HTTP StatusCode and response</returns>
        //    /// <param name="text">Text</param>
        //    /// <param name="mediaID">Media ID for the uploaded image. Pass empty string, if you want to send just text</param>
        //    public Task<Tuple<int, string>> TweetText(string text, string mediaID, string oauth_token, string oauth_token_secret)
        //    {
        //        var textData = new Dictionary<string, string> {
        //            { "status", text },
        //            { "trim_user", "1" },
        //            { "media_ids", mediaID}
        //        };

        //        return SendText(_TwitterTextAPI, textData, oauth_token, oauth_token_secret);
        //    }


        //    /// <summary>
        //    /// Upload some image to Twitter
        //    /// </summary>
        //    /// <returns>HTTP StatusCode and response</returns>
        //    /// <param name="pathToImage">Path to the image to send</param>
        //    public Task<Tuple<int, string>> TweetImage(string pathToImage, string oauth_token, string oauth_token_secret)
        //    {
        //        byte[] imgdata = System.IO.File.ReadAllBytes(pathToImage);
        //        var imageContent = new ByteArrayContent(imgdata);
        //        imageContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

        //        var multipartContent = new MultipartFormDataContent();
        //        multipartContent.Add(imageContent, "media");

        //        return SendImage(_TwitterMediaAPI, multipartContent, oauth_token, oauth_token_secret);
        //    }

        //    async Task<Tuple<int, string>> SendText(string URL, Dictionary<string, string> textData, string oauth_token, string oauth_token_secret)
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(URL, textData, "POST", oauth_token, oauth_token_secret));

        //            var httpResponse = await httpClient.PostAsync(URL, new FormUrlEncodedContent(textData));
        //            var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //            return new Tuple<int, string>(
        //                (int)httpResponse.StatusCode,
        //                httpContent
        //                );
        //        }
        //    }

        //    async Task<Tuple<int, string>> SendImage(string URL, MultipartFormDataContent multipartContent, string oauth_token, string oauth_token_secret)
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(URL, null, "POST", oauth_token, oauth_token_secret));

        //            var httpResponse = await httpClient.PostAsync(URL, multipartContent);
        //            var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //            return new Tuple<int, string>(
        //                (int)httpResponse.StatusCode,
        //                httpContent
        //                );
        //        }
        //    }


        //    public async Task<Tuple<int, string>> Retweet(string tweetId, string accessToken)
        //    {
        //        var httpClient = new HttpClient();

        //        // Set request parameters
        //        var requestUrl = $"https://api.twitter.com/2/users/{tweetId}/retweets";

        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

        //        var response = await httpClient.SendAsync(request);

        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>(
        //            (int)response.StatusCode,
        //            responseContent
        //        );
        //    }


        //}
    }
}
