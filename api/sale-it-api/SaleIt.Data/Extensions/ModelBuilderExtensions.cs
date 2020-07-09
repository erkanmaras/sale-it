
using Microsoft.EntityFrameworkCore;
using SaleIt.Data.CustomerInformation.EfConfigurations;
using SaleIt.Data.Sale.EfConfigurations;

namespace SaleIt.Data.Extensions
{
    internal static class ModelBuilderExtensions
    {
        public static void ApplyConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SaleDocumentConfiguration());
            modelBuilder.ApplyConfiguration(new SaleDocumentLineConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            //modelBuilder.ApplyConfiguration(new PaymentDataConfiguration());
            //modelBuilder.ApplyConfiguration(new CustomerDataConfiguration());
            //modelBuilder.ApplyConfiguration(new UserDataConfiguration());
        }
    }
}
