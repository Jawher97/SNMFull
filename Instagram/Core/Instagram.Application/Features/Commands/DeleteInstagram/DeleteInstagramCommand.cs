using MediatR;

namespace SNM.Instagram.Application.Features.Commands.DeleteInstagram
{
    public class DeleteInstagramCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}