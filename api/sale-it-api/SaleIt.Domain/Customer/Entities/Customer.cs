using SaleIt.Domain.Customer.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleIt.Domain.Customer.Entities
{
    public class Customer
    {
        private Customer()
        {

        }

        public Customer(Guid customerId, string name, string surname, CustomerAddress address)
        {
            this.customerId = customerId;
            this.name = name;
            this.surname = surname;
            this.address = address;
        }

        private Guid customerId;
        public Guid CustomerId => customerId;

        private string name;
        public string Name => name;

        private string surname;
        public string Surname => surname;

        private CustomerAddress address;
        public CustomerAddress Address => address;
    }
}
