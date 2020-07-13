using SaleIt.Bus;
using System.Collections.Generic;

namespace SaleIt.Domain.Core
{
  public  class Entity
    {
        private List<INotification>? domainEvents;
        public IReadOnlyCollection<INotification>? DomainEvents => domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents ??= new List<INotification>();
            domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            domainEvents?.Clear();
        }
    }
}
