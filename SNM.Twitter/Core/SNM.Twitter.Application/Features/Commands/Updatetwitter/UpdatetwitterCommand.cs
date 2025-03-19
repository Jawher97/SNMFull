using MediatR;

namespace SNM.Twitter.Application.Features.Commands.Updatetwitter
{
    public class UpdatetwitterCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}