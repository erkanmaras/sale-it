using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaleIt.Domain.Sale.Entities;

namespace SaleIt.Domain.Sale.Services
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
