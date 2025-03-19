using MediatR;

namespace SNM.Instagram.Application.Features.Commands.Brands.UpdateBrand
{
    public class UpdateBrandCommand : IRequest
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
        public string TimeZone { get; set; }

    }
}
