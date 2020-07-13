using SaleIt.Domain.Core;
using System;
using System.Collections.Generic;

namespace SaleIt.Domain.Shared.ValueObjects
{
    class Money : ValueObject
    {
        public string Currency { get; }
        public decimal Amount { get; }

        public Money(string currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEquitableValues()
        {
            yield return Currency.ToUpper();
            yield return Math.Round(Amount, 2);
        }
    }
}
