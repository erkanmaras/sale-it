using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaleIt.Domain.Sale.Entities;

namespace SaleIt.Data.Sale.EntityFramework.Configurations
{
    internal class SaleDocumentConfiguration : IEntityTypeConfiguration<SaleDocument>
    {
        public void Configure(EntityTypeBuilder<SaleDocument> builder)
        {
            builder.ToTable("SaleDocument");
            builder.HasKey(s => s.SaleId);
            builder.HasMany(s => s.Lines);
        }
    }
}
