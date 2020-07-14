using System;
using SaleIt.Domain.Core;

namespace SaleIt.Domain.Sales.Entities
{
    public class Product: Entity
    {

        private Product()
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

        private Guid productId;
        public Guid ProductId => productId;

        private string barCode = null!;
        public string Barcode => barCode;

        private string name = null!;
        public string Name => name;

        private decimal taxRate;
        public decimal TaxRate => taxRate;

        private decimal price;
        public decimal Price => price;
    }
}
