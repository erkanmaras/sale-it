using SaleIt.Domain.Sales.Entities;

namespace SaleIt.Domain.Sales.Services
{
    public interface ISaleDocumentCalculator
    {
        decimal CalcTax(Product product, decimal units);

        decimal CalcAmount(Product product, decimal units, decimal discount, decimal tax);
    }
}