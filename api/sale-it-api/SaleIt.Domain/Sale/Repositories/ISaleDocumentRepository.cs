using System;
using System.Threading.Tasks;
using SaleIt.Domain.Core;
using SaleIt.Domain.Sale.Entities;

namespace SaleIt.Domain.Sale.Repositories
{
    public interface ISaleDocumentRepository : IRepository
    {
        Task<SaleDocument?> FindAsync(Guid saleId);

        SaleDocument? Add(SaleDocument saleDocument);

        void Update(SaleDocument? saleDocument);

        void Remove(Guid saleId);
    }
}
