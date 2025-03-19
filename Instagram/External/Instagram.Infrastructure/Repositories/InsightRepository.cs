using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Infrastructure.DataContext;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Configuration;



namespace SNM.Instagram.Infrastructure.Repositories
{
    public class InsightRepository : IInsightRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly IConfiguration _configuration;

        public InsightRepository(ApplicationDbContext dbContext , IConfiguration configuration) 
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _accessToken = configuration["Instagram:AccessToken"];
            _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v16.0/") };
            _configuration = configuration;
        }

        /*public async Task<IEnumerable<Insight>> GetInsightsAsync(string username, string mediaType, string period, string metric, string accessToken, DateTime? since = null)
        {
            return await _dbContext.Set<Insight>()
                         .Where(i => i.Name == username && i.MediaType == mediaType && i.Period == period && i.Metric == metric && (since == null || i.Date >= since))
                         .ToListAsync();
        }

        public async Task<IEnumerable<Insight>> GetByInsightsAsync(string name, string period, DateTime date, string metric)
        {
            var result = new List<Insight>();
            var insights = await GetInsightsAsync(name, "", period, metric, "");
            foreach (var insight in insights)
            {
                result.Add(new Insight
                {
                    Name = insight.Name,
                    Period = insight.Period,
                    Date = insight.Date,
                    Value = insight.Values.FirstOrDefault()?.Value ?? 0 // ou une autre valeur par défaut
                });
            }
            return result;
        }*/

        public async Task AddAsync(Insight entity)
        {
            await _dbContext.Set<Insight>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        

        public async Task<string> GetInsightsAsync(string metric, string period, string since, string until)
        {

                // Ajouter le jeton d'accès à l'en-tête de la demande HTTP
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "EAAHe7jYz7d0BAAyEvFCuiH31PCkeZAKYxaxNnuvZCNa2L9faroJouCpSzL5qcKeZAvOXUWEZCdm4ddIKS9JSop9vZCBkDsnX8Fd2ZAsZBhUlqBkdPd6BUjvSuO3sIDpZAxYPZAQomZCmxJvGxUDM1ptWdOZAvgh85FwqZBbGTFQ1ubDIyFKkzJm7HYHZC");

                // Envoyer la demande HTTP et récupérer la réponse
                var userId = _configuration["Instagram:UserId"];
                var response = await _httpClient.GetAsync($"{userId}/insights?metric={metric}&period={period}&since={since}&until={until}");
                var responseContent = await response.Content.ReadAsStringAsync();

                // Vérifier si la demande a réussi
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = $"Request failed with status code {response.StatusCode}. Response content: {responseContent}";
                    throw new Exception(errorMessage);
                }


                return responseContent.ToString();

      
        }
    }
}