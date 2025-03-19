using MediatR;

namespace SNM.Instagram.Application.Features.Queries.GetEntityById
{
    public class GetEntityByIdQuery : IRequest<GetParameterId>
    {
        public GetEntityByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}