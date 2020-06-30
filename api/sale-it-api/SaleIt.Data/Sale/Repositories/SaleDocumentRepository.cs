using System;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SaleIt.Domain.Sale.Entities;
using SaleIt.Domain.Sale.Repositories;

namespace SaleIt.Data.Sale.Repositories
{
    public class SaleDocumentRepository : EntityFrameworkRepository<SaleDocument> ,ISaleDocumentRepository
    {
        public SaleDocumentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<SaleDocument?> FindAsync(Guid saleId)
        {
            var sale = await base.FindAsync(saleId);
            if (sale != null)
            {
                await dbContext.Entry(sale)
                    .Collection(i => i.Lines).LoadAsync();
            }

            return sale;
        }

        public SaleDocument? Add(SaleDocument? saleDocument)
        {
            return base.Insert(saleDocument);
        }

        public void Update(SaleDocument? saleDocument)
        {
            base.Update(saleDocument); 
        }

        public void Remove(Guid saleId)
        {
            base.Delete(saleId); 
        }
    }
}
