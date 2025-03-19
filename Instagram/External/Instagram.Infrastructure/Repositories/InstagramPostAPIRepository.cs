using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg.OpenPgp;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Serilog;
using Polly;
using Polly.Retry;
using Microsoft.AspNetCore.Http;
using SNM.Instagram.Application.Exceptions.Model;

namespace SNM.Instagram.Infrastructure.Repositories
{

    public class InstagramPostAPIRepository:IInstagramPostApiRepository
    {
        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;
        public InstagramPostAPIRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryCount: 3, count => TimeSpan.FromMilliseconds(60), onRetry: (exception, count, context) =>
                {
                    Log.Information("___Retrying____ => {@result}", count);
                    Log.Information("_______DateTime _____=> {@result}", DateTime.Now);
                    //logger.LogInformation($"___Retrying:{count} of 3 ___");
                });
        }

     

        public async Task<Response<string>> PublishPostToInstagram(InstagramPostDto instagramPostDto)
        {
            Response<string> Msg = new Response<string>();
                   var httpClientmedia = new HttpClient();
                  
                   List<string> builderIds = new List<string>();
                 
                   httpClientmedia.BaseAddress = new Uri(instagramPostDto.InstagramChannelDto.ChannelAPI);
                   httpClientmedia.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", instagramPostDto.InstagramChannelDto.UserAccessToken);
                   HttpResponseMessage response; 
                                               
                   // relative to Create Carousel 
                   var postDataimages = new Dictionary<string, string> ();
                    // for one image or video
                    // if multiple image
                   if (instagramPostDto.PostDto.MediaData.Count() > 1 )
                      {
                    if (instagramPostDto.PostDto.MediaData.Count() <= 10)
                    {
                        foreach (var image in instagramPostDto.PostDto.MediaData)
                        {


                        // for only image
                        var postData = new Dictionary<string, string>();
                        postData["is_carousel_item"] = "true";
                        postData["published_shed"] = "1";

                        if (image.Media_type.ToString() == "IMAGE")
                        {
                            // imageUrl = ConvertImageBase64ToUrl(image);
                            //postData["image_url"] = image.Media_url;
                            postData["image_url"] = image.Media_url;

                        }
                        else if (image.Media_type.ToString() == "VIDEO")
                        {
                            // videoUrl = ConvertImageBase64ToUrl(image);
                            // postData["video_url"] = videoUrl;
                            postData["video_url"] = image.Media_url;
                            postData["media_type"] = image.Media_type.ToString();

                        }
                        // get first image child

                        var childid = await GetChildrenId(postData, instagramPostDto, $"{instagramPostDto.InstagramChannelDto.UserId}/media", instagramPostDto.InstagramChannelDto.UserAccessToken, instagramPostDto.InstagramChannelDto.ChannelAPI);

                        JObject jObjectchild = JObject.Parse(childid);
                        if (jObjectchild.ContainsKey("id"))
                        {
                            // Extract the "id" field as a string
                            string id = jObjectchild["id"].Value<string>();


                            builderIds.Add(id);


                        }

                    }


                    // create carousel for multiple image

                    postDataimages["children"] = string.Join(",", builderIds);
                    postDataimages["media_type"] = "CAROUSEL";
                    postDataimages["caption"] = instagramPostDto.Caption;

                    // creation de carousel
                    var jObject = await CreateCarousel(postDataimages, instagramPostDto, $"{instagramPostDto.InstagramChannelDto.UserId}/media", instagramPostDto.InstagramChannelDto.UserAccessToken, instagramPostDto.InstagramChannelDto.ChannelAPI);

                    JObject jObjectCarosel = JObject.Parse(jObject);
                    if (jObjectCarosel.ContainsKey("id"))
                    {
                        // Extract the "id" field as a string
                        string creationId = jObjectCarosel["id"].Value<string>();
                        // publish to instagram
                        //await Task.Delay(6000);
                        HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
                               {
                                   response = await httpClientmedia.PostAsync($"{instagramPostDto.InstagramChannelDto.UserId}/media_publish?creation_id={creationId}", null);
                                   //var httpContent = await response.Content.ReadAsStringAsync();
                                  
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
                           );
                        Msg = HandleResponseException(httpResponse,  await httpResponse.Content.ReadAsStringAsync());
                        //  Msg = httpResponse.Content.ToString();

                    }
                }
                else
                {
                    Msg.Succeeded = false;
                    Msg.Message = "You may publish up to 10 images, videos, or a mix of the two in a single post";
                }
                                       
        }

               // for one image or video
                else
                  {
                                    
                    if (instagramPostDto.PostDto.MediaData.FirstOrDefault().Media_type.ToString() == "IMAGE")
                      {
                              // imageUrl = ConvertImageBase64ToUrl(instagramPostDto.PostDto.MediaData.FirstOrDefault());
                                 postDataimages["image_url"] = instagramPostDto.PostDto.MediaData.FirstOrDefault().Media_url;
                    // update
                                //postDataimages["image_url"] = instagramPostDto.InstagramImagesDto.FirstOrDefault().Media_url;

                      }
                    else if (instagramPostDto.PostDto.MediaData.FirstOrDefault().Media_type.ToString() == "VIDEO")
                      {     

                                // videoUrl = ConvertImageBase64ToUrl(instagramPostDto.InstagramImagesDto.FirstOrDefault());
                                //  postDataimages["video_url"] = videoUrl;
                                postDataimages["video_url"] = instagramPostDto.PostDto.MediaData.FirstOrDefault().Media_url;
                                postDataimages["media_type"] = instagramPostDto.PostDto.MediaData.FirstOrDefault().Media_type.ToString();

                       }
                      postDataimages["caption"] = instagramPostDto.Caption;                              
                      var childid = await GetChildrenId(postDataimages, instagramPostDto, $"{instagramPostDto.InstagramChannelDto.UserId}/media", instagramPostDto.InstagramChannelDto.UserAccessToken, instagramPostDto.InstagramChannelDto.ChannelAPI);                                                 
                      JObject jObjectchild = JObject.Parse(childid);
                        if (jObjectchild.ContainsKey("id"))
                        {
                            // Extract the "id" field as a string
                            string id = jObjectchild["id"].Value<string>();
                            HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
                            {
                                response = await httpClientmedia.PostAsync($"{instagramPostDto.InstagramChannelDto.UserId}/media_publish?creation_id={id}", null);
                                //var httpContent = await response.Content.ReadAsStringAsync();
                        
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
                            Msg = HandleResponseException(httpResponse,await httpResponse.Content.ReadAsStringAsync());
                }
                                    
                }

                                  
            return Msg;
        }

        public string ConvertImageBase64ToUrl(InstagramImageDto instagramImageDto)
        {
            var data = instagramImageDto.Media_url.Split(',')[1];
            string fileName = "";
            // Decode the base64 data
            byte[] imageBytes = Convert.FromBase64String(data);

            // Define the path and file name for the saved image
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if(instagramImageDto.Media_type == Domain.Enumeration.MediaTypeEnum.IMAGE)
           {  fileName = Guid.NewGuid().ToString() + ".jpg"; }
            else { fileName = Guid.NewGuid().ToString() + ".mp4";
            }
            string outputFilePath = Path.Combine(outputDirectory, fileName);

            // Ensure the output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Save the image to the specified path
            System.IO.File.WriteAllBytes(outputFilePath, imageBytes);

            // Generate a URL pointing to the saved image
            string baseUrl = _configuration["AppBaseUrl"]; 
            string imageRelativePath = Path.Combine("uploads", fileName);
            string imageUrl = baseUrl + "/" + imageRelativePath.Replace("\\", "/"); 
            return imageUrl;

        }
        public async Task<string>  CreateCarousel(Dictionary<string, string> postData, InstagramPostDto instagramPostDto, string PostToPageURL, string userAccessToken, string apiBaseUrl)
                {
            using (var httpClient = new HttpClient())
            {
                
                // Configurer HttpClient
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);
                HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    var response = await httpClient.PostAsync(PostToPageURL, new FormUrlEncodedContent(postData));
                        var httpContent = await response.Content.ReadAsStringAsync();
                        HandleResponseException(response, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;

                    }
                    else
                    {
                        //Log.Information("___Retrying => {@result}", response.StatusCode);
                        throw new HttpRequestException($"Request failed to createCarousel");
                    }
                 });
                    // Analyser la réponse JSON
                 var json = await httpResponse.Content.ReadAsStringAsync();

                 var jObject = JObject.Parse(json);
                 return jObject.ToString();
             }
        }
        public async Task<string> GetChildrenId(Dictionary<string, string> postData,InstagramPostDto instagramPostDto,string PostToPageURL,string userAccessToken,string apiBaseUrl)
        {
            
            using (var httpClient = new HttpClient())
            {
                // Configurer HttpClient
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);
                //postData["published_shed"] = "1";
                // Envoyer la demande HTTP et récupérer la réponse
                HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    var response = await httpClient.PostAsync(PostToPageURL, new FormUrlEncodedContent(postData));
                    var httpContent = await response.Content.ReadAsStringAsync();
                    HandleResponseException(response, httpContent);
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
                    // Analyser la réponse JSON
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(json);
                return jObject.ToString();
            }

        }
        private Response<string> HandleResponseException(HttpResponseMessage response, string content)
        {
            Response<string> resp = new Response<string>();
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Request failed with status code {response.StatusCode}. Response content: {content}";
                //throw new Exception(errorMessage);
                resp.Succeeded = false;
                resp.Message = errorMessage;
                return resp;
            }
            resp.Succeeded = true;
            resp.Message = content;
            return resp;
            
        }
        //public async Task<Tuple<int, string>> PublishPost(InstagramChannelPostDto instagramChannelPostDto)
        //{


        //    using (var http = new HttpClient())
        //    {

        //        var postData = new Dictionary<string, string> {
        //            { "access_token", instagramChannelPostDto.InstagramChannelDto.ChannelAccessToken },
        //            { "created_time", instagramChannelPostDto.CreatedTime.ToString() },

        //            { "link", instagramChannelPostDto.Link },
        //            { "message", instagramChannelPostDto.Message },

        //            { "picture", instagramChannelPostDto.Picture },

        //            { "type", instagramChannelPostDto.Type.ToString() },
        //            { "updated_time", instagramChannelPostDto.UpdatedTime.ToString() },


        //        };

        //        var httpResponse = await http.PostAsync(
        //            instagramChannelPostDto.InstagramChannelDto.PostToPageURL,
        //            new FormUrlEncodedContent(postData)
        //            );
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>(
        //            (int)httpResponse.StatusCode,
        //            httpContent
        //            );
        //    }
        //}

        //public async Task<Tuple<int, string>> PublishSimplePost(InstagramChannelDto instagramChannelDto, string postText)
        //{
        //    using (var http = new HttpClient())
        //    {
        //        var postData = new Dictionary<string, string> {
        //        { "access_token", instagramChannelDto.ChannelAccessToken },
        //        { "message", postText }//,
        //        // { "formatting", "MARKDOWN" } // doesn't work
        //    };

        //        var httpResponse = await http.PostAsync(
        //            instagramChannelDto.PostToPageURL,
        //            new FormUrlEncodedContent(postData)
        //            );
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>(
        //            (int)httpResponse.StatusCode,
        //            httpContent
        //            );
        //    }
        //}

        //public async Task<string> PublishToInstagram(InstagramChannelDto instagramChannelDto, string postText, string pictureURL)
        //{

        //    try
        //    {
        //        // upload picture first
        //        var rezImage = Task.Run(async () =>
        //        {
        //            using (var http = new HttpClient())
        //            {
        //                return await UploadPhoto(instagramChannelDto, pictureURL);
        //            }
        //        });
        //        var rezImageJson = JObject.Parse(rezImage.Result.Item2);

        //        if (rezImage.Result.Item1 != 200)
        //        {
        //            try // return error from JSON
        //            {
        //                return $"Error uploading photo to Instagram. {rezImageJson["error"]["message"].Value<string>()}";
        //            }
        //            catch (Exception ex) // return unknown error
        //            {
        //                // log exception somewhere
        //                return $"Unknown error uploading photo to Instagram. {ex.Message}";
        //            }
        //        }
        //        // get post ID from the response
        //        string postID = rezImageJson["post_id"].Value<string>();

        //        // and update this post (which is actually a photo) with your text
        //        var rezText = Task.Run(async () =>
        //        {
        //            using (var http = new HttpClient())
        //            {
        //                return await UpdatePhotoWithPost(instagramChannelDto, postID, postText);
        //            }
        //        });
        //        var rezTextJson = JObject.Parse(rezText.Result.Item2);

        //        if (rezText.Result.Item1 != 200)
        //        {
        //            try // return error from JSON
        //            {
        //                return $"Error posting to Instagram. {rezTextJson["error"]["message"].Value<string>()}";
        //            }
        //            catch (Exception ex) // return unknown error
        //            {
        //                // log exception somewhere
        //                return $"Unknown error posting to Instagram. {ex.Message}";
        //            }
        //        }

        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        // log exception somewhere
        //        return $"Unknown error publishing post to Instagram. {ex.Message}";
        //    }
        //}


        //public async Task<Tuple<int, string>> UploadPhoto(InstagramChannelDto instagramChannelDto, string photoURL)
        //{
        //    using (var http = new HttpClient())
        //    {
        //        var postData = new Dictionary<string, string> {
        //        { "access_token", instagramChannelDto.ChannelAccessToken },
        //        { "url", photoURL }
        //    };

        //        var httpResponse = await http.PostAsync(
        //           instagramChannelDto.PostToPagePhotosURL,
        //            new FormUrlEncodedContent(postData)
        //            );
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>(
        //            (int)httpResponse.StatusCode,
        //            httpContent
        //            );
        //    }
        //}


        //public async Task<Tuple<int, string>> UpdatePhotoWithPost(InstagramChannelDto instagramChannelDto, string postID, string postText)
        //{

        //    using (var http = new HttpClient())
        //    {
        //        var postData = new Dictionary<string, string> {
        //        { "access_token", instagramChannelDto.ChannelAccessToken },
        //        { "message", postText }//,
        //        // { "formatting", "MARKDOWN" } // doesn't work
        //    };

        //        var httpResponse = await http.PostAsync(
        //            $"{instagramChannelDto.ChannelAPI}{postID}",
        //            new FormUrlEncodedContent(postData)
        //            );
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>(
        //            (int)httpResponse.StatusCode,
        //            httpContent
        //            );
        //    }
        //}

       
    }
}
