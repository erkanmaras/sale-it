﻿using System;
using System.Linq.Expressions;

namespace SaleIt.Infrastructure.Specification
{
    internal class OrSpecification<TEntity> : IOrSpecification<TEntity>
    {
        public ISpecification<TEntity> Spec1 { get; }

        public ISpecification<TEntity> Spec2 { get; }

        internal OrSpecification(ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            Spec1 = spec1 ?? throw new ArgumentNullException(nameof(spec1));
            Spec2 = spec2 ?? throw new ArgumentNullException(nameof(spec2));
        }

        public Expression<Func<TEntity, bool>> Expression => Spec1.Expression.Or(Spec2.Expression);

        public bool IsSatisfiedBy(TEntity candidate)
        {
            return Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
        }
    }
}
