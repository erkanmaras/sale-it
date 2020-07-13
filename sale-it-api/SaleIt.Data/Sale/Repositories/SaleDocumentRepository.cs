using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaleIt.Domain.Sale.Entities;
using SaleIt.Domain.Sale.Repositories;

namespace SaleIt.Data.Sale.Repositories
{
    public class SaleDocumentRepository : EntityFrameworkRepository<SaleDocument>, ISaleDocumentRepository
    {
        public SaleDocumentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        async Task<SaleDocument?> ISaleDocumentRepository.FindAsync(Guid saleId)
        {
            return await base.GetFirstOrDefaultAsync(predicate:s=> s.SaleId==saleId,
                include:s=> s.Include(f => f.Lines));
        }

        SaleDocument ISaleDocumentRepository.Add(SaleDocument saleDocument)
        {
            return base.Add(saleDocument);
        }

        void ISaleDocumentRepository.Update(SaleDocument saleDocument)
        {
            base.Update(saleDocument);
        }

        void ISaleDocumentRepository.Remove(Guid saleId)
        {
            base.Remove(saleId);
        }
    }
}
