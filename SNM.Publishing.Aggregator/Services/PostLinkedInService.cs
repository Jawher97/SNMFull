using Newtonsoft.Json;
using Polly.Retry;
using Polly;
using SNM.Publishing.Aggregator.Configurations.Extensions;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using System.Text;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace SNM.Publishing.Aggregator.Services
{
    public class PostLinkedInService : IPostLinkedInService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;

        private readonly IConfiguration _configuration;

        public PostLinkedInService(IConfiguration configuration, ILogger<PostLinkedInService> logger, IHttpClientFactory httpClientFactory)
        {

            _configuration = configuration;
            _httpClientFactory = httpClientFactory;


            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryCount: 3, count => TimeSpan.FromMilliseconds(1), onRetry: (exception, count, context) =>
                {
                    Log.Information("___Retrying____ => {@result}", count);
                    Log.Information("_______DateTime _____=> {@result}", DateTime.Now);

                });

        }

        public async Task<Response<LinkedInPostDto>> PublishPostLinkedIn(LinkedInPostDto linkedInPost, Guid ChannelId)
        {
            //var response = await _client.GetAsync($"/apiLinkedIn/v1/LinkedIn");
            //return await response.ReadContentAs<PostDto>();
            Response<LinkedInPostDto> result = new Response<LinkedInPostDto>();
            try
            {

                string LinkedinPublishingUrl = _configuration["ApiSettings:LinkedInPublishingUrl"] + ChannelId;

                // Use the IHttpClientFactory to create an HttpClient
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);

                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(linkedInPost);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage httpResponse = await
                             asyncRetryPolicy.ExecuteAsync(async () =>
                             {


                                 HttpResponseMessage response = await client.PostAsync(LinkedinPublishingUrl, content);

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
                                     throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                                 }


                             });
                var http = httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    result.Succeeded = true;
                    result.Data = await httpResponse.ReadContentAs<LinkedInPostDto>();

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
        // Use the retry policy to send the request



        public async Task<Response<PostDetalisDto>> GeLinkedinChannels(ChannelDto Channel)
        {
            Response<PostDetalisDto> result = new Response<PostDetalisDto>();

            try
            {



                string LastPostUrl = "apiLinkedIn/v1/LinkedInAPI/GetLatestPost?channelId=" + Channel.Id;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);
                // Serialize the data to JSON (assuming JSON data)



                // Send the POST request
                HttpResponseMessage response = await _client.GetAsync(LastPostUrl);

                // Check if the request was successful (status code in the 2xx range)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the entire response into an intermediate object
                    var responseObject = JsonConvert.DeserializeObject<Response<PostDetalisDto>>(responseContent)?.Data;

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

        public async Task<Response<string>> PublishSchedulePostLinkedIn(LinkedInPostDto LinkedInPost, Guid channelId)
        {
            Response<string> result = new Response<string>();
            try
            {

                string LinkedinPublishingUrl = _configuration["ApiSettings:LinkedInSchedulePost"] + channelId;

                // Use the IHttpClientFactory to create an HttpClient
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ApiSettings:JobUrl"]);

                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(LinkedInPost);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage httpResponse = await
                             asyncRetryPolicy.ExecuteAsync(async () =>
                             {


                                 HttpResponseMessage response = await client.PostAsync(LinkedinPublishingUrl, content);




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
                    // result.Data = await httpResponse.ReadContentAs<string>();

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

        public async Task<Response<CommentDto>> CreateComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();

            try
            {



                string CommentPostUrl = "api/v1/LinkedinPostAPI/CreateComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);
                // Serialize the data to JSON (assuming JSON data)



                // Send the POST request
                // Serialize the data to JSON (assuming JSON data)
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

        public async Task<Response<string>> DeleteComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = $"api/v1/LinkedinPostAPI/DeleteComment?channelId={channelId}";

                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);



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
                    var responseObject = JsonConvert.DeserializeObject<Response<string>>(responseContent)?.Data;

                    // Use the deserialized PostDto
                    if (responseObject != null)
                    {
                        result.Succeeded = true;
                        result.Data = responseObject.ToString();
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

        public async Task<Response<CommentDto>> UpdateComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();

            try
            {



                string CommentPostUrl = "api/v1/LinkedinPostAPI/UpdateComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);

                var json = JsonConvert.SerializeObject(comment);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");




                HttpResponseMessage response = await _client.PutAsync(CommentPostUrl, content);


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



                string CommentPostUrl = "api/v1/LinkedinPostAPI/CreateSubComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);

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

        public async Task<Response<string>> CreateRectionComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/LinkedinPostAPI/CreateReactionComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);
                // Serialize the data to JSON (assuming JSON data)



                // Send the POST request
                // Serialize the data to JSON (assuming JSON data)
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
                    var responseObject = JsonConvert.DeserializeObject<Response<string>>(responseContent);

                    result.Succeeded = true;
                    result.Message = responseObject.Message;

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
    

        public async Task<Response<string>> DeleteReactionComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/LinkedinPostAPI/DeleteReactionComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);
                // Serialize the data to JSON (assuming JSON data)



                // Send the POST request
                // Serialize the data to JSON (assuming JSON data)
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
                    var responseObject = JsonConvert.DeserializeObject<Response<string>>(responseContent);

                    result.Succeeded = true;
                    result.Message = responseObject.Message;

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
    

        public async Task<Response<string>> CreateRectionPost(PostDetalisDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/LinkedinPostAPI/CreateReactionPost?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);
                // Serialize the data to JSON (assuming JSON data)



                // Send the POST request
                // Serialize the data to JSON (assuming JSON data)
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
                    var responseObject = JsonConvert.DeserializeObject<Response<string>>(responseContent);

                    // Use the deserialized PostDto
                    result.Succeeded = true;
                    result.Message = responseObject.Message;

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
    

        public async Task<Response<string>> DeleteReactionPost(PostDetalisDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/LinkedinPostAPI/DeleteReactionPost?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);
                // Serialize the data to JSON (assuming JSON data)



                // Send the POST request
                // Serialize the data to JSON (assuming JSON data)
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
                    var responseObject = JsonConvert.DeserializeObject<Response<string>>(responseContent);

                    result.Succeeded = true;
                    result.Message = responseObject.Message;

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

        public async  Task<Response<string>> DeletePost(PostDetalisDto post, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/LinkedinPostAPI/DeletePost?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:LinkedInUrl"]);
                // Serialize the data to JSON (assuming JSON data)



                // Send the POST request
                // Serialize the data to JSON (assuming JSON data)
                var json = JsonConvert.SerializeObject(post);

                // Create a StringContent object with the JSON data and specify the content type
                var content = new StringContent(json, Encoding.UTF8, "application/json");




                HttpResponseMessage response = await _client.PostAsync(CommentPostUrl, content);


                // Check if the request was successful (status code in the 2xx range)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the entire response into an intermediate object
                    var responseObject = JsonConvert.DeserializeObject<Response<string>>(responseContent);

                    result.Succeeded = true;
                    result.Message = responseObject.Message;

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

