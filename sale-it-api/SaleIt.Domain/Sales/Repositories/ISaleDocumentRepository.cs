using System;
using System.Threading.Tasks;
using SaleIt.Domain.Core;
using SaleIt.Domain.Sales.Entities;

namespace SaleIt.Domain.Sales.Repositories
{
    public interface ISaleDocumentRepository : IRepository
    {
        Task<SaleDocument?> FindAsync(Guid saleId);

        SaleDocument? Add(SaleDocument saleDocument);

        void Update(SaleDocument saleDocument);

        void Remove(Guid saleId);

    }
}
