
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SNM.Instagram.Application.Interfaces;

public class InstagramPublishService : IInstagramPublishService
{
    private readonly HttpClient _httpClient;
    private readonly string _accessToken;
    private readonly IConfiguration _configuration;

    public InstagramPublishService(IConfiguration configuration)
    {
        _configuration = configuration;
        _accessToken = configuration["Instagram:AccessToken"];
        _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v16.0/") };
    }

    public Task AddInstagramPostAsync(InstagramPostDto post)
    {
        throw new NotImplementedException();
    }

    public Task<InstagramPostDto> GetPostByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<InstagramPostDto> PublishPostOnInstagramAsync(string image_url, string caption)
    {
        // Créer le contenu de la publication
       // var content0 = new MultipartFormDataContent();
        //content0.Add(new StringContent(image_url), "image_url");
       // content0.Add(new StringContent(caption), "caption");
        //content0.Add(new StringContent("1"), "published_shed");

        var content = new Dictionary<string, string> {
                { "image_url", image_url },
                { "caption", caption },
                { "published_shed", "1" } ,
        };
        // Ajouter le jeton d'accès à l'en-tête de la demande HTTP
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAADuIPRhw9oBO7oWE8gqogDprVDVYAgsjobnhtjewM4NTJHDGSGOFLU3P1EZBV57sFzGBQZCXkLfwVlSlIdPDEZB5kgvloLEBcoyaSDglhZAgRg3OMVc0aYutT6LBUZC6c4uT9LYEVEzIPxxSB9yqfaLUceyzOjQhumDdpFZApmH2uXS0aq6neohwrZAZAJiq8pt");

        // Envoyer la demande HTTP et récupérer la réponse
        var userId = _configuration["Instagram:UserId"];
        var response = await _httpClient.PostAsync($"{userId}/media", new FormUrlEncodedContent(content));

        // Vérifier si la demande a réussi
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorMessage = $"Request failed with status code {response.StatusCode}. Response content: {responseContent}";
            throw new Exception(errorMessage);
        }

        // Analyser la réponse JSON
        var json = await response.Content.ReadAsStringAsync();
        var media = JsonConvert.DeserializeObject<InstagramMediaPublishResponse>(json);

        // Publier la publication
        response = await _httpClient.PostAsync($"{userId}/media_publish?creation_id={media.Id}", null);

        // Vérifier si la demande a réussi
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorMessage = $"Request failed with status code {response.StatusCode}. Response content: {responseContent}";
            throw new Exception(errorMessage);
        }

        // Analyser la réponse JSON
        json = await response.Content.ReadAsStringAsync();
        var post = JsonConvert.DeserializeObject<InstagramPostDto>(json);

        // Retourner les informations de la publication
        return post;
            
    }



    public async Task<InstagramPostDto> PublishVideoPostOnInstagramAsync(string video_url, string caption, string media_type)
    {
        // Créer le contenu de la publication
        // var content0 = new MultipartFormDataContent();
        //content0.Add(new StringContent(image_url), "image_url");
        // content0.Add(new StringContent(caption), "caption");
        //content0.Add(new StringContent("1"), "published_shed");

        var content = new Dictionary<string, string> {
                { "media_type", media_type },
                { "video_url", video_url },
                { "caption", caption },
                { "published_shed", "1" } ,
        };
        // Ajouter le jeton d'accès à l'en-tête de la demande HTTP
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAADuIPRhw9oBO7oWE8gqogDprVDVYAgsjobnhtjewM4NTJHDGSGOFLU3P1EZBV57sFzGBQZCXkLfwVlSlIdPDEZB5kgvloLEBcoyaSDglhZAgRg3OMVc0aYutT6LBUZC6c4uT9LYEVEzIPxxSB9yqfaLUceyzOjQhumDdpFZApmH2uXS0aq6neohwrZAZAJiq8pt");

        // Envoyer la demande HTTP et récupérer la réponse
        var userId = _configuration["Instagram:UserId"];
        var response = await _httpClient.PostAsync($"{userId}/media", new FormUrlEncodedContent(content));

        // Vérifier si la demande a réussi
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorMessage = $"Request failed with status code {response.StatusCode}. Response content: {responseContent}";
            throw new Exception(errorMessage);
        }

        // Analyser la réponse JSON
        var json = await response.Content.ReadAsStringAsync();
        var media = JsonConvert.DeserializeObject<InstagramMediaPublishResponse>(json);

        // Publier la publication
        response = await _httpClient.PostAsync($"{userId}/media_publish?creation_id={media.Id}", null);

        // Vérifier si la demande a réussi
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorMessage = $"Request failed with status code {response.StatusCode}. Response content: {responseContent}";
            throw new Exception(errorMessage);
        }

        // Analyser la réponse JSON
        json = await response.Content.ReadAsStringAsync();
        var post = JsonConvert.DeserializeObject<InstagramPostDto>(json);

        // Retourner les informations de la publication
        return post;

    }
}