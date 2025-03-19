using MediatR;

namespace SNM.Instagram.Application.Features.Queries.GetEntities
{
    public class GetEntitiesQuery : IRequest<List<GetEntitiesViewModel>>
    {
    }
}