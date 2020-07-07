using SaleIt.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleIt.Domain.Sale.DomainEvents
{
    public class SaleDocumentCreatedDomainEvent : INotification
    {
        SaleDocumentCreatedDomainEvent(Guid saleId)
        {
            SaleId = saleId;
        }

        Guid SaleId { get; }
    }
}
