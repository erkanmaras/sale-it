using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaleIt.Data.Sales.Repositories;
using SaleIt.Mediator;
using SaleIt.Domain.Core;
using SaleIt.Domain.Sales.Repositories;
using SaleIt.Infrastructure.Extensions;

namespace SaleIt.Data
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IUnitOfWork"/> interface.
    /// </summary>
    /// <typeparam name="TContext">The type of the db context.</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {

        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The context.</param>
        /// <param name="mediator"></param>
        public UnitOfWork(TContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private readonly TContext dbContext;
        private readonly IMediator mediator;
        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
        public TContext DbContext => dbContext;

        private ISaleDocumentRepository saleDocumentRepository = null!;
        public ISaleDocumentRepository SaleDocumentRepository
        {
            get
            {
                if (saleDocumentRepository == null)
                {
                    saleDocumentRepository = new SaleDocumentRepository(dbContext);
                }

                return saleDocumentRepository;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            await DispatchDomainEventsAsync();
            return await dbContext.SaveChangesAsync();
        }


        public async Task DispatchDomainEventsAsync()
        {
            var entities = dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.HasItem()).ToArray();

            var domainEvents = new List<INotification>();
            foreach (var entityEntry in entities)
            {
                if (entityEntry.Entity.DomainEvents.HasItem())
                {
                    domainEvents.AddRange(entityEntry.Entity.DomainEvents);
                }
            }

            Array.ForEach(entities, entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            } 
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose the db context.
                    dbContext.Dispose();
                }
            }

            disposed = true;
        }
    }
}
