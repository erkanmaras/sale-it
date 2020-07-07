using SaleIt.Domain.Core;
using System;

namespace SaleIt.Domain.Sale.Entities
{
    public class Product: Entity
    {

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Product()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            // required for orm
        }

        private Product(Guid productId, string barCode, string name, decimal taxRate, decimal price)
        {
            this.productId = productId;
            this.barCode = barCode;
            this.name = name;
            this.taxRate = taxRate;
            this.price = price;
        }

        public Guid productId;
        public Guid ProductId => productId;

        public string barCode;
        public string Barcode => barCode;

        public string name;
        public string Name => name;

        public decimal taxRate;
        public decimal TaxRate => taxRate;

        public decimal price;
        public decimal Price => price;
    }
}
