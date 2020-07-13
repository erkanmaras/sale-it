using SaleIt.Bus;
using System;

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
