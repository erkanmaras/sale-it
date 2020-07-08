using SaleIt.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleIt.Domain.Customer.ValueObjects
{
    public class CustomerAddress : ValueObject
    {


        private CustomerAddress() { }

        public CustomerAddress(string country, string state, string city, string district, string street, string zipcode)
        {

            _country = country;
            _state = state;
            _city = city;
            _district = district;
            _street = street;
            _zipCode = zipcode;
        }

        private readonly string _street;
        public string Street => _street;


        private readonly string _district;
        public string District => _district;

        private readonly string _city;
        public string City => _city;

        private readonly string _state;
        public string State => _state;

        private readonly string _country;
        public string Country => _country;

        private readonly string _zipCode;
        public string ZipCode => _zipCode;

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
