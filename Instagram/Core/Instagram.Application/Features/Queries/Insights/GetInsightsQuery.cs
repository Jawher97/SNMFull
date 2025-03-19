using AutoMapper;
using MediatR;
using System.Collections.Generic;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SNM.Instagram.Domain.Entities;

namespace SNM.Instagram.Application.Features.Queries.Insights
{
    public class GetInsightsQuery : IRequest<List<InsightDto>>
    {
        public string Name { get; set; }
        public string Period { get; set; }
        public DateTime Date { get; set; }
        public string Metric { get; set; }
    }

    public class GetInsightListQueryHandler : IRequestHandler<GetInsightsQuery, List<InsightDto>>
    {
        private readonly IInsightRepository _repository;
        private readonly IMapper _mapper;

        public GetInsightListQueryHandler(IInsightRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<InsightDto>> Handle(GetInsightsQuery request, CancellationToken cancellationToken)
        {
                throw new NotImplementedException();
        }
    }
}









