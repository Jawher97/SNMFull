using MediatR;

namespace SNM.BrandManagement.Application.Features.Commands.Brands.DeleteBrand
{
    public class DeleteBrandCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}