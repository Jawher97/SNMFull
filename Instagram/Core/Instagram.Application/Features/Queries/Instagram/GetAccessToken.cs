using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json.Linq;
using AutoMapper;
using SNM.Instagram.Application.Interfaces;

namespace SNM.Instagram.Application.Features.Queries.Instagram
{
    public class GetAccessTokenByCodeQuery : IRequest<string>
    {
        public GetAccessTokenByCodeQuery(string clientId, string clientSecret, string redirectUri, string code, string Scopes)
        {
            ClientId = "526589669666269";
            ClientSecret = "0f2cc4293be57ac3d0b7f11520652126";
            RedirectUri = "https://www.artbr.com/";
            Code = code;
            Scopes = "public_profile,instagram_basic,pages_show_list";

        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Code { get; set; }
        public string Scopes { get; set; }

    }

    public class GetAccessTokenByCodeQueryHandler : IRequestHandler<GetAccessTokenByCodeQuery, string>
    {
        private readonly IInstagramRepository<Guid> _repository;
        private readonly IMapper _mapper;

        public GetAccessTokenByCodeQueryHandler(IInstagramRepository<Guid> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> Handle(GetAccessTokenByCodeQuery request, CancellationToken cancellationToken)
        {
            var url = $"https://api.instagram.com/oauth/access_token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "526589669666269"),
                new KeyValuePair<string, string>("client_secret", "0f2cc4293be57ac3d0b7f11520652126"),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", "https://www.artbr.com/"),
                new KeyValuePair<string, string>("code", request.Code),
                new KeyValuePair<string, string>("scope", "public_profile,instagram_basic,pages_show_list"),

            });

            using (var http = new HttpClient())
            {
                var httpResponse = await http.PostAsync(url, content);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var responseJson = JObject.Parse(httpContent);

                if (responseJson["access_token"] == null)
                    throw new Exception("Unable to retrieve access token");

                var accessToken = responseJson["access_token"].ToString();

                return accessToken;
            }
        }
    }
}

