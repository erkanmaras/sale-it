using SaleIt.Domain.Core;
using System.Collections.Generic;

namespace SaleIt.Domain.Shared.ValueObjects
{
    public class Address : ValueObject
    {


        private Address() { }

        public Address(string country, string state, string city, string district, string street, string zipCode)
        {

            this.country = country;
            this.state = state;
            this.city = city;
            this.district = district;
            this.street = street;
            this.zipCode = zipCode;
        }

        private readonly string street;
        public string Street => street;


        private readonly string district;
        public string District => district;

        private readonly string city;
        public string City => city;

        private readonly string state;
        public string State => state;

        private readonly string country;
        public string Country => country;

        private readonly string zipCode;
        public string ZipCode => zipCode;

        protected override IEnumerable<object> GetEquitableValues()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return District;
            yield return ZipCode;
        }
    }

}
