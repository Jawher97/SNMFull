﻿using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Facebook.Domain.Catalog;

namespace SNS.Facebook.Infrastructure.Persistence.Configuration
{
    public class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.IsMultiTenant();

            builder
                .Property(b => b.Name)
                    .HasMaxLength(256);
        }
    }

    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.IsMultiTenant();

            builder
                .Property(b => b.Name)
                    .HasMaxLength(1024);

            builder
                .Property(p => p.ImagePath)
                    .HasMaxLength(2048);
        }
    }
}