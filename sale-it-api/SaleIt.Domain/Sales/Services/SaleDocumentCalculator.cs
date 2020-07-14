using SaleIt.Domain.Sales.Entities;

namespace SaleIt.Domain.Sales.Services
{
    public class SaleDocumentCalculator : ISaleDocumentCalculator
    {
        public decimal CalcTax(Product product, decimal units)
        {
            return product.Price * product.TaxRate * units;
        }

        public decimal CalcAmount(Product product, decimal units, decimal discount, decimal tax)
        {
            return (product.Price * units) - discount + tax;
        }
    }
}
