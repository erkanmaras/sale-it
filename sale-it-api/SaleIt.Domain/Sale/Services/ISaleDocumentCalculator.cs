using SaleIt.Domain.Sale.Entities;

namespace SaleIt.Domain.Sale.Services
{
    public interface ISaleDocumentCalculator
    {
        decimal CalcTax(Product product, decimal units);

        decimal CalcAmount(Product product, decimal units, decimal discount, decimal tax);
    }
}