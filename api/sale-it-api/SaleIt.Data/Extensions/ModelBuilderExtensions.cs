using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaleIt.Data.Sale.EfConfigurations;

namespace SaleIt.Data.Extensions
{
    public static class ModelBuilderExtensions
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
