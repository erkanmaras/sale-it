using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaleIt.Domain.Sales.Entities;

namespace SaleIt.Data.Sales.EfConfigurations
{
    internal class SaleDocumentLineConfiguration : IEntityTypeConfiguration<SaleDocumentLine>
    {
        public void Configure(EntityTypeBuilder<SaleDocumentLine> builder)
        {
            builder.ToTable("SaleDocumentLine");
            builder.HasKey(s => s.SaleLineId);
            builder.Property(e => e.Amount).HasConversion<double>();
            builder.Property(e => e.Discount).HasConversion<double>();
            builder.Property(e => e.Tax).HasConversion<double>();
        }
    }
}
