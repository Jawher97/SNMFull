using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using SNM.Instagram.Application.DTO;
using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Domain.Common;
using SNM.Instagram.Domain.Entities;
using SNM.Instagram.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Commands.Insights
{
    public class CreateInsightCommand : IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public DateTime Date { get; set; }
        public string Metric { get; set; }
        public int Value { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<ValueItem> Values { get; set; }
    }
    public class ValueItem
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }
    }

    public class CreateInsightCommandHandler : IRequestHandler<CreateInsightCommand>
    {
        private readonly IInsightRepository _repository;
        private readonly IMapper _mapper;

        public CreateInsightCommandHandler(IInsightRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Unit> Handle(CreateInsightCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Insight>(request);

            await _repository.AddAsync(entity);

            return Unit.Value;
        }
    }
}
