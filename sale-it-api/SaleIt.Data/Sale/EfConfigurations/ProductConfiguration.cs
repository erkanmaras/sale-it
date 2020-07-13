
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaleIt.Domain.Sale.Entities;

namespace SaleIt.Data.Sale.EfConfigurations
{
  internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(s => s.ProductId);
        }
    }
}
