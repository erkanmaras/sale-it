
using System;
using SaleIt.Domain.Shared.ValueObjects;

namespace SaleIt.Domain.CustomerInformation.Entities
{
    public class Customer
    {
        private Customer()
        {

        }

        public Customer(Guid customerId, string name, string surname, Address address)
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

        private Address address;
        public Address Address => address;
    }
}
