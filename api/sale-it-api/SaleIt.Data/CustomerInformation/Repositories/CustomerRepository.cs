using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SaleIt.Domain.CustomerInformation.Entities;
using SaleIt.Domain.CustomerInformation.Repositories;


namespace SaleIt.Data.CustomerInformation.Repositories
{
    public class CustomerRepository : EntityFrameworkRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
