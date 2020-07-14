using Microsoft.EntityFrameworkCore;
using SaleIt.Domain.Customers.Entities;
using SaleIt.Domain.Customers.Repositories;

namespace SaleIt.Data.Customers.Repositories
{
    public class CustomerRepository : EntityFrameworkRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
