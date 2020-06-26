using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaleIt.Domain.Sale.Entities;
using SaleIt.Domain.Sale.Repositories;

namespace SaleIt.Data.Sale.Repositories
{
    public class SaleDocumentRepository : ISaleDocumentRepository
    {
        public SaleDocumentRepository(SaleItDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly SaleItDbContext dbContext;

        public async Task<SaleDocument> FindAsync(Guid saleId)
        {
            var sale = await dbContext.Sales.FindAsync(saleId);
            if (sale != null)
            {
                await dbContext.Entry(sale)
                    .Collection(i => i.Lines).LoadAsync();
            }

            return sale;
        }

        public SaleDocument Add(SaleDocument saleDocument)
        {
            return dbContext.Sales.Add(saleDocument).Entity;
        }

        public void Update(SaleDocument saleDocument)
        {
            dbContext.Entry(saleDocument).State = EntityState.Modified;
        }

        public void Remove(Guid saleId)
        {
            //find remove or attach remove?
            var saleDocument = SaleDocument.CreateForDelete(saleId);
            dbContext.Sales.Attach(saleDocument);
            dbContext.Sales.Remove(saleDocument);
        }
    }
}
