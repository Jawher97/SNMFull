using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Serilog;
using SNM.Publishing.Aggregator.Configurations.Extensions;
using SNM.Publishing.Aggregator.DataContext;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace SNM.Publishing.Aggregator.Services
{
    public class PostInstagramServices : IPostInstagramServices
    {

       
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;
      
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public PostInstagramServices(ApplicationDbContext context,IConfiguration configuration, ILogger<IPostInstagramServices> logger, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            
           
            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryCount: 3,count => TimeSpan.FromMilliseconds(1), onRetry: (exception, count, context) =>
                {
                    Log.Information("___Retrying____ => {@result}", count);
                    Log.Information("_______DateTime _____=> {@result}", DateTime.Now);

                });
           
        }
        
        

       
      

        public async Task<Response<InstagramPostDto>> PublishInstagramPost(InstagramPostDto instagramPost,Guid ChannelId)
        {
            Response<InstagramPostDto> result = new Response<InstagramPostDto>();
            try
            {

               
                string instagramPublishingUrl = _configuration["ApiSettings:InstagramPublishingUrl"]+ ChannelId;

                // Use the IHttpClientFactory to create an HttpClient
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ApiSettings:InstagramUrl"]);
               
                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(instagramPost);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");

              
                HttpResponseMessage httpResponse = await
                             asyncRetryPolicy.ExecuteAsync(async () =>
                             {


                                 HttpResponseMessage response = await client.PostAsync(instagramPublishingUrl, content);
                                 result = await response.ReadContentAs<Response<InstagramPostDto>>();
                                 // Check if the request was successful (status code in the 2xx range)

                                 // The request was successful
                                 if (response.IsSuccessStatusCode)
                                 {
                                     // The request was successful
                                     return response;
                                 }
                                 else
                                 {
                                     // Handle the case where the request was not successful
                                     Log.Information("___Retrying => {@result}", response.StatusCode);
                                     throw new HttpRequestException($"Request failed with status code {response.Content.ReadAsStringAsync()} with instagram");
                                 }


                             });


                // Use the retry policy to send the request


                if (httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    result.Succeeded = true;
                    result = await httpResponse.ReadContentAs<Response<InstagramPostDto>>();

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
        public async Task<Response<PostDetalisDto>> GeInstagramChannels(ChannelDto Channel)
        {
            Response<PostDetalisDto> result = new Response<PostDetalisDto>();

            try
            {



                string LastPostUrl = "api/v1/InstagramPostAPI/GetInstagramMedia?channelId="+ Channel.Id;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:InstagramUrl"]);
                
                


             
                HttpResponseMessage response = await _client.GetAsync(LastPostUrl);


                //if (response.IsSuccessStatusCode)
                //{
                
                    string responseContent = await response.Content.ReadAsStringAsync();

                    
                    var responseObject = JsonConvert.DeserializeObject<Response<PostDetalisDto>>(responseContent)?.Data;

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

                //}
                //else
                //{
                //    result.Succeeded = false;
                //    result.Message = response.StatusCode.ToString();
                //}
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }

            return result;

        }
        

        public async Task<Response<string>> PublishSchedulePostInstagram(InstagramPostDto instagramPost, Guid channelId)
        {
            Response<string> result = new Response<string>();
            try
            {
                string LinkedinPublishingUrl = _configuration["ApiSettings:InstagramSchedulePost"] + channelId;

                // Use the IHttpClientFactory to create an HttpClient
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ApiSettings:JobUrl"]);

                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(instagramPost);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage httpResponse = await
                             asyncRetryPolicy.ExecuteAsync(async () =>
                             {


                                 HttpResponseMessage response = await client.PostAsync(LinkedinPublishingUrl, content);

                                 //Check if the request was successful(status code in the 2xx range)

                                 //The request was successful
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


                             });
                var http = httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    result.Succeeded = true;
                  //  result.Data = await httpResponse.ReadContentAs<InstagramPostDto>();

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

        public async  Task<Response<CommentDto>> CreateComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();

            try
            {



                string CommentPostUrl = "api/v1/InstagramPostAPI/CreateComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:InstagramUrl"]);

                var json = JsonConvert.SerializeObject(comment);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");




                HttpResponseMessage response = await _client.PostAsync(CommentPostUrl, content);


                // Check if the request was successful (status code in the 2xx range)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the entire response into an intermediate object
                    var responseObject = JsonConvert.DeserializeObject<Response<CommentDto>>(responseContent)?.Data;

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

        public async Task<Response<CommentDto>> CreateSubComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();

            try
            {



                string CommentPostUrl = "api/v1/InstagramPostAPI/CreateSubComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:InstagramUrl"]);

                var json = JsonConvert.SerializeObject(comment);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");




                HttpResponseMessage response = await _client.PostAsync(CommentPostUrl, content);


                // Check if the request was successful (status code in the 2xx range)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the entire response into an intermediate object
                    var responseObject = JsonConvert.DeserializeObject<Response<CommentDto>>(responseContent)?.Data;

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
    }
    }
