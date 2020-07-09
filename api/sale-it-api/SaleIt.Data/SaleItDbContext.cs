using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaleIt.Data.Extensions;
using SaleIt.Domain.CustomerInformation.Entities;
using SaleIt.Domain.Sale.Entities;

namespace SaleIt.Data
{
    public class SaleItDbContext : DbContext
    {

        public DbSet<SaleDocument?> Sales { get; set; } = null!;
        public DbSet<SaleDocumentLine?> SaleLines { get; set; } = null!;
        public DbSet<SaleDocumentLine?> Products { get; set; } = null!;
        public DbSet<Customer?> Customers { get; set; } = null!;
        //public DbSet<Payment> Payments { get; set; }
        //public DbSet<UserData> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {  
            // ReSharper disable once StringLiteralTypo
            options.UseSqlite(@"Data Source=saleit.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurations();
        }
    }
}
