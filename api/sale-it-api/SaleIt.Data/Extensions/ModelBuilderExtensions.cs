using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaleIt.Data.Sale.EntityFramework.Configurations;

namespace SaleIt.Data.Extensions
{
    internal static class ModelBuilderExtensions
    {
        public static void ApplyConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SaleDocumentConfiguration());
            modelBuilder.ApplyConfiguration(new SaleDocumentLineConfiguration());
            //modelBuilder.ApplyConfiguration(new PaymentDataConfiguration());
            //modelBuilder.ApplyConfiguration(new CustomerDataConfiguration());
            //modelBuilder.ApplyConfiguration(new UserDataConfiguration());
        }
    }
}
