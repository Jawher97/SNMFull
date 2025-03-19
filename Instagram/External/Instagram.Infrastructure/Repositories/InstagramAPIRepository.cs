using Newtonsoft.Json.Linq;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Entities;
using System.Linq.Expressions;


namespace SNM.Instagram.Infrastructure.Repositories
{
    public class InstagramAPIRepository :IInstagramAPIRepository<Guid>
    {
        readonly string _accessToken;
        readonly string _pageID;
        // readonly string _instagramAPI = "https://graph.instagram.com/";
        //readonly string _pageEdgeFeed = "feed";
        //readonly string _pageEdgePhotos = "photos";
        readonly string _postToPageURL;
        readonly string _postToPagePhotosURL;

        public InstagramAPIRepository()
        {

        }
        
        //public async Task<Tuple<int, string>> PublishPost(InstagramChannelPostDto instagramChannelPostDto)
        //{

        //    using (var http = new HttpClient())
        //    {

        //        var postData = new Dictionary<string, string> {
        //           { "access_token", instagramChannelPostDto.InstagramChannelDto.ChannelAccessToken },
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