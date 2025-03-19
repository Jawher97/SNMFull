
namespace SNS.Facebook.Application.Features.Queries.Facebook
{
    using AutoMapper;
    using global::SNS.Facebook.Application.Interfaces;
    using MediatR;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading.Tasks;

    namespace SNS.Facebook.Application.Features.Queries.Posts
    {
        public class GetAccessTokenByUrkQuery : IRequest<GetAccessTokenParameterId>
        {
            public GetAccessTokenByUrkQuery(string url)
            {
                Url = url;
            }

            //public Guid Id { get; set; }
            public string Url { get; set; }
        }
        public class GetAccessTokenParameterId
        {
            public Guid Id { get; set; }
            public string Url { get; set; }
            public string TokenObject { get; set; }
        }
        public class GetEntityByIdQueryHandler : IRequestHandler<GetAccessTokenByUrkQuery, object>
        {
            private readonly IFacebookRepository<Guid> _repository;
            private readonly IMapper _mapper;

            public GetEntityByIdQueryHandler(IFacebookRepository<Guid> repository, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<object> Handle(GetAccessTokenByUrkQuery request, CancellationToken cancellationToken)
            {
                var entity = await GetAccessTokenByUrlAsync(request.Url);

                return entity;// await Task.FromResult(_mapper.Map<object>(entity));
            }
            public async Task<object> GetAccessTokenByUrlAsync(string url)
            {
                var rez = await Task.Run(async () =>
                {
                    using (var http = new HttpClient())
                    {
                        var httpResponse = await http.GetAsync(url);
                        var httpContent = await httpResponse.Content.ReadAsStringAsync();

                        return httpContent;
                    }
                });
                var rezJson = JObject.Parse(rez);
                return rezJson;
            }
        }
    }
}
