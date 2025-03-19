using SNM.LinkedIn.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNM.LinkedIn.Application.DTO;
using SNM.LinkedIn.Domain.Entities;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.Design;
using System;
using SNM.LinkedIn.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components.Web;
using SNM.LinkedIn.Domain.Enumeration;
using static System.Net.Mime.MediaTypeNames;
using Polly.Retry;
using Polly;
using Serilog;
using SNM.LinkedIn.Application.Exceptions.Model;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Polly.Caching;
using MediatR;

namespace SNM.LinkedIn.Infrastructure.Repositories
{
    public class LinkedInAPIRepository : ILinkedInAPIRepository<Guid>
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _versionNumber;

        private const string LinkedInUserUrl = "https://api.linkedin.com/v2/me";

        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _dbcontext;
        private readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy;
        public LinkedInAPIRepository(IConfiguration config, ApplicationDbContext dbContext)
        {
            _config = config;
            _clientId = _config.GetValue<string>("LinkedIn:ClientId");
            _clientSecret = _config.GetValue<string>("LinkedIn:ClientSecret");
            _redirectUri = _config.GetValue<string>("LinkedIn:RedirectUri");
            _versionNumber = _config.GetValue<string>("LinkedIn:versionNumber");
            _dbcontext = dbContext;
            asyncRetryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                  .WaitAndRetryAsync(retryCount: 3, count => TimeSpan.FromMilliseconds(60), onRetry: (exception, count, context) =>
                  {
                      Log.Information("___Retrying____ => {@result}", count);
                      Log.Information("_______DateTime _____=> {@result}", DateTime.Now);

                  });

        }

        public LinkedInAPIRepository() { }

        public async Task<Response<ChannelProfile>> LinkedINAuth(string code)
        {
            Response<ChannelProfile> result = new Response<ChannelProfile>();
           
            var parameters = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "redirect_uri", _redirectUri }
                };
            var formData = new FormUrlEncodedContent(parameters);

            // Set the Content-Type header
            formData.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Create HttpClient instance
            var client = new HttpClient();

            // Send the POST request and retrieve the response
            var response = await client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", formData);

            client.DefaultRequestHeaders.Add("Accept", "application/x-www-form-urlencoded");

            // Read the response content as string
            var responseContent = await response.Content.ReadAsStringAsync();

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                var objects = JsonConvert.DeserializeObject<LinkedInAccountTokens>(responseContent);
                var accesstoken = objects.access_token;
                var refreshtoken = objects.refresh_token;
                var expires_in = objects.expires_in;
                var refresh_token_expires_in = objects.refresh_token_expires_in;
                var scopes = objects.scope;

                ChannelProfile usertokens = new ChannelProfile
                {
                    AccessToken = accesstoken,
                    RefreshToken = refreshtoken,
                    expires_in = expires_in,
                    RefreshTokenExpiresIn = refresh_token_expires_in,
                    Scope = scopes
                };
                result.Succeeded = true;
                result.Data = usertokens;
                return result;
            }
            else
            {
                result.Succeeded = false;
                return result;
               // throw new Exception("HTTP request failed with status code " + response.StatusCode);
            }

        }


        public async Task<string> refreshAccessToken(string refreshToken)
        {
            
            string authUrl = "https://www.linkedin.com/oauth/v2/accessToken";
            string sign = $"grant_type=refresh_token&refresh_token={refreshToken}" +
                $"&client_id={_clientId}&client_secret={_clientSecret}";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest webRequest = WebRequest.Create(authUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.Host = "www.linkedin.com";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            byte[] signBytes = Encoding.ASCII.GetBytes(sign);
            webRequest.ContentLength = signBytes.Length;
            Stream dataStream = webRequest.GetRequestStream();
            dataStream.Write(signBytes, 0, signBytes.Length);
            dataStream.Close();

            WebResponse response = webRequest.GetResponse();
            dataStream = response.GetResponseStream();

            StreamReader responseReader = new StreamReader(dataStream);
            string returnVal = responseReader.ReadToEnd();
            responseReader.Close();
            dataStream.Close();
            response.Close();

            var objects = JsonConvert.DeserializeObject<LinkedInAccountTokens>(returnVal);
            var TokenGlobe = objects.access_token;

            return TokenGlobe;
        }

        public async Task<Response<ChannelProfile>> GetProfileID(string accessToken)
        {
            Response<ChannelProfile> result = new Response<ChannelProfile>();
            var requestUrl = LinkedInUserUrl+"?projection=(id,profilePicture(displayImage~:playableStreams))";
        

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(requestUrl);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                   
                   
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);
                    string id = jsonObject["id"];
                    try
                    {
                        string identifier = jsonObject["profilePicture"]["displayImage~"]["elements"][0]["identifiers"][0]["identifier"];
                        var memberProfile = await GetMemberProfile(accessToken, id);

                        memberProfile.Data.CoverPhoto = identifier;
                        memberProfile.Data.AccessToken = accessToken;
                        memberProfile.Data.ProfileUserId = "urn:li:person:" + id;
                        result.Succeeded = true;
                        result.Data = memberProfile.Data;
                    }
                    
                    catch(Exception ex){

                    }
               
                    return result;
         

                    
                }
                else
                {
                    result.Succeeded = false;
                    result.Message = content;
                    return result;
                }
            }
        }


        public async Task<Response<ChannelProfile>> GetMemberProfile(string accesstoken, string person_id)
        {
            Response<ChannelProfile> result=new Response<ChannelProfile>();
            var requesturl = $"https://api.linkedin.com/v2/people/(id:{person_id})";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                var response = await client.GetAsync(requesturl);

                var content = await response.Content.ReadAsStringAsync();

                if(response.IsSuccessStatusCode)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);
                    string lastname = jsonObject["localizedLastName"];
                    string firstname = jsonObject["localizedFirstName"];
                    //string profilePicture = jsonObject["profilePicture"];
                    string vanityName = jsonObject["vanityName"];
                    string headline = jsonObject["localizedHeadline"];

                    ChannelProfile ChannelProfile = new ChannelProfile
                    {
                        ProfileUserId = "urn:li:person:" + person_id,
                       
                        UserName = firstname + " " + lastname,
                        Headline = headline,
                        ProfileLink = "https://www.linkedin.com/in/" + vanityName,


                    };
                    //ChannelProfile profile = new ChannelProfile
                    //{
                    //    Id = person_id,
                    //    FullName = firstname + " " + lastname,
                    //    LinkedinProfileLink = "www.linkedin.com/in/" + vanityName,
                    //    Headline = headline,
                    //    //ProfilePicture = profilePicture

                    //};
                    result.Succeeded = true;
                    result.Data = ChannelProfile;
                    return result;
                }
                    
                else {
                    
                       result.Message="HTTP request failed with status code " + response.StatusCode; }
                       result.Succeeded=false;
                       return result;

            }
        }
    

    public async Task<Response<ChannelProfile>> GetLinkedInProfilePicture(string accessToken, string userUrn)
    {
            Response<ChannelProfile> memberresult = new Response<ChannelProfile>();
            memberresult.Data = new ChannelProfile();
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                var apiEndpoint = $"https://api.linkedin.com/v2/people/(id:{userUrn})?projection=(id,profilePicture(displayImage~:playableStreams))";

            var response = await httpClient.GetAsync(apiEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);
                    var result = await GetMemberProfile(accessToken, userUrn);
                    string identifier = jsonObject["profilePicture"]?["displayImage~"]?["elements"][0]?["identifiers"][0]?["identifier"] ?? "";
                    result.Data.CoverPhoto = identifier;
                    memberresult.Data = result.Data;
                    memberresult.Succeeded = true;
             
                return memberresult;
                }
                else
                {
                    memberresult.Succeeded = false;

                    return memberresult;
                }
            }
    }

   

        public async Task<MemberProfile> GetCommentersProfile(string accesstoken, string entityUrn)
        {
            var requesturl = $"https://api.linkedin.com/rest/socialActions/{entityUrn}/comments?projection=(elements(*(*,actor~(*,profilePicture(displayImage~:playableStreams)))))";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var response = await client.GetAsync(requesturl);

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);
                    string lastname = jsonObject["elements"][0]["localizedLastName"];
                    string firstname = jsonObject["localizedFirstName"];
                    string vanityName = jsonObject["elements"][0]["actor"]["vanityName"];
                    string headline = jsonObject["localizedHeadline"];

                    MemberProfile profile = new MemberProfile
                    {
                        
                        FullName = firstname + " " + lastname,
                        LinkedinProfileLink = "www.linkedin.com/in/" + vanityName,
                        Headline = headline
                    };

                    return profile;
                }

                else { throw new Exception("HTTP request failed with status code " + response.StatusCode); }


            }
        }


        public async Task<Response<Channel>> GetCompanyProfile(string accesstoken, string org_id)
        {
            Response<Channel> channel = new Response<Channel>();
            channel.Data = new Channel();
            string Link= "";        
            var requesturl = $"https://api.linkedin.com/rest/organizations/{org_id}";                     
            var response= await GetPhotoCompanyProfile(accesstoken, org_id);
            if (response.Succeeded)
              {
                var httpclient = new HttpClient();
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                httpclient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                httpclient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                var httpresponse = await httpclient.GetAsync(requesturl);
                var httpcontent = await httpresponse.Content.ReadAsStringAsync();
                if (httpresponse.IsSuccessStatusCode)
                {
                    dynamic organization = JsonConvert.DeserializeObject<dynamic>(httpcontent);
                    string fullname = organization["localizedName"];  
                    channel.Data.DisplayName = fullname;
                    channel.Data.Photo = response.Data;
                    channel.Data.Link = Link;
                    channel.Data.SocialChannelId= organization["$URN"];
                    channel.Succeeded = true;
                    return channel;
                }
                channel.Succeeded = false;
                channel.Message= "HTTP request failed with status code "+ httpresponse.StatusCode;
                return channel;

              }

                else {
                channel.Message = response.Message;
                channel.Succeeded= false;
                return channel;
                    
                 }


            
        }
        public async Task<Response<string>> GetPhotoCompanyProfile(string accesstoken, string org_id)
        {
            Response<string> result = new Response<string>();
            var getImageUrl = $"https://api.linkedin.com/v2/organizations/{org_id}?projection=(logoV2(original~:playableStreams))";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
            var response = await client.GetAsync(getImageUrl);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JObject jsonObject = JObject.Parse(content);
                JArray identifiers = jsonObject["logoV2"]["original~"]["elements"][0]["identifiers"] as JArray;



                foreach (var identifier in identifiers)
                {
                    if (identifier["identifierType"]?.ToString() == "EXTERNAL_URL")
                    {
                        result.Data = identifier["identifier"]?.ToString();
                       break; // Exit the loop if an external URL is found
                    }
                }

                result.Succeeded = true;
                return result;
            }
            result.Message = "HTTP request failed with status code " + response.StatusCode;

            return result;
        }
        public static string ConvertUrnToId(string urn)
        {
            // Split the URN by ":"
            string[] parts = urn.Split(':');

            // The last part is the ID
            string id = parts[parts.Length - 1];

            return id;
        }
        public async Task<Response<Dictionary<string, object>>> GetOrgDetails(string accessToken,Guid BrandId)
        {
            Response<Dictionary<string, object>> result=new Response<Dictionary<string, object>> ();
            var requestUrl = "https://api.linkedin.com/v2/organizationalEntityAcls?q=roleAssignee&role=ADMINISTRATOR&state=APPROVED&projection=(elements*(organizationalTarget~(id,localizedName,localizedWebsite,logoV2(original~:playableStreams))))";

            List<Channel> companyPagesDetails = new List<Channel>();

            string Link = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                          
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(content);

                    List<string> photoUrls = new List<string>();

                

                     foreach (var element in jsonObject["elements"])
                            {
                   

                            string companyName = element["organizationalTarget~"]["localizedName"];
                            string companyId = element["organizationalTarget~"]["id"];
                            string photoUrl = element?["organizationalTarget~"]?["logoV2"]?["original~"]?["elements"]?[0]?["identifiers"]?[0]?["identifier"]?.ToString() ?? "";
                            string organizationType = element["organizationalTarget"];

                                if (organizationType.Contains("organizationBrand"))
                                {

                                    Link = $"https://www.linkedin.com/showcase/{companyId}/admin/feed/posts/?feedType=following";
                                }
                                else if (organizationType.Contains("organization"))
                                {
                                     Link = $"https://www.linkedin.com/company/{companyId}/admin/feed/posts/";
                       
                                }

                                Channel pagedetails = new Channel
                                    {
                                        DisplayName = companyName,
                                        Photo = photoUrl,
                                        BrandId = BrandId,                          
                                        Link= Link,
                                        SocialChannelId = companyId
                                        };

                        
                                companyPagesDetails.Add(pagedetails);
                    }


                    var resultData = new Dictionary<string, object>
                                    {
                                    { "Channel", companyPagesDetails },

                                };
                    result.Data = resultData;
                    result.Succeeded = true;



                    // return companyPagesDetails;
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Message = "HTTP request failed with status code " + response.StatusCode;


                    }
                    return result;
                //linkedInChannels.Add(linkedInChannel);

                //var profile = await GetCompanyProfile(accessToken, companyId);
                //pagedetails.CompanyProfileUrl = "https://" + profile.LinkedinProfileLink;

                //var insight = await GetPageStatistics(pagedetails.CompanyUrn, accessToken);
                //pagedetails.insight.totalLikes = insight.totalLikes;
                //pagedetails.insight.totalComments = insight.totalComments;
                //pagedetails.insight.clickCount = insight.clickCount;
                //pagedetails.insight.impressionCount = insight.impressionCount;
                //pagedetails.insight.engagement = insight.engagement;

                //companyPagesDetails.Add(pagedetails);

                // await _dbcontext.LinkedInChannel.AddAsync(pagedetails);


            }
        }

        public async Task<LinkedInInsight> GetPageStatistics(string orgUrn, string accesstoken)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

            var apiUrl = $"https://api.linkedin.com/rest/organizationalEntityShareStatistics?q=organizationalEntity&organizationalEntity={orgUrn}";

            var rez = Task.Run(async () =>
            {

                var httpResponse = await httpClient.GetAsync(apiUrl);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;

            });
            var rezJson = JObject.Parse(rez.Result).ToString();

            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(rezJson);

            int commentscounts = jsonObject["elements"][0]["totalShareStatistics"]["commentCount"];
            int likescounts = jsonObject["elements"][0]["totalShareStatistics"]["likeCount"];
            double engagement = jsonObject["elements"][0]["totalShareStatistics"]["engagement"];
            int impressionCount = jsonObject["elements"][0]["totalShareStatistics"]["impressionCount"];
            int clickCount = jsonObject["elements"][0]["totalShareStatistics"]["clickCount"];

            LinkedInInsight insight = new LinkedInInsight
            {
                totalComments = commentscounts,
                totalLikes = likescounts,
                engagement = engagement,
                clickCount = clickCount,
                impressionCount = impressionCount
            };

            return insight;


        }


        public async Task<Response<string>> PublishToLinkedIn(LinkedInPostDto post, LinkedInChannelDto linkedInChannelDto)
        {
            Response<string> resp =new Response<string>();
            if (post == null || linkedInChannelDto == null)
            {
                // Add appropriate error handling for null input parameters
                return null;
            }

            List<string> mediaUrnList = new List<string>();
            var messagePost = new Dictionary<string, object>
                {
                    { "author", linkedInChannelDto.Author_urn },
                    { "commentary", post.Message },
                    { "visibility", "PUBLIC" },
                    { "distribution", new Dictionary<string, object>
                        {
                            { "feedDistribution", "MAIN_FEED" }
                        }
                    },
                    { "isReshareDisabledByAuthor", false },
                    { "lifecycleState", "PUBLISHED" }
                };

            if (post.Message !="" && post.PostDto.MediaData == null)
            {
                // Handle the case where there is no message
                return await PostToLinkedIn(post, linkedInChannelDto);
            }

            if (post.PostDto.MediaData != null && post.PostDto.MediaData.Any() && post.PostDto.MediaData.FirstOrDefault()?.Media_type == MediaTypeEnum.IMAGE)
            {
               
                    foreach (var item in post.PostDto.MediaData)
                    {
                        var mediaUrn = await LinkedinPostWithImage(post, linkedInChannelDto, item.Media_url);
                        mediaUrnList.Add(mediaUrn);
                    }
                if (post.PostDto.MediaData.Count() > 1)
                {
                    var content = new
                    {
                        multiImage = new
                        {
                            images = mediaUrnList.Select(imageId => new
                            {
                                id = imageId,
                                altText = "Image Alt Text"
                            }).ToList()
                        }
                    };

                    messagePost["content"] = content;
                }
                else if (post.PostDto.MediaData.Count() == 1)
                {
                    if (post.PostDto.MediaData.FirstOrDefault().Media_type == MediaTypeEnum.IMAGE)
                    {
                        var content = new
                        {
                            media = new
                            {
                                title = "title of the image",
                                id = mediaUrnList[0]
                            }
                        };
                        messagePost["content"] = content;
                    }
                }
            }

            // Handle the case of video media
            if (post.PostDto.MediaData != null && post.PostDto.MediaData.FirstOrDefault()?.Media_type == MediaTypeEnum.VIDEO)
            {
                return await LinkedinPostWithVideo(linkedInChannelDto, post);
            }

            var requestJson = JsonConvert.SerializeObject(messagePost);
            return await Finalizecreation(linkedInChannelDto.AccessToken, requestJson);
        }

        public async Task<Response<string>> PostToLinkedIn(LinkedInPostDto post, LinkedInChannelDto linkedInChannelDto)
        {

         
            var message = new
            {
                author = linkedInChannelDto.Author_urn,
                commentary = post.Message,
                visibility = "PUBLIC",
                distribution = new
                {
                    feedDistribution = "MAIN_FEED"
                },
                lifecycleState = "PUBLISHED",
                isReshareDisabledByAuthor = false
            };
            var requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(message);

            var response = await Finalizecreation(linkedInChannelDto.AccessToken, requestJson);

            

            return response;
        }

        
        public async Task<string> LinkedinPostWithImage(LinkedInPostDto post, LinkedInChannelDto linkedInChannelDto, string photo)
        {

                var MediaUrn = string.Empty;
                string upload_url = string.Empty;
                

                var rez = Task.Run(async () =>
                {//1step for image
                    var responseImage = await InitializeUpload(linkedInChannelDto.Author_urn, linkedInChannelDto.AccessToken);
                    return responseImage;
                });

                var rezJson = JObject.Parse(rez.Result.Item2);
            
                if (rez.Result.Item1 == 200)
                {
                        try // return error from JSON
                        {
                                MediaUrn = rezJson["value"]["image"].Value<string>();
                                upload_url = rezJson["value"]["uploadUrl"].Value<string>();


                                //upload the image
                                var client = new HttpClient();
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", linkedInChannelDto.AccessToken);
                                var data = photo.Split(',')[1];
                                byte[] imgdata = Convert.FromBase64String(data);
                                var content = new MultipartFormDataContent();
                                var fileContent = new ByteArrayContent(imgdata);
                                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                                content.Add(fileContent, "file", "image.jpg");
                                HttpResponseMessage putResponse = await client.PutAsync(upload_url, content);
                                putResponse.EnsureSuccessStatusCode();

                               
                }
                        catch (Exception ex) // return unknown error
                        {
                            // log exception somewhere
                            return null;
                        }
                }
                 
            return MediaUrn;
        }





        //1step for image
        public async Task<Tuple<int, string>> InitializeUpload(string author_urn, string accessToken)
        {
            //initialize upload
            var requestBody = new
            {
                initializeUploadRequest = new
                {
                    owner = author_urn
                }
            };

            var requestJsonImage = JsonConvert.SerializeObject(requestBody);

            var requestUrl = "https://api.linkedin.com/rest/images?action=initializeUpload";

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var requestContent = new StringContent(requestJsonImage, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(requestUrl, requestContent);
                var responseImageContent = await response.Content.ReadAsStringAsync();

                /*var uploadurl = "https://www.linkedin.com/dms-uploads/D4E10AQGIAonObLeXiA/uploaded-image/0?ca=vector_ads&cn=uploads&sync=0&v=beta&ut=1MqUH3N6tWWaI1";
                var asset_id = "urn:li:image:D4E10AQGIAonObLeXiA";*/
                return new Tuple<int, string>(
                (int)response.StatusCode,
                responseImageContent
                );
            }


        }




        public async Task<Tuple<int, string>> InitializeVideoUpload(LinkedInChannelDto linkedInChannelDto, LinkedInPostDto post,long totalbytes)
        {

            var requestBody = new
            {
                initializeUploadRequest = new
                {
                    owner = linkedInChannelDto.Author_urn,
                    fileSizeBytes = totalbytes,
                    uploadCaptions = false,
                    uploadThumbnail = false
                }
            };

            var requestJson = JsonConvert.SerializeObject(requestBody);

            var requestUrl = "https://api.linkedin.com/rest/videos?action=initializeUpload";

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", linkedInChannelDto.AccessToken);
                client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, requestContent);

                var responseJson = await response.Content.ReadAsStringAsync();

                    return new Tuple<int, string>(
                    (int)response.StatusCode,
                    responseJson
                    );
            }
        }
        //public async Task<string> initializeVideoRegistre(LinkedInChannelDto linkedInChannelDto, string author_urn, LinkedInPostDto post, long totalbytes)
        //{

        //    var requestPayload = new
        //    {
        //        registerUploadRequest = new
        //        {
        //            owner = linkedInChannelDto.Author_urn,
        //            recipes = new[]
        //      {
        //            "urn:li:digitalmediaRecipe:feedshare-video"
        //        },
        //            serviceRelationships = new[]
        //      {
        //            new
        //            {
        //                identifier = "urn:li:userGeneratedContent",
        //                relationshipType = "OWNER"
        //            }
        //        }
        //        }
        //    };

        //    // Serialize the request payload to JSON
        //    string jsonPayload = JsonConvert.SerializeObject(requestPayload);

        //    // Create an HttpClient instance
        //    using var httpClient = new HttpClient();

        //    // Set the request headers
        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {linkedInChannelDto.AccessToken}");
        //    httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    // Create a StringContent with the JSON payload
        //    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //    // Send the POST request
        //    var response = await httpClient.PostAsync("https://api.linkedin.com/v2/assets?action=registerUpload", content);

        //    // Check the response status code and handle the response as needed
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Request was successful
        //        var responseBody = await response.Content.ReadAsStringAsync();
              
        //        return new Tuple<int, string>(
        //         (int)response.StatusCode,
        //         responseBody
        //         );
        //    }
        //    else
        //    {
        //        // Request failed, handle the error
        //        throw new Exception($"Error: {response.StatusCode}");
        //    }
         
        //}



        public async Task<Response<string>> LinkedinPostWithVideo(LinkedInChannelDto linkedInChannelDto,  LinkedInPostDto post)
        {
            Response<string> resp =new Response<string>();
            //initialize upload
            var MediaUrn = string.Empty;
            string upload_url = string.Empty;
            var data = post.PostDto.MediaData.FirstOrDefault().Media_url.Split(',')[1];

            byte[] imgdata = Convert.FromBase64String(data);
          //  byte[] imgbytes = File.ReadAllBytes(post.PostDto.MediaData.FirstOrDefault().Media_url);
            long totalbytes = imgdata.Length;

            var rez = Task.Run(async () =>
            {
             //   var responseImage = await initializeVideoRegistre(accessToken, author_urn, post, totalbytes);
                var responseImage = await InitializeVideoUpload(linkedInChannelDto, post, totalbytes);
                return responseImage;
            });

            var rezJson = JObject.Parse(rez.Result.Item2);
            MediaUrn = rezJson["value"]["video"].Value<string>();
            upload_url = rezJson["value"]["uploadInstructions"][0]["uploadUrl"].Value<string>();
           // upload_url = rezJson["value"]["uploadMechanism"]["com.linkedin.digitalmedia.uploading.MediaUploadHttpRequest"]["uploadUrl"]?.Value<string>();
          
            var etagVideo = "";
            //upload video
            using (var httpClient = new HttpClient())
            {
               
                    var fileContent = new ByteArrayContent(imgdata);
                    fileContent.Headers.Add("Content-Type", "application/octet-stream");
                    

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(fileContent, "file", "video.mp4"); 
                    // Send the POST request to the specified URL

                    HttpResponseMessage responseuPload = await httpClient.PutAsync(upload_url, fileContent);

                        if (responseuPload.IsSuccessStatusCode)
                        {

                            if (responseuPload.Headers.TryGetValues("ETag", out var etagValues))
                            {
                                var etag = etagValues.FirstOrDefault();
                                etagVideo = etag;
                            }

                            else
                            {
                               resp.Data="ETag not found in response headers.";
                               resp.Succeeded = false;
                            }
                        }


                }
            

            Dictionary<string, object> jsonDictionary = new Dictionary<string, object>
                    {
                        { "finalizeUploadRequest", new Dictionary<string, object>
                            {
                                { "video", MediaUrn },
                                { "uploadToken", "" },
                                { "uploadedPartIds", new List<string>
                                    {
                                        etagVideo
                                    }
                                }
                            }
                        }
                    };

            var requestJson = JsonConvert.SerializeObject( jsonDictionary );

            var responseVideo = await FinalizecreationVideo(linkedInChannelDto, requestJson,post, MediaUrn);

            
            return responseVideo;


        }
       

        public async Task<Tuple<int, string>> UploadCaptionWithVideo( string accesstoken,LinkedInPostDto post,string upload_url)
        {
            // Prepare the JSON metadata for the request
            string metadataJson = @"{
            ""format"": ""SRT"",
            ""formattedForEasyReader"": true,
            ""largeText"": true,
            ""source"": ""USER_PROVIDED"",
            ""locale"": {
                ""variant"": ""AMERICAN"",
                ""country"": ""US"",
                ""language"": ""EN""
            },
            ""transcriptType"": ""CLOSED_CAPTION""
        }";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

                // Create a multipart form data content
                var content = new MultipartFormDataContent();

                // Add metadata as a string
                content.Add(new StringContent(metadataJson, Encoding.UTF8, "application/json"), "metadata");

                // Add the caption text directly to the form data
                content.Add(new StringContent(post.Message, Encoding.UTF8), "file", "sample.srt");



                var response = await httpClient.PostAsync(upload_url, content);
           

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return new Tuple<int, string>(
                     (int)response.StatusCode,
                     responseContent
                     );

                }
                else
                {
                    throw new Exception($"Failed to upload captions with status code: {response.StatusCode}");
                }
               
            }
        }
        public async Task<Response<string>> CreatePoll(string accessToken, string orgId, string commentary, string question)
        {

            var message = new
            {
                author = "urn:li:organization:" +orgId,
                commentary = commentary,
                visibility = "PUBLIC",
                distribution = new
                {
                    feedDistribution = "MAIN_FEED"
                },
                lifecycleState = "PUBLISHED",
                isReshareDisabledByAuthor = false,
                content = new
                {
                    poll = question,
                    options = new object[]
                        {
                            new { text = "Red" },
                            new { text = "Blue" },
                            new { text = "Yellow" },
                            new { text = "green" }
                        },
                    settings = new
                    { duration = "THREE_DAYS" }
                }
            };

            var requestJson = JsonConvert.SerializeObject(message);

            var response = await Finalizecreation(accessToken, requestJson);

            return response;
        }


        public async Task<Response<string>> CreateArticle(string accessToken, string author_id, LinkedInArticleDto article)
        {
            var message = new
            {
                author = "urn:li:" +article.author_type+ ":" +author_id,
                commentary = article.commentary,
                visibility = article.visibility,
                distribution = new
                {
                    feedDistribution = "MAIN_FEED"
                },
                content = new
                {
                    article = new
                    {
                        source = article.source, //REQUIRED
                        title = article.title,  //REQUIRED
                        description = article.description //OPTIONAL
                    }
                },
                lifecycleState = "PUBLISHED",
                isReshareDisabledByAuthor = article.isReshareDisabledByAuthor
            };

            var requestJson = JsonConvert.SerializeObject(message);

            var response = await Finalizecreation(accessToken, requestJson);

            return response;

        }


        private async Task<Response<string>> FinalizecreationVideo(LinkedInChannelDto linkedInChannelDto, string requestJson,LinkedInPostDto post,string MediaUrn)
        {
            Response<string> resp=new Response<string>();
            var requestUrl = "https://api.linkedin.com/rest/videos?action=finalizeUpload";

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", linkedInChannelDto.AccessToken);
                client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, requestContent);



                if (response.IsSuccessStatusCode)
                {

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var  message = new
                        {
                            author = linkedInChannelDto.Author_urn,
                            commentary = post.Message,
                            visibility = "PUBLIC",
                            distribution = new
                            {
                                feedDistribution = "MAIN_FEED"
                            },
                            content = new
                            {
                                media = new
                                {
                                    title = "title of the image",
                                    id = MediaUrn
                                }
                            },
                            lifecycleState = "PUBLISHED",
                            isReshareDisabledByAuthor = false
                        };

                   
                  var postContentJson = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                  var responseFinale = await Finalizecreation(linkedInChannelDto.AccessToken, postContentJson);


                    return responseFinale;
                }

                else
                {
                    var errorMessage = $"Request failed with status code {response.StatusCode}. ";
                    resp.Data= errorMessage;
                    resp.Succeeded = false;
                    return resp;
                }
            } 
        }
        
    
        private async Task<Response<string>> Finalizecreation(string accessToken, string requestJson)
        {
             Response<string> resp = new Response<string>();
            var requestUrl = "https://api.linkedin.com/rest/posts";

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, requestContent);
                var responseJson = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    
                    var urn = response.Headers.GetValues("x-restli-id").FirstOrDefault();
                    resp.Succeeded = true;
                    resp.Data = urn;
                    return resp;
                }
                else
                {
                    var errorMessage = $"Request failed with status code {response.StatusCode}. Response content: {responseJson}";
                    resp.Succeeded = false;
                    resp.Data = errorMessage;
                    return resp;
                    
                }
            }
        }
            public async Task<Response<string>> CreateReaction(PostDetalisDto post, string accessToken)
        {
            Response<string> result = new Response<string>();
            var requestUrl = $"https://api.linkedin.com/rest/reactions?actor={WebUtility.UrlDecode(post.FromId)}";

            var reaction = new
            {
                root = post.PostIdAPI,//unecoded share/post/comment
                reactionType = "EMPATHY"
            };

            var requestJson = JsonConvert.SerializeObject(reaction);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);


                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Post liked successfully!";
                    return result;
                }
                result.Message = "Failed to like post.";

                return result;

            }
        }
        public async Task<Response<string>> CreateReactionComment(CommentDetailsDto comment, string accessToken)
        {
            Response<string> result = new Response<string>();
            var requestUrl = $"https://api.linkedin.com/rest/reactions?actor={WebUtility.UrlDecode(comment.FromId)}";

            var reaction = new
            {
                root = WebUtility.UrlDecode(comment.CommentUrn),//unecoded share/post/comment
                reactionType = "LIKE"
            };

            var requestJson = JsonConvert.SerializeObject(reaction);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);


                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, requestContent);

                //if (response.IsSuccessStatusCode)
                //{
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Comment liked successfully!";
                    return result;
                //}
                result.Message = "Failed to like comment.";

                return result;

            }
        }

        public async Task<Response<string>> DeleteReaction(PostDetalisDto post, string accessToken)
        {
            Response<string> result = new Response<string>();
            var requestUrl = $"https://api.linkedin.com/rest/reactions/(actor:{WebUtility.UrlEncode(post.FromId)},entity:{WebUtility.UrlEncode(post.PostIdAPI)})";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var response = await httpClient.DeleteAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = "Post disliked successfully!";
                    return result;
                }
                result.Message = "Failed to dislike post.";
                return result;
            }



        }
        public async Task<Response<string>> DeleteReactionComment(CommentDetailsDto comment, string accessToken)
        {
            Response<string> result = new Response<string>();
          
            var requestUrl = $"https://api.linkedin.com/rest/reactions/(actor:{WebUtility.UrlEncode(comment.FromId)},entity:{WebUtility.UrlEncode(comment.CommentUrn)})";


            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var response = await httpClient.DeleteAsync(requestUrl);

                //if (response.IsSuccessStatusCode)
                //{
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                result.Message = "Comment liked successfully!";
                return result;
            //}
            //result.Message = "Failed to like comment.";
            //return result;
        }



        }
        public async Task<Response<string>> DeletePostAsync(PostDetalisDto post, string accessToken)
        {
            var url = $"https://api.linkedin.com/rest/posts/{WebUtility.UrlEncode(post.PostIdAPI)}";
            Response<string> result = new Response<string>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                httpClient.DefaultRequestHeaders.Add("X-RestLi-Method", "DELETE");

                
                var response = await httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Message = $"Post deleted successfully";
                    return result;
                }
                return result;
            }
        }

        public async Task<Response<string>> ResharePost(string accessToken, string author_urn, string shareId, string commentary)
        {

            var message = new
            {
                author = author_urn,
                commentary = commentary,
                visibility = "PUBLIC",
                distribution = new
                {
                    feedDistribution = "MAIN_FEED"
                },
                lifecycleState = "PUBLISHED",
                isReshareDisabledByAuthor = false,
                reshareContext = new
                {
                    parent = "urn:li:share:" +shareId
                }
            };

            var requestJson = JsonConvert.SerializeObject(message);

            var response = await Finalizecreation(accessToken, requestJson);

            return response;
        }

        
        public async Task<string> DisableCommentsOnCreatedPost(string accessToken, string shareUrn, string actorUrn)
        {
            var url = $"https://api.linkedin.com/rest/socialMetadata/{shareUrn}?actor={actorUrn}";

            JObject body = new JObject();
            JObject patch = new JObject();


            patch["$set"] = new JObject();
            patch["$set"]["commentsState"] = "CLOSED";

            body["patch"] = patch;

                

            var requestJson = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);


                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();

                    return responseJson;
                }
                else
                {
                    return "Error creating entity";
                }
            }
        }

        public async Task<string> EditPost(string accessToken, string commentary, string shareUrn)
        {
            var url = $"https://api.linkedin.com/rest/posts/{shareUrn}";
     

            JObject body = new JObject();
            JObject patch = new JObject();


            patch["$set"] = new JObject();
            patch["$set"]["commentary"] = commentary;

            body["patch"] = patch;

            var requestJson = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                client.DefaultRequestHeaders.Add("X-RestLi-Method", "PARTIAL_UPDATE");

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return responseJson;
                }
                else
                {
                    return "Ok!";
                }
            }
        }





        public async Task<Response<CommentDto>> CreateComment(CommentDetailsDto comment, string accessToken)
        {
            Response<CommentDto> result =new Response<CommentDto>();
            var url = $"https://api.linkedin.com/v2/socialActions/{WebUtility.UrlEncode(comment.PostId)}/comments";

            var body = new Dictionary<string, object>
                {
                    { "actor", comment.FromId },
                    { "message", new Dictionary<string, object>
                        {
                            { "text", comment.Message }
                        }
                    }
                    
                };
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
               
                if (comment.PhotoUrl != "")
                {
                    var mediaUrn = await LinkedinCommentWithImage(comment, accessToken);
                    var content = new[]
                    {
                        new
                        {entity = new
                        {
                             image = mediaUrn,
                              //imageUrl = await GetImageUrl(mediaUrn,accessToken)
                        }
                            //type = "IMAGE"
                        }
                        
                    };
                    body["content"] =  content ;
                }


                var requestJson = JsonConvert.SerializeObject(body);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, requestContent);
                var httpContent = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(httpContent);

                if (response.IsSuccessStatusCode)
                {
                    CommentDto commentDto = new CommentDto()
                    {
                        CommentId = json["id"].ToString(),
                        CommentUrn= json["$URN"].ToString(),
                        Message = comment.Message,
                        FromId = comment.FromId,
                        Replies = new List<CommentDto>(),
                        Reactions = new List<ReactionsDto>(),
                        CreatedTime=DateTime.Now,
                        LikesCount=0

                    };
                    result.Data = commentDto;
                    result.Succeeded=true;
                   // result.Data = json;
                    return result;

                }
                result.Message = "Failed To create Comment with status" + response.StatusCode;
               return  result;
            }
          
            
        }
        public async Task<string> GetImageUrl(string mediaUrn, string accessToken)
        {
            var url = $"https://api.linkedin.com/rest/images/{mediaUrn}";

            var client = new HttpClient();

            using (var http = client)
            {


                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                http.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);


                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var resJson = JObject.Parse(content);
                var mediaUrl = resJson["downloadUrl"].ToString();

                return mediaUrl;
            }
        }
        public async Task<string> CommentWithImage(CommentDetailsDto comment, string accessToken)
        {
            //var registerUploadRequest = new Dictionary<string, object>
            //                {
            //                    { "owner", comment.FromId },
            //                    { "recipes", new[] { "urn:li:digitalmediaRecipe:feedshare-image" } },
            //                    { "serviceRelationships", new[]
            //                        {
            //                            new Dictionary<string, object>
            //                            {
            //                                { "identifier", "urn:li:userGeneratedContent" },
            //                                { "relationshipType", "OWNER" }
            //                            }
            //                        }
            //                    },
            //                    { "supportedUploadMechanism", new[] { "SYNCHRONOUS_UPLOAD" } }
            //                };

            using (var httpClient = new HttpClient())
            {
                // Set headers
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
           

                // Construct API URL
                string apiUrl = "https://api.linkedin.com/v2/assets?action=registerUpload";

                // Prepare data for registering the upload
                var uploadData = new
                {
                    registerUploadRequest = new
                    {
                        recipes = new[]
                        {
                "urn:li:digitalmediaRecipe:feedshare-image"
            },
                        owner = comment.FromId,
                        serviceRelationships = new[]
                        {
                new
                {
                    relationshipType = "OWNER",
                    identifier = "urn:li:userGeneratedContent"
                }
            }
                    }
                };

                // Serialize data to JSON
                var jsonUploadData = JsonConvert.SerializeObject(uploadData);

                // Send POST request
                var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonUploadData, Encoding.UTF8, "application/json"));


                //if (response.IsSuccessStatusCode)
                //{
                var responseContent = await response.Content.ReadAsStringAsync();
                    // Parse the response to obtain the mediaArtifact and upload URL.
                    // You can extract "mediaArtifact" and "uploadUrl" from the responseContent.
                    using (var imageClient = new HttpClient())
                    {
                        var jsonResponse = JObject.Parse(responseContent);
                        var mediaArtifact = jsonResponse["value"]["mediaArtifact"].ToString();
                        var uploadUrl = jsonResponse["value"]["uploadMechanism"]["com.linkedin.digitalmedia.uploading.MediaUploadHttpRequest"]["uploadUrl"].ToString();

                        

                        imageClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                        var data = comment.PhotoUrl.Split(',')[1];
                        byte[] imgdata = Convert.FromBase64String(data);
                        //var content = new MultipartFormDataContent();
                        //var fileContent = new ByteArrayContent(imgdata);
                        //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        //content.Add(fileContent, "file", "image.jpg");
                        using (var imageContent = new ByteArrayContent(imgdata))
                        {
                            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                            var imageResponse = await imageClient.PostAsync(uploadUrl, imageContent);

                            if (imageResponse.IsSuccessStatusCode)
                            {
                            var Id = mediaArtifact.Split('(')[1];
                            var assetId = Id.Split(',')[0];
                            var imageUrn = assetId.Replace("urn:li:digitalmediaAsset", "urn:li:image");
                            return imageUrn;
                            }
                            else
                            {
                                return "Failed to upload the image.";
                            }
                        }
                    }
                //}
            //    else
            //    {
            //        return "Failed to register the upload.";
            //    }
            }
           
        }

            public async Task<string> LinkedinCommentWithImage( CommentDetailsDto comment, string accessToken)
                    {

                                        var MediaUrn = string.Empty;
                                        string upload_url = string.Empty;


                                        var rez = Task.Run(async () =>
                                        {//1step for image
                                            var responseImage = await InitializeUpload(comment.FromId, accessToken);
                                            return responseImage;
                                        });

                                        var rezJson = JObject.Parse(rez.Result.Item2);

                                        if (rez.Result.Item1 == 200)
                                        {
                                            try // return error from JSON
                                            {
                                                MediaUrn = rezJson["value"]["image"].Value<string>();
                                                upload_url = rezJson["value"]["uploadUrl"].Value<string>();


                                                //upload the image
                                                var client = new HttpClient();
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                                                var data = comment.PhotoUrl.Split(',')[1];
                                                byte[] imgdata = Convert.FromBase64String(data);
                                                var content = new MultipartFormDataContent();
                                                var fileContent = new ByteArrayContent(imgdata);
                                                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                                                content.Add(fileContent, "file", "image.jpg");
                                                HttpResponseMessage putResponse = await client.PutAsync(upload_url, content);
                                                putResponse.EnsureSuccessStatusCode();


                                            }
                                            catch (Exception ex) // return unknown error
                                            {
                                                // log exception somewhere
                                                return null;
                                            }
                                        }

                                        return MediaUrn;
                    }



    public async Task<Response<CommentDto>> CreateSubComment(CommentDetailsDto comment, string accessToken)
        {
            Response<CommentDto> result = new Response<CommentDto>();
            var url = $"https://api.linkedin.com/rest/socialActions/{comment.CommentUrn}/comments";

            var body = new
            {
                actor = comment.FromId,
                message = new
                {
                    text = comment.Message
                },
                parentComment= comment.CommentUrn
            };

           

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var requestJson = JsonConvert.SerializeObject(body);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, requestContent);
                var responseJson = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseJson);


                if (response.IsSuccessStatusCode)
                {
                    CommentDto commentDto = new CommentDto()
                    {
                        Message = comment.Message,
                        FromId = comment.FromId,
                        Replies = new List<CommentDto>(),
                        Reactions = new List<ReactionsDto>(),
                        CreatedTime = DateTime.Now,
                        CommentId = json["id"].ToString(),
                        CommentUrn= json["$URN"].ToString(),
                        LikesCount=0

                    };
                    result.Data = commentDto;
                    result.Succeeded = true;
                    return result;

                }
                else
                {
                    return result;
                }
            }
        }


        public async Task<Response<CommentDto>> EditComment(CommentDetailsDto comment,string accessToken)
        {
            Response<CommentDto> result = new Response<CommentDto>();
            var url = $"https://api.linkedin.com/rest/socialActions/{comment.PostId}/comments/{comment.CommentId}?actor={comment.FromId}";

            JObject body = new JObject
            {
                ["patch"] = new JObject
                {
                    ["message"] = new JObject
                    {
                        ["$set"] = new JObject
                        {
                            ["text"] = comment.Message
                        }
                    }
                }
            };

            var requestJson = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);
                client.DefaultRequestHeaders.Add("X-RestLi-Method", "PARTIAL_UPDATE");

                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var httpContent = await response.Content.ReadAsStringAsync();
                    var linkedincomment = JsonConvert.DeserializeObject<dynamic>(httpContent);
                   
                        result.Succeeded= true;
                    CommentDto commentDto = new CommentDto
                    {
                        Message = comment.Message,
                        CommentId = linkedincomment.id,
                        PhotoUrl = comment.PhotoUrl,
                        VideoUrl = comment.VideoUrl,
                        Replies = comment.Replies,
                        Reactions = new List<ReactionsDto>(),
                        CreatedTime = DateTime.Now,
                        LikesCount = comment.LikesCount

                    };
                    result.Data=commentDto;
                result.Succeeded= true;
                    return result;
                }
                return result;

            }
        }



        public async Task<Response<string>> DeleteComment(CommentDetailsDto comment, string accessToken)
        {
            Response<string> result = new Response<string>();
            var requestUrl = $"https://api.linkedin.com/rest/socialActions/{comment.PostId}/comments/{comment.CommentId}?actor={comment.FromId}";


            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                var response = await httpClient.DeleteAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result.Succeeded = true;
                    result.Data = responseJson.ToString();
                    return result;
                }
                else
                {
                    return result;
                }
            }



        }

        public Task<string> CreateReaction(string accessToken, string authorUrn, string entityUrn)
        {
            throw new NotImplementedException();
        }

        #region Post
        public async Task<LinkedInPost>GetLastsLinkedInPost(LinkedInChannelDto linkedInChannel,int count)
        {
            var apiUrl = $"https://api.linkedin.com/rest/posts?author="; 
                //{WebUtility.UrlEncode(linkedInChannel.CompanyUrn)}&q=author&count="+count+"&sortBy=(value:CREATED)";
            var latestPost =new LinkedInPost();
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
                //client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                //client.DefaultRequestHeaders.Add("X-RestLi-Method", "FINDER");
                //client.DefaultRequestHeaders.Add("LinkedIn-Version", _versionNumber);

                //var httpResponse = await client.GetAsync(apiUrl);
                //var httpContent = await httpResponse.Content.ReadAsStringAsync();

                //var responseJson = JObject.Parse(httpContent);
                //var elements = responseJson["elements"];

                //if (elements.Count() > 0)
                //{
                //    var firstPost = elements.First;

                //    string postId = firstPost["id"].ToString();
                //    long milliseconds = firstPost["publishedAt"].Value<long>();

                //    DateTime createdAt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                //        .AddMilliseconds(milliseconds);

                //    string authorUrn = firstPost["author"].ToString();
                //    string commentary = firstPost["commentary"].ToString();

                //    /* Post Details*/

                //    LinkedInPost latestPost = new LinkedInPost
                //    {
                //        postId = postId,
                //       // author_urn = authorUrn,
                //        Message = commentary,
                //        createdAt = createdAt,
                //    };



                //    //if (!Convert.ToBoolean(firstPost["content"]))

                //    var mediaId = firstPost["content"]["media"]["id"].ToString();

                //    latestPost.MediaUrn = mediaId;

                //    if (mediaId.StartsWith("urn:li:image"))
                //    {

                //        //latestPost.mediaUrl = await GetImageUrl(latestPost.mediaUrn, accessToken);
                //        latestPost.Photo = latestPost.MediaUrn;

                //    }
                //    else if (mediaId.StartsWith("urn:li:video"))
                //    {
                //        //latestPost.mediaUrl = await GetVideoUrl(latestPost.mediaUrn, accessToken);
                //        latestPost.Video = latestPost.MediaUrn;
                //    }




                return latestPost;


            //}
                

                //else
                //{
                //    return null;
                //}
            
            }
        }

        public Task<LinkedInPost> GetLastsLinkedInPost(Post post)
        {
            throw new NotImplementedException();
        }

       

        





        //Task<LinkedInPost> ILinkedInAPIRepository<Guid>.PublishToLinkedIn(LinkedInPostDto post, LinkedInProfileDataDto linkedInProfileDto)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<string> LinkedinPostWithVideo(string accessToken, string author_urn, LinkedInPostDto post)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }


    public class LinkedInAccountTokens
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string refresh_token_expires_in { get; set; }
        public string scope { get; set; }
    }


}
