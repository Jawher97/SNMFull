using AutoMapper;
using MediatR;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.BrandManagement.Application.Features.Queries.ChannelTypes
{
    public class GetChannelTypesWithChannels : IRequest<ChannelTypeDto>
    {
    //    public GetChannelTypesWithChannels(string name)
    //    {
    //        Name = name;
    //    }

    //    public string Name { get; set; }
    //}
    ////public class GetChannelTypeParameterBrandId
    ////{
    ////    public ChannelTypeDto ChannelType { get; set; }

    ////}
    //public class GetChannelTypesWithChannelsQueryHandler : IRequestHandler<GetChannelTypesWithChannels, ChannelTypeDto>
    //{
    //    private readonly IChannelTypeRepository<Guid> _repository;
    //    private readonly IMapper _mapper;

    //    public GetChannelTypesWithChannelsQueryHandler(IChannelTypeRepository<Guid> repository, IMapper mapper)
    //    {
    //        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    //        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    //    }

    //    public async Task<ChannelTypeDto> Handle(GetChannelTypesWithChannels request, CancellationToken cancellationToken)
    //    {
    //        var entity = await _repository.GetChannelTypesWithChannels(request.Name);

    //        return await Task.FromResult(_mapper.Map<ChannelTypeDto>(entity));
    //    }
    //}
}
}
