using MediatR;

namespace SNM.Instagram.Application.Features.Commands.UpdateInstagram
{
    public class UpdateInstagramCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}