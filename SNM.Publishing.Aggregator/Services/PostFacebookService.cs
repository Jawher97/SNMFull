using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Serilog;
using SNM.Publishing.Aggregator.Configurations.Extensions;
using SNM.Publishing.Aggregator.Exceptions.Model;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Models;
using System.Net.Http;
using System.Text;

namespace SNM.Publishing.Aggregator.Services
{

    public class PostFacebookService : IPostFacebookService
    {
        private readonly HttpClient _client;

        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;

        public PostFacebookService(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client ?? throw new ArgumentNullException(nameof(client));
            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
              .WaitAndRetryAsync(retryCount: 3, count => TimeSpan.FromMilliseconds(1), onRetry: (exception, count, context) =>
              {
                  Log.Information("___Retrying____ => {@result}", count);
                  Log.Information("_______DateTime _____=> {@result}", DateTime.Now);

              });
        }

        public async Task<Response<FacebookPostDto>> PublishFacebookPost(FacebookPostDto facebookPost, Guid channelId)
        {
            Response<FacebookPostDto> result = new Response<FacebookPostDto>();
            try
            {
                string facebookPublishingUrl = "/api/v1/FacebookPostAPI/PublishPostToFacebook?channelId=" + channelId;
                var json = JsonConvert.SerializeObject(facebookPost);
               
                var content = new StringContent(json, Encoding.UTF8, "application/json");            
                HttpResponseMessage response = await _client.PostAsync(facebookPublishingUrl, content);              
                if (response.IsSuccessStatusCode)
                {
                    
                    result.Succeeded = true;
                    result.Data = await response.ReadContentAs<FacebookPostDto>();

                }
                else
                {
                    result.Succeeded = false;
                    result.Errors.Append(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Append(ex.Message);
            }

            return result;

        }
        public async Task<Response<PostDetalisDto>> GeFacebookChannels(ChannelDto Channel)
        {
            Response<PostDetalisDto> result = new Response<PostDetalisDto>();

            try
            {



                string LastPostUrl = "api/v1/FacebookPostAPI/GetLatestPostFieldsByPageId?channelId=" + Channel.Id;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);

                HttpResponseMessage response = await _client.GetAsync(LastPostUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
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

        public async Task<Response<string>> PublishScheduleFacebookPost(FacebookPostDto facebooPost, Guid channelId)
        {
            Response<string> result = new Response<string>();
            try
            {

                string LinkedinPublishingUrl = _configuration["ApiSettings:FacebookSchedulePost"] + channelId;
                _client.BaseAddress = new Uri(_configuration["ApiSettings:JobUrl"]);
                var json = JsonConvert.SerializeObject(facebooPost);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = await
                             asyncRetryPolicy.ExecuteAsync(async () =>
                             {


                                 HttpResponseMessage response = await _client.PostAsync(LinkedinPublishingUrl, content);                             
                                 if (response.IsSuccessStatusCode)
                                 {                                
                                     return response;
                                 }
                                 else
                                 {
                                     Log.Information("___Retrying => {@result}", response.StatusCode);
                                     throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                                 }


                             });
                var http = httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    result.Succeeded = true;

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
                string CommentPostUrl = "api/v1/FacebookPostAPI/CreateComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);                
                var json = JsonConvert.SerializeObject(comment);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(CommentPostUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<Response<CommentDto>>(responseContent)?.Data;
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



                    string CommentPostUrl = $"api/v1/FacebookPostAPI/DeleteComment?channelId={channelId}";

                    var _client = new HttpClient();
                    _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);



                    var json = JsonConvert.SerializeObject(comment);

                    // Create a StringContent object with the JSON data and specify the content type
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PostAsync(CommentPostUrl, content);
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

        public async Task<Response<CommentDto>> CreateSubComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();

            try
            {



                string CommentPostUrl = "api/v1/FacebookPostAPI/CreateSubComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);

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

        public async  Task<Response<CommentDto>> UpdateComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<CommentDto> result = new Response<CommentDto>();

            try
            {



                string CommentPostUrl = "api/v1/FacebookPostAPI/UpdateComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);

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

        public async Task<Response<string>> CreateRectionComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/FacebookPostAPI/CreateReactionComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);
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

        public async Task<Response<string>> DeleteReactionComment(CommentDetailsDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/FacebookPostAPI/DeleteReactionComment?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);
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

        public async Task<Response<string>> CreateRectionPost(PostDetalisDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/FacebookPostAPI/CreateReactionPost?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);
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

        public async Task<Response<string>> DeleteReactionPost(PostDetalisDto comment, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/FacebookPostAPI/DeleteReactionPost?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);
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

        public async Task<Response<string>> DeletePost(PostDetalisDto post, Guid channelId)
        {
            Response<string> result = new Response<string>();

            try
            {



                string CommentPostUrl = "api/v1/FacebookPostAPI/DeletePost?channelId=" + channelId;
                var _client = new HttpClient();
                _client.BaseAddress = new Uri(_configuration["ApiSettings:FacebookUrl"]);
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
    }
    }

