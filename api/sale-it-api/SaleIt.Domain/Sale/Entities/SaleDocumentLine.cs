using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleIt.Domain.Sale.Entities
{
    public class SaleDocumentLine
    {
        
        public SaleDocumentLine(Guid saleLineId, Guid saleId,Product product, decimal units, decimal discount  , decimal tax ,decimal amount)
        {
            this.saleLineId = saleLineId;
            this.saleId = saleId;
            this.product = product;
            this.productId = product.ProductId;
            this.units = units;
            this.discount = discount;
            this.tax = tax;
            this.amount = amount;
        }

        private Guid saleLineId;
        public Guid SaleLineId => saleLineId;

        private Guid saleId;
        public Guid SaleId => saleId;

        private Guid productId;
        public Guid ProductId => productId;

        private  decimal units;
        public decimal Units => units;

        private  decimal discount;
        public decimal Discount => discount;

        private  decimal tax;
        public decimal Tax => tax;

        private  decimal amount;
        public decimal Amount => amount;

        private  Product product;
        private Product Product => product;

        public void SetDiscount(decimal value)
        {
            this.discount = value;
        }

        public void SetUnits(decimal value)
        {
            this.units = value;
        }
    }
}
