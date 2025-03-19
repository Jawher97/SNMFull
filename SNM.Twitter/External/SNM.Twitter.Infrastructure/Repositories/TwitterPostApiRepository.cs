using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SNM.Twitter.Application.DTO;
using SNM.Twitter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using MediatR;
using Serilog;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Polly;
using Polly.Retry;
using Tweetinvi.Core.Events;
using SNM.Twitter.Application.Exceptions.Model;

namespace SNM.Twitter.Infrastructure.Repositories
{
    public class TwitterPostApiRepository : ITwitterPostApiRepository
    {
        readonly string _consumerKey;
        readonly string _consumerKeySecret;
        readonly HMACSHA1 _sigHasher;
        readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        readonly int _limit;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;
        public TwitterPostApiRepository(IConfiguration config, 
        int limit = 280 )
        {
         
            _config = config;
            _consumerKey = _config.GetValue<string>("Twitter:ConsumerKey");
            _consumerKeySecret = _config.GetValue<string>("Twitter:ConsumerSecretKey");
            _httpClient = new HttpClient();
            _limit = limit;
            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                  .WaitAndRetryAsync(retryCount: 3, count => TimeSpan.FromMilliseconds(60), onRetry: (exception, count, context) =>
                  {
                      Log.Information("___Retrying____ => {@result}", count);
                      Log.Information("_______DateTime _____=> {@result}", DateTime.Now);

                  });
        }



        public async Task<Response<string>> PublishPostToTwitter(TwitterPostDto twitterPostDto)
        {
            Response<string> resp=new Response<string>();
            // first, upload the image
            string mediaID = string.Empty;
            List<string> mediaIDs = new List<string>();
            if ( twitterPostDto.PostDto.MediaData.Count() > 0)
            {
                foreach (var image in twitterPostDto.PostDto.MediaData)
                {
                    // Assuming you want to await each TweetImage call
                    var response = await TweetImage(image, twitterPostDto.TwitterChannelDto);
                    var rezImageJson = JObject.Parse(response.Item2);
                    if (response.Item1 != 200 && response.Item1 != 201 && response.Item1 != 204 &&  response.Item1 != 202)
                    {
                        
                           
                            resp.Succeeded = false;
                            resp.Message = $"Error sending post to Twitter. {response.Item2}";
                            return resp;
                      
                    }
                    mediaIDs.Add(rezImageJson["media_id"].Value<string>());
                }
            }
            

            // Send the text with the uploaded images
            var rezText = await TweetText(CutTweetToLimit(twitterPostDto.Text), mediaIDs, twitterPostDto.TwitterChannelDto);
            var rezTextJson = JObject.Parse(rezText.Item2);

            var jsonId = rezTextJson["data"]["id"];
            // Access the "edit_history_tweet_ids" property
          
            if (rezText.Item1 != 200 && rezText.Item1 != 201 && rezText.Item1 != 204)
            {
                
                    resp.Succeeded = false;
                    resp.Message = $"Error sending post to Twitter. {rezText.Item2}";
                    return resp;

                   
                
              
            }
            resp.Succeeded = true;
            resp.Data = jsonId.ToString();
            return resp;


        }


        public Task<Tuple<int, string>> TweetText(string text, List<string> mediaIDs, TwitterChannelDto TwitterChannel)
        {
            var textData = new Dictionary<string, object>
                    {
                        { "text", text }
                    };

            if (mediaIDs.Count() != 0)
               {
                 textData["media"] = new Dictionary<string, object>
                   {
                      { "media_ids", mediaIDs }
                    };
               }
            return SendText(TwitterChannel, textData);
        }



        public async Task<Tuple<int, string>> TweetImage(MediaDto twitterimagesDto, TwitterChannelDto twitterChannelDto)
        {


            //byte[] imgdata = System.IO.File.ReadAllBytes(twitterimagesDto.Media);
           
            // var imageContent = new ByteArrayContent(imgdata);


                var data = twitterimagesDto.Media_url.Split(',')[1];
                byte[] imgdata = Convert.FromBase64String(data);
                long totalBytes = imgdata.Length;
                if (twitterimagesDto.Media_type == Domain.Enumeration.MediaTypeEnum.IMAGE)
                {

                    var imageContent = new ByteArrayContent(imgdata);
                    imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(imageContent, "media");
                    return await SendImage(twitterChannelDto, multipartContent);
                }
           


                else
                {


                    return await sendVideo(imgdata, twitterChannelDto, totalBytes);


                }
            
           
        }
    
        public async Task<Tuple<int, string>> sendVideo(byte[] Media, TwitterChannelDto twitterChannelDto,long totalBytes)
        {
            // Step 1: Initialize the media upload
         
            var initContent =  new Dictionary<string, string>
                {
                    { "command", "INIT" },
                    { "media_type", "video/mp4" },
                    { "media_category", "tweet_video"},
                    { "total_bytes", totalBytes.ToString()},
                  
                    
                };
            var formData = new MultipartFormDataContent();

            // Add the parameters to the form data
            formData.Add(new StringContent("INIT"), "command");
            formData.Add(new StringContent("video/mp4"), "media_type");
            formData.Add(new StringContent("tweet_video"), "media_category");
            formData.Add(new StringContent(totalBytes.ToString()), "total_bytes");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(twitterChannelDto.TwitterImageAPI, null, "POST", twitterChannelDto));

            HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
            {
                var httpResponse = await httpClient.PostAsync(twitterChannelDto.TwitterImageAPI, formData);
            
                if (httpResponse.IsSuccessStatusCode)
                {
                    return httpResponse;

                }
                else
                {
                    //Log.Information("___Retrying => {@result}", response.StatusCode);
                    throw new HttpRequestException($"Request failed ");
                }
            });
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            //2 API
            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                //step 2
              var result=  await AppendMediaChunksAsync( jsonResponse["media_id"], Media, twitterChannelDto);

                if (result.Item1 == 200 || result.Item1 == 201 || result.Item1 == 204 || result.Item1==202)
                {
                    
                        await FinalizeMediaUploadAsync(jsonResponse["media_id"], twitterChannelDto);


                }
                else
                {
                    return new Tuple<int, string>(
                  (int)result.Item1,
                 result.Item2
                  );
                }
                //step 3
                //await FinalizeMediaUploadAsync( jsonResponse["media_id"], twitterChannelDto);
            }

            return new Tuple<int, string>(
                  (int)httpResponse.StatusCode,
                  httpContent
                  );
        }
        public async Task FinalizeMediaUploadAsync(string media_id,TwitterChannelDto twitterChannelDto)
        {
            try
            {
                
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(twitterChannelDto.TwitterImageAPI, null, "POST", twitterChannelDto));

                var requestData = new Dictionary<string, string>
                    {
                        { "command", "FINALIZE" },
                        { "media_id", media_id },
           
                    };

                // Create a MultipartFormDataContent to send the request data and media segment
                var multipartContent = new MultipartFormDataContent();
                foreach (var entry in requestData)
                {
                    multipartContent.Add(new StringContent(entry.Value), $"\"{entry.Key}\"");
                }


                HttpResponseMessage response = await asyncRetryPolicy.ExecuteAsync(async () =>
                {

                    // Send the POST request to append the media segment
                    var response = await httpClient.PostAsync(twitterChannelDto.TwitterImageAPI, multipartContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;

                    }
                    else
                    {
                        //Log.Information("___Retrying => {@result}", response.StatusCode);
                        throw new HttpRequestException($"Request failed ");
                    }
                });
                var httpContent = await  response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    // Handle the error response here
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }
        public async Task<Tuple<int, string>> AppendMediaChunksAsync(string media_id, byte[] data,  TwitterChannelDto twitterChannelDto)
        {
            try
            {
                int segmentIndex = 0;
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(twitterChannelDto.TwitterImageAPI, null, "POST", twitterChannelDto));

                var requestData = new Dictionary<string, string>
                    {
                        { "command", "APPEND" },
                        { "media_id", media_id },
                        { "segment_index", segmentIndex.ToString() }
                    };

                // Create a MultipartFormDataContent to send the request data and media segment
                var multipartContent = new MultipartFormDataContent();
                foreach (var entry in requestData)
                {
                    multipartContent.Add(new StringContent(entry.Value), $"\"{entry.Key}\"");
                }

                // Add the media segment as a ByteArrayContent
                var mediaSegmentContent = new ByteArrayContent(data);
                multipartContent.Add(mediaSegmentContent, "media");

                // Send the POST request to append the media segment
                HttpResponseMessage response = await asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    var response = await httpClient.PostAsync(twitterChannelDto.TwitterImageAPI, multipartContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;

                    }
                    else
                    {
                        //Log.Information("___Retrying => {@result}", response.StatusCode);
                        throw new HttpRequestException($"Request failed {response.ReasonPhrase} ");
                    }
                });
                var responseContent = await response.Content.ReadAsStringAsync();
                return new Tuple<int, string>(
                  (int)response.StatusCode,
                  responseContent
                  );
               
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
        async Task<Tuple<int, string>> SendText(TwitterChannelDto twiiterChannel, Dictionary<string, object> textData)
        {
            using (var httpClient = new HttpClient())
            {


                var requestJson = JsonConvert.SerializeObject(textData);
                // Set the Authorization header
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", twiiterChannel.UserAccessToken);

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    var response = await httpClient.PostAsync(twiiterChannel.TwitterTextAPI, requestContent);
                    var httpContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return response;

                    }
                    else
                    {
                        //Log.Information("___Retrying => {@result}", response.StatusCode);
                        throw new HttpRequestException($"Request failed {response.ReasonPhrase} ");
                    }
                });
                var httpContent = await response.Content.ReadAsStringAsync();

                return new Tuple<int, string>((int)response.StatusCode, httpContent);
            }
        }

        async Task<Tuple<int, string>> SendImage(TwitterChannelDto twitterChannelDto, MultipartFormDataContent multipartContent)
        {
            using (var httpClient = new HttpClient())
            {
                

                httpClient.DefaultRequestHeaders.Add("Authorization", PrepareOAuth(twitterChannelDto.TwitterImageAPI, null, "POST",twitterChannelDto));
                HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
                {

                    var httpResponse = await httpClient.PostAsync(twitterChannelDto.TwitterImageAPI, multipartContent);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return httpResponse;

                    }
                    else
                    {
                        //Log.Information("___Retrying => {@result}", response.StatusCode);
                        throw new HttpRequestException($"Request failed {httpResponse.ReasonPhrase} ");
                    }
                });
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }

        #region Some OAuth magic
        string PrepareOAuth(string URL, Dictionary<string, string> data, string httpMethod,TwitterChannelDto twitterChannelDto)
        {
            // seconds passed since 1/1/1970
            var timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);

            // Add all the OAuth headers we'll need to use when constructing the hash
            Dictionary<string, string> oAuthData = new Dictionary<string, string>();
            oAuthData.Add("oauth_consumer_key", twitterChannelDto.ConsumerKey);
            oAuthData.Add("oauth_signature_method", "HMAC-SHA1");
            oAuthData.Add("oauth_timestamp", timestamp.ToString());
            oAuthData.Add("oauth_nonce", Guid.NewGuid().ToString());
            oAuthData.Add("oauth_token", twitterChannelDto.AccessToken);
            oAuthData.Add("oauth_version", "1.0");

            if (data != null) // add text data too, because it is a part of the signature
            {
                foreach (var item in data)
                {
                    oAuthData.Add(item.Key, item.Value);
                }
            }

            // Generate the OAuth signature and add it to our payload
            oAuthData.Add("oauth_signature", GenerateSignature(URL, oAuthData, httpMethod,twitterChannelDto.AccessTokenSecret, twitterChannelDto.ConsumerSecret));

            // Build the OAuth HTTP Header from the data
            return GenerateOAuthHeader(oAuthData);
        }

        /// <summary>
        /// Generate an OAuth signature from OAuth header values
        /// </summary>
        string GenerateSignature(string url, Dictionary<string, string> data, string httpMethod, string oauth_token_secret,string ConsumerKeySecret)
        {
            var _sigHasher = new HMACSHA1(
               new ASCIIEncoding().GetBytes($"{ConsumerKeySecret}&{oauth_token_secret}")
           );
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format("{0}&{1}&{2}",
                httpMethod,
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString()
                )
            );

            return Convert.ToBase64String(
                _sigHasher.ComputeHash(
                    new ASCIIEncoding().GetBytes(fullSigData.ToString())
                )
            );
        }

        /// <summary>
        /// Generate the raw OAuth HTML header from the values (including signature)
        /// </summary>
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
        #endregion

        // dépasse pas une limite de caractères en supprimant les mots à partir de la fin du tweet jusqu'à ce qu'il atteigne la limite.
        string CutTweetToLimit(string tweet)
        {
            while (tweet.Length >= _limit)
            {
                tweet = tweet.Substring(0, tweet.LastIndexOf(" ", StringComparison.Ordinal));
            }
            return tweet;
        }
    }
} 



