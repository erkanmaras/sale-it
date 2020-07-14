using System;
using System.Linq.Expressions;

namespace SaleIt.Infrastructure.Specification
{
    internal class NotSpecification<T> : INotSpecification<T>
    {
        public ISpecification<T> Spec { get; }

        internal NotSpecification(ISpecification<T> spec)
        {
            Spec = spec ?? throw new ArgumentNullException(nameof(spec));
        }

        public Expression<Func<T, bool>> Expression => Spec.Expression.Not();

        public bool IsSatisfiedBy(T candidate)
        {
            return !Spec.IsSatisfiedBy(candidate);
        }
    }

}
