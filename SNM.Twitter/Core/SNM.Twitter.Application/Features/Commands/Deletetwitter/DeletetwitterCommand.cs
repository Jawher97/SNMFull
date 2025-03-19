using MediatR;

namespace SNM.Twitter.Application.Features.Commands.Deletetwitter
{
    public class DeletetwitterCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}