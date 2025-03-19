using MediatR;

namespace SNM.Instagram.Application.Features.Commands.CreateInstagram
{
    public class CreateInstagramCommand : IRequest<Guid>
    {
        public string Message { get; set; }
    }
}