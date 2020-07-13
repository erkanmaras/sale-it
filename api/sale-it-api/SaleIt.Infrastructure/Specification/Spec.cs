using System;
using System.Linq.Expressions;

namespace SaleIt.Infrastructure.Specification
{

    /// <summary>
    /// For a fluent language.
    /// </summary>
    public static class Spec
    {
        /// <summary>
        /// Satisfied by any candidates.
        /// </summary>
        public static Spec<T> Any<T>()
        {
            return Spec<T>.Any;
        }

        /// <summary>
        /// Not satisfied by any candidate.
        /// </summary>
        public static Spec<T> Not<T>()
        {
            return Spec<T>.None;
        }
    }

    /// <summary>
    /// Abstract Specification defined by an Expressions that can be used on IQueryables.
    /// </summary>
    /// <typeparam name="T">The type of the candidate.</typeparam>
    public class Spec<T> : ISpecification<T>
    {
        protected bool Equals(Spec<T> other)
        {
            return expression.Equals(other.expression);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Spec<T>)obj);
        }

        public override int GetHashCode()
        {
            return expression.GetHashCode();
        }

        private readonly Expression<Func<T, bool>> expression;

        /// <summary>
        /// Satisfied by any candidates.
        /// </summary>
        public static readonly Spec<T> Any = new Spec<T>(x => true);

        /// <summary>
        /// Not satisfied by any candidate.
        /// </summary>
        public static readonly Spec<T> None = new Spec<T>(x => false);

        /// <summary>
        /// Caches the compiled Expression so that it doesn't have to compile every time IsSatisfiedBy is
        /// invoked.
        /// </summary>
        private readonly Lazy<Func<T, bool>> compiledExpression;

        public Spec(Expression<Func<T, bool>> expression)
        {
            this.expression = expression;
            compiledExpression = new Lazy<Func<T, bool>>(() => this.expression.Compile());
        }

        public virtual  Expression<Func<T, bool>> Expression => expression;

        public virtual bool IsSatisfiedBy(T candidate)
        {
            return compiledExpression.Value(candidate);
        }

 
        /// <summary>
        /// Composes two specifications with an And operator.
        /// </summary>
        /// <param name="spec1">Specification</param>
        /// <param name="spec2">Specification</param>
        /// <returns>New specification</returns>
        public static And<T> operator &(Spec<T> spec1, Spec<T> spec2)
        {
            return new And<T>(spec1, spec2);
        }

        /// <summary>
        /// Composes two specifications with an Or operator.
        /// </summary>
        /// <param name="spec1">Specification</param>
        /// <param name="spec2">Specification</param>
        /// <returns>New specification</returns>
        public static Or<T> operator |(Spec<T> spec1, Spec<T> spec2)
        {
            return new Or<T>(spec1, spec2);
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is true.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static Spec<T> operator ==(bool value, Spec<T> spec)
        {
            return value ? spec : !spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is true.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static Spec<T> operator ==(Spec<T> spec, bool value)
        {
            return value ? spec : !spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is false.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static Spec<T> operator !=(bool value, Spec<T> spec)
        {
            return value ? !spec : spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is false.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static Spec<T> operator !=(Spec<T> spec, bool value)
        {
            return value ? !spec : spec;
        }

        /// <summary>
        /// Creates a new specification that negates a given specification.
        /// </summary>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static Not<T> operator !(Spec<T> spec)
        {
            return new Not<T>(spec);
        }

        /// <summary>
        /// Allows using ASpec[T] in place of a lambda expression.
        /// </summary>
        /// <param name="spec"></param>
        public static implicit operator Expression<Func<T, bool>>(Spec<T> spec)
        {
            return spec.Expression;
        }

        /// <summary>
        /// Allows using ASpec[T] in place of Func[T, bool].
        /// </summary>
        /// <param name="spec"></param>
        public static implicit operator Func<T, bool>(Spec<T> spec)
        {
            return spec.IsSatisfiedBy;
        }

        /// <summary>
        /// Converts the expression into a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Expression.ToString();
        }

#pragma warning disable 693
        public sealed class And<T> : Spec<T>, IOrSpecification<T>
        {
            public Spec<T> Spec1 { get; }

            public Spec<T> Spec2 { get; }

            ISpecification<T> IOrSpecification<T>.Spec1 => Spec1;

            ISpecification<T> IOrSpecification<T>.Spec2 => Spec1;

            internal And(Spec<T> spec1, Spec<T> spec2):base(spec1.Expression.And(spec2.Expression))
            {
                Spec1 = spec1;
                Spec2 = spec2;
            }

            public new bool IsSatisfiedBy(T candidate)
            {
                return Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate);
            }
        }

        public sealed class Or<T> : Spec<T>, IOrSpecification<T>
        {
            public Spec<T> Spec1 { get; }

            public Spec<T> Spec2 { get; }

            ISpecification<T> IOrSpecification<T>.Spec1 => Spec1;

            ISpecification<T> IOrSpecification<T>.Spec2 => Spec1;

            internal Or(Spec<T> spec1, Spec<T> spec2):base(spec1.Expression.Or(spec2.Expression))
            {
                Spec1 = spec1;
                Spec2 = spec2;
            }

            public bool Is(T candidate)
            {
                return Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
            }
        }

        public sealed class Not<T> : Spec<T>, INotSpecification<T>
        {
            public Spec<T> Inner { get; }

            ISpecification<T> INotSpecification<T>.Inner => Inner;

            internal Not(Spec<T> spec) : base(spec.Expression.Not())
            {
                Inner = spec;
            }

            public bool Is(T candidate)
            {
                return !Inner.IsSatisfiedBy(candidate);
            }
        }

#pragma warning restore 693
    }
}
