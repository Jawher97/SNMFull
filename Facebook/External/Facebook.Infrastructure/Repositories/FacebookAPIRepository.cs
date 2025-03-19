using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNS.Facebook.Application.DTO;
using SNS.Facebook.Application.Exceptions.Model;
using SNS.Facebook.Application.Interfaces;
using SNS.Facebook.Domain.Entities;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using Polly;
using Serilog;
using Polly.Retry;

namespace SNS.Facebook.Infrastructure.Repositories
{
    public class FacebookAPIRepository : IFacebookAPIRepository
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;
        public FacebookAPIRepository()
        {
           
            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryCount: 5, count => TimeSpan.FromMilliseconds(60), onRetry: (exception, count, context) =>
                {
                    Log.Information("___Retrying____ => {@result}", count);
                    Log.Information("_______DateTime _____=> {@result}", DateTime.Now);
                    //logger.LogInformation($"___Retrying:{count} of 3 ___");
                });
        }
        

        /// <summary>
        /// Publish a post to Facebook page
        /// </summary>
        /// <returns>Result</returns>
        /// <param name="postText">Post to publish</param>
        /// <param name="pictureURL">Post to publish</param>
        public async Task<Tuple<int, string>> PublishPostToFacebook(FacebookPostDto facebookPostDto)
        {

            if (facebookPostDto.Post.MediaData.FirstOrDefault().Media_type == Domain.Enumeration.MediaTypeEnum.VIDEO)
            {
                var tuple = await StartVideoUpload(facebookPostDto);

                return tuple;

            }
            else
            {
                using (var http = new HttpClient())
                {
                    var _postToPagePhotosURL = $"{facebookPostDto.FacebookChannel.ChannelAPI}v18.0/{facebookPostDto.FacebookChannel.SocialChannelNetwokId}/feed";
                    // upload picture first
                    var rezImages = Task.Run(async () =>
                    {
                        using (var http = new HttpClient())
                        {
                            return await UploadPhotos(facebookPostDto.FacebookChannel, facebookPostDto.Post.MediaData);
                        }
                    });

                    var postData = new Dictionary<string, string> {
                    { "access_token", facebookPostDto.FacebookChannel.ChannelAccessToken },
                    { "created_time", facebookPostDto.CreatedTime.ToString() },
                    { "link", facebookPostDto.Link },
                    { "message", facebookPostDto.Message },
                    { "type", facebookPostDto.Type.ToString() },
                    { "updated_time", facebookPostDto.UpdatedTime.ToString() },
                    { "formatting", facebookPostDto.Formatting.ToString() },
                    { "icon", facebookPostDto.Icon },
                    { "name", facebookPostDto.Name },
                     { "description", "description" },


                };
                    for (int i = 0; i < rezImages.Result.Count(); i++)
                    {
                        var rezImageJson = JObject.Parse(rezImages.Result[i].Item2);

                        if (rezImages.Result[i].Item1 == 200)
                        {
                            string mediaFbid;
                            // get ID from the response
                            mediaFbid = rezImageJson["id"].Value<string>();
                            postData.Add("attached_media[" + i + "]", "{\"media_fbid\":\"" + mediaFbid + "\"}");
                        }

                    }
                    var httpResponse = await http.PostAsync(
                      _postToPagePhotosURL,
                        new FormUrlEncodedContent(postData)
                        );
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();

                    return new Tuple<int, string>(
                        (int)httpResponse.StatusCode,
                        httpContent
                        );
                }
            }

        }




        /// <summary>
        /// Upload a picture (photo)
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="photoURL">URL of the picture to upload</param>
        public async Task<Tuple<int, string>> UploadPhoto(FacebookChannelDto facebookChannelDto, string photoURL)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", facebookChannelDto.ChannelAccessToken },
                { "url", photoURL }
            };

                var httpResponse = await http.PostAsync(
                   facebookChannelDto.PostToPagePhotosURL,
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }


        /// <summary>
        /// Upload a picture (photo)
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="photoURL">URL of the picture to upload</param>
        public async Task<List<Tuple<int, string>>> UploadPhotos(FacebookChannelDto facebookChannelDto, ICollection<MediaDto> mediaData)
        {

            var tupleList = new List<Tuple<int, string>>();
            foreach (var media in mediaData)
            {
                if (media.Media_type == Domain.Enumeration.MediaTypeEnum.IMAGE)
                {
                    using (var http = new HttpClient())
                    {
                        var postData = new Dictionary<string, string> {
                    { "access_token", facebookChannelDto.ChannelAccessToken },
                    { "url", media.Media_url },
                    { "published","false" }
                };
                        var httpResponse = await http.PostAsync(
                           facebookChannelDto.PostToPagePhotosURL,
                            new FormUrlEncodedContent(postData)
                            );
                        var httpContent = await httpResponse.Content.ReadAsStringAsync();

                        tupleList.Add(new Tuple<int, string>(
                           (int)httpResponse.StatusCode,
                           httpContent
                           ));
                    }
                }
                

            }
            return tupleList;

        }
        public async Task<Tuple<int, string>> StartVideoUpload(FacebookPostDto FacebookPostDto)
        {
            var FacebookApiUrl = $"https://graph-video.facebook.com/v18.0/{FacebookPostDto.FacebookChannel.SocialChannelNetwokId}/videos";


            using (var client = new HttpClient())
            
                using (var content = new MultipartFormDataContent())
            {

                // Convert base64 string to byte array
                byte[] videoBytes = Convert.FromBase64String(FacebookPostDto.Post.MediaData.FirstOrDefault().Media_url.Split(',')[1]);

                // Use MemoryStream to create a stream from the byte array
                using (var fileStream = new MemoryStream(videoBytes))
                {
                    content.Add(new StringContent(FacebookPostDto.FacebookChannel.ChannelAccessToken), "access_token");
                    content.Add(new StringContent(FacebookPostDto.Message), "title");
                    content.Add(new StreamContent(fileStream), "source", "video.mp4"); // You can set a desired filename here
                   
                    try
                    {
                        var response = await client.PostAsync(FacebookApiUrl, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var rezVideoJson = JObject.Parse(responseContent);

                            await UpdateVideoDescriptionAsync(rezVideoJson["id"].ToString(), FacebookPostDto.FacebookChannel.ChannelAccessToken, FacebookPostDto.Message);
                            return new Tuple<int, string>((int)response.StatusCode, responseContent);
                        }
                        return new Tuple<int, string>((int)response.StatusCode, $"Failed to upload video: {response.ReasonPhrase}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());   
                    }

                    return null;
                
                }


            }

        }
        public async Task<Tuple<int, string>> UpdateVideoDescriptionAsync(string videoId, string accessToken, string description)
        {
            var apiUrl = $"https://graph-video.facebook.com/v18.0/{videoId}";
            using (var client = new HttpClient())
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(accessToken), "access_token");
                formData.Add(new StringContent(description), "description");

                var response = await client.PostAsync(apiUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return new Tuple<int, string>((int)response.StatusCode, responseContent);
                }

                return new Tuple<int, string>((int)response.StatusCode, $"Failed to update video description: {response.ReasonPhrase}");
            }
               
        }

        //public async Task<Tuple<int, string>> InitializeUploadSessionAsync(FacebookPostDto facebookPostDto, MediaDto media)
        //{
        //    var data = media.Media_url.Split(',')[1];
        //    byte[] videoBytes = Convert.FromBase64String(data);

        //    // Calculate file size in bytes
        //    try
        //    {
        //        long fileSizeInBytes = videoBytes.Length;
           
        //    using (var client = new HttpClient())
        //    {
        //        var initializeUrl = $"https://graph-video.facebook.com/v18.0/{facebookPostDto.FacebookChannel.SocialChannelNetwokId}/videos";
        //        var content = new MultipartFormDataContent();
        //        content.Add(new StringContent("start"), "upload_phase");
        //        content.Add(new StringContent(facebookPostDto.FacebookChannel.ChannelAccessToken), "access_token");
        //        content.Add(new StringContent(fileSizeInBytes.ToString()), "file_size");

        //        var response = await client.PostAsync(initializeUrl, content);
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        var responseJson = JsonConvert.DeserializeObject<dynamic>(responseContent);

        //        return new Tuple<int, string>((int)response.StatusCode, responseContent);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async Task UploadVideoChunkAsync(string accessToken, string uploadSessionId, string startOffsetstring, string endOffsetstring, MediaDto mediaData)
        //{
        //    var startOffset = long.Parse(startOffsetstring);
        //    var endOffset = long.Parse(endOffsetstring);
        //    var videoChunk = GenerateVideoFileChunk(mediaData, startOffset, endOffset);
        //    using (var client = new HttpClient())
        //    {
        //        var uploadUrl = $"https://graph-video.facebook.com/v18.0/{uploadSessionId}";
        //        var content = new MultipartFormDataContent();
        //        content.Add(new StringContent("transfer"), "upload_phase");
        //        content.Add(new StringContent(accessToken), "access_token");
        //        content.Add(new StringContent(uploadSessionId), "upload_session_id");
        //        content.Add(new StringContent(startOffset.ToString()), "start_offset");
        //        content.Add(new ByteArrayContent(Convert.FromBase64String(videoChunk)), "video_file_chunk");
        //        content.Add(new StringContent("123"), "target");
        //        var response = await client.PostAsync(uploadUrl, content);
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //    }
        //}

        //public async Task FinishVideoUploadAsync(string accessToken, string uploadSessionId)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var finishUrl = $"https://graph-video.facebook.com/v18.0/{uploadSessionId}";
        //        var content = new MultipartFormDataContent();
        //        content.Add(new StringContent("finish"), "upload_phase");
        //        content.Add(new StringContent(accessToken), "access_token");
        //        content.Add(new StringContent(uploadSessionId), "upload_session_id");

        //        var response = await client.PostAsync(finishUrl, content);
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //    }
        //}

        //public async Task<Tuple<int, string>> StartVideoUpload(MediaDto mediaData, FacebookChannelDto facebookChannelDto)
        //{
        //    var FacebookApiUrl = $"https://graph-video.facebook.com/v18.0/{facebookChannelDto.SocialChannelNetwokId}/videos";

        //    using (var client = new HttpClient())
        //    using (var content = new MultipartFormDataContent())
        //    {
        //        // Convert base64 string to byte array
        //        try
        //        {
        //            var data = mediaData.Media_url.Split(',')[1];
        //            byte[] videoBytes = Convert.FromBase64String(data);



        //        using (var fileStream = new MemoryStream(videoBytes))
        //        {
        //            content.Add(new StringContent("start"), "upload_phase");
        //            content.Add(new StringContent(facebookChannelDto.ChannelAccessToken), "access_token");
        //            content.Add(new StringContent(videoBytes.Length.ToString()), "file_size");
        //            content.Add(new StreamContent(fileStream), "video_file", "video.mp4");

        //            var response = await client.PostAsync(FacebookApiUrl, content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseContent = await response.Content.ReadAsStringAsync();
        //                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseContent);
        //                    var startOffset = long.Parse(responseJson.start_offset.ToString());
        //                    var endOffset = long.Parse(responseJson.end_offset.ToString());
        //                //    await TransferVideoAsync(mediaData, facebookChannelDto, responseJson.upload_session_id.ToString(), startOffset, endOffset);
        //                    return new Tuple<int, string>((int)response.StatusCode, responseContent);
        //            }


        //        return new Tuple<int, string>((int)response.StatusCode, $"Failed to upload video: {response.ReasonPhrase}");
        //        }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //            return new Tuple<int, string>(1, $"Failed to upload video: ");
        //        }
        //    }
        //}


        public async Task TransferVideoAsync(MediaDto mediaData, FacebookChannelDto facebookChannelDto, string uploadSessionId, long startOffset, long endOffset)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                var transferUrl = $"https://graph-video.facebook.com/v18.0/{facebookChannelDto.SocialChannelNetwokId}/videos";
                content.Add(new StringContent("transfer"), "upload_phase");
                content.Add(new StringContent(facebookChannelDto.ChannelAccessToken), "access_token");
                content.Add(new StringContent(uploadSessionId), "upload_session_id");
                content.Add(new StringContent(startOffset.ToString()), "start_offset");

                // For video_file_chunk, you need to send the actual video chunk
                var videoChunk = GenerateVideoFileChunk(mediaData, startOffset, endOffset);
                content.Add(new StringContent(videoChunk), "video_file_chunk");
                HttpResponseMessage httpResponse = await asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    var response = await client.PostAsync(transferUrl, content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    response.EnsureSuccessStatusCode();
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
                
                
                await FinishVideoUploadAsync(facebookChannelDto, uploadSessionId);
            }
        }

        public string GenerateVideoFileChunk(MediaDto mediaData, long startOffset, long endOffset)
        {
            // Assuming MediaDto.Media_url is the base64-encoded video string
            var data = mediaData.Media_url.Split(',')[1];
            byte[] videoBytes = Convert.FromBase64String(data);

            var chunkSize = (int)(endOffset - startOffset);
            var chunk = new byte[chunkSize];
            Array.Copy(videoBytes, startOffset, chunk, 0, chunkSize);

            // Convert chunk to base64 for API request
            return Convert.ToBase64String(chunk);
        }
        public async Task FinishVideoUploadAsync(FacebookChannelDto facebookChannelDto, string uploadSessionId)
        {
            using (var client = new HttpClient())
            {
                var finishUrl = $"/v18.0/{facebookChannelDto.SocialChannelNetwokId}/videos" +
                    $"?upload_phase=finish" +
                    $"&access_token={facebookChannelDto.ChannelAccessToken}" +
                    $"&upload_session_id={uploadSessionId}";

                var response = await client.PostAsync(finishUrl, null);
                response.EnsureSuccessStatusCode();
            }
        }
        /// <summary>
        /// Update the uploaded picture (photo) with the given text
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="postID">Post ID</param>
        /// <param name="postText">Text to add tp the post</param>
        public async Task<Tuple<int, string>> UpdatePhotoWithPost(FacebookChannelDto facebookChannelDto, string postID, string postText)
        {

            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", facebookChannelDto.ChannelAccessToken },
                { "message", postText }//,
                // { "formatting", "MARKDOWN" } // doesn't work
            };

                var httpResponse = await http.PostAsync(
                    $"{facebookChannelDto.ChannelAPI}{postID}",
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }


    }
        }



