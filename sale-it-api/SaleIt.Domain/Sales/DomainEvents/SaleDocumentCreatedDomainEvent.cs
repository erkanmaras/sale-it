using System;
using SaleIt.Mediator;

namespace SaleIt.Domain.Sales.DomainEvents
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
