using SNS.Facebook.Application.Catalog.Brands;

namespace SNS.Facebook.Application.Catalog.Products
{
    public class ProductDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Rate { get; set; }
        public string? ImagePath { get; set; }
        public BrandDto Brand { get; set; } = default!;
    }
}