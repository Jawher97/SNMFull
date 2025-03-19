using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Polly.Retry;
using Polly;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using SNM.Publishing.Aggregator.DataContext;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using SNM.Publishing.Aggregator.Configurations.Extensions;
using SNM.Publishing.Aggregator.Exceptions.Model;

namespace SNM.Publishing.Aggregator.Services
{
    public class PostTwitterService : IPostTwitterServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;

        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public PostTwitterService(ApplicationDbContext context, IConfiguration configuration, ILogger<IPostInstagramServices> logger, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;


            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryCount: 3, count => TimeSpan.FromMilliseconds(1), onRetry: (exception, count, context) =>
                {
                    Log.Information("___Retrying____ => {@result}", count);
                    Log.Information("_______DateTime _____=> {@result}", DateTime.Now);

                });

        }
        //public async Task<TwitterChannel?> GetTwitterchannelByChannelId(Guid ChannelId)
        //{
        //    return await _context.Set<TwitterChannel>().FirstOrDefaultAsync(twitterchannel => twitterchannel.ChannelId == ChannelId);
        //}

        public async Task<Response<TwitterPostDto>> PublishTwitterPost(TwitterPostDto twitterPost, Guid ChanbelId)
        {
            Response<TwitterPostDto> result = new Response<TwitterPostDto>();
            try
            {
                string TwitterPublishingUrl = _configuration["ApiSettings:TwitterPublishingUrl"] + ChanbelId;
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ApiSettings:TwitterUrl"]);

                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(twitterPost);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        HttpResponseMessage response = await client.PostAsync(TwitterPublishingUrl, content);

                        // Check if the request was successful (status code in the 2xx range)
                        if (response.IsSuccessStatusCode)
                        {
                            // The request was successful
                            return response;
                        }
                        else
                        {
                            // Handle the case where the request was not successful
                            Log.Information("___Retrying => {@result}", response.StatusCode);
                            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Information("___Retrying => {@result}", ex.Message);
                        throw; // Re-throw the caught exception
                    }


                });


                // Use the retry policy to send the request

                if (httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    result.Succeeded = true;
                    result.Data = await httpResponse.ReadContentAs<TwitterPostDto>();

                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Append(httpResponse.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Append(ex.Message);
            }

            return result;

        }

        public async Task<Response<PostDto>> GeTwitterChannels(ChannelDto Channel)
        {
            Response<PostDto> result = new Response<PostDto>();

            try
            {



                string LastPostUrl = "apitwitter/v1/TwitterAPI/GetLastTweet?channelId=" + Channel.Id;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:TwitterUrl"]);
                // Serialize the data to JSON (assuming JSON data)
             


                // Send the POST request
                HttpResponseMessage response = await _client.GetAsync(LastPostUrl);

                // Check if the request was successful (status code in the 2xx range)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the entire response into an intermediate object
                    var responseObject = JsonConvert.DeserializeObject<Response<PostDto>>(responseContent)?.Data;

                    // Use the deserialized PostDto
                    if (responseObject != null)
                    {
                        result.Succeeded = true;
                        result.Data = responseObject;
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Message = "Response does not contain valid data.";
                    }

                }
                else
                {
                    result.Succeeded = false;
                    result.Message = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }

            return result;

        }

        public async Task<Response<string>> PublishSchedulePostTwitter(TwitterPostDto twitterPost, Guid channelId)
        {
            Response<string> result = new Response<string>();
            try
            {
                string LinkedinPublishingUrl = _configuration["ApiSettings:TwitterSchedulePost"] + channelId;

                // Use the IHttpClientFactory to create an HttpClient
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ApiSettings:JobUrl"]);

                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(twitterPost);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                //HttpResponseMessage httpResponse = await
                //             asyncRetryPolicy.ExecuteAsync(async () =>
                //             {


                                 HttpResponseMessage httpResponse = await client.PostAsync(LinkedinPublishingUrl, content);

                                 //Check if the request was successful(status code in the 2xx range)

                                 //The request was successful
                             //    if (response.IsSuccessStatusCode)
                             //    {
                             //        // The request was successful
                             //        return response;
                             //    }
                             //    else
                             //    {
                             //        // Handle the case where the request was not successful
                             //        Log.Information("___Retrying => {@result}", response.StatusCode);
                             //        throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                             //    }


                             //});
                var http = httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    result.Succeeded = true;
                  //  result.Data = await httpResponse.ReadContentAs<TwitterPostDto>();

                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Append(httpResponse.StatusCode.ToString());
                }
            }
                catch (Exception ex)
                {
                    result.Succeeded = false;
                    result.Errors.Append(ex.Message);
                }

                return result;
        }
    }
}

