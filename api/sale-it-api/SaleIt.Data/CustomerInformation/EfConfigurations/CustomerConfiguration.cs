using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleIt.Domain.CustomerInformation.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaleIt.Domain.Customer.ValueObjects;

namespace SaleIt.Data.CustomerInformation.EfConfigurations
{
  internal  class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(s => s.CustomerId);
        }
    }
}
