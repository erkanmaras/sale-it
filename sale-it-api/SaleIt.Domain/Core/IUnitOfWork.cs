using SaleIt.Domain.Sale.Repositories;
using System;
using System.Threading.Tasks;

namespace SaleIt.Domain.Core
{

   public interface IUnitOfWork : IDisposable
   {


       ISaleDocumentRepository SaleDocumentRepository { get; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges();

       /// <summary>
       /// Asynchronously saves all changes made in this unit of work to the database.
       /// </summary>
       /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
       Task<int> SaveChangesAsync();
   }
}
