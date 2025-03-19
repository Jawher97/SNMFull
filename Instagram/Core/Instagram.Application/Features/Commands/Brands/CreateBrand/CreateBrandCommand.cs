using MediatR;
using SNM.Instagram.Application.DTO;

namespace SNM.Instagram.Application.Features.Commands.Brands.CreateBrand
{
    public class CreateBrandCommand : IRequest<Guid>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
      public string Photo { get; set; }
       public string CoverPhoto { get; set; }
        public string TimeZone { get; set; }
        public BrandDto Brand { get; set; }
    }
}
