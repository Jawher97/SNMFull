using MediatR;

namespace SNM.Instagram.Application.Features.Commands.Brands.DeleteBrand
{
    public class DeleteBrandCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}