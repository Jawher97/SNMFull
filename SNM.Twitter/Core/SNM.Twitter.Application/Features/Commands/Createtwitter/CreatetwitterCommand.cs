using MediatR;

namespace SNM.Twitter.Application.Features.Commands.Createtwitter
{
    public class CreatetwitterCommand : IRequest<Guid>
    {
        public string Message { get; set; }
    }
}