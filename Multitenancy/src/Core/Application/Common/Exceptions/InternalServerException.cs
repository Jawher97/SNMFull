using System.Net;

namespace Multitenancy.Application.Identity
{
    public class InternalServerException : CustomException
    {
        public InternalServerException(string message, List<string>? errors = default)
            : base(message, errors, HttpStatusCode.InternalServerError)
        {
        }
    }
}