
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaleIt.Domain.Sale.Entities;

namespace SaleIt.Data.Sale.EfConfigurations
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
