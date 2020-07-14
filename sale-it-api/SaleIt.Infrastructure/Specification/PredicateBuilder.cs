using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace SaleIt.Infrastructure.Specification
{
    /// <summary>
    /// Enables the efficient, dynamic composition of query predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
                                                       Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
                                                      Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            UnaryExpression negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
                                                Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            Dictionary<ParameterExpression, ParameterExpression> map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            Expression secondBody = ExpressionParameterVisitor.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
    }

    //http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    public class ExpressionParameterVisitor : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ExpressionParameterVisitor(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ExpressionParameterVisitor(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression parameterExp)
        {
            if (map.TryGetValue(parameterExp, out var replacement))
            {
                parameterExp = replacement;
            }
            return base.VisitParameter(parameterExp);
        }
    }

    //http://blogs.msdn.com/b/mattwar/archive/2007/07/31/linq-building-an-iqueryable-provider-part-ii.aspx
    public abstract class ExpressionVisitor
    {
        protected virtual Expression Visit(Expression? exp)
        {
            if (exp == null)
            {
                //return null ???
                return Expression.Empty();
            }

            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return this.VisitBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New:
                    return this.VisitNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)exp);
                default:
                    throw new Exception($"Unhandled expression type: '{exp.NodeType}'");
            }
        }

        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            return binding.BindingType switch
            {
                MemberBindingType.Assignment => this.VisitMemberAssignment((MemberAssignment)binding),
                MemberBindingType.MemberBinding => this.VisitMemberMemberBinding((MemberMemberBinding)binding),
                MemberBindingType.ListBinding => this.VisitMemberListBinding((MemberListBinding)binding),
                _ => throw new Exception($"Unhandled binding type '{binding.BindingType}'")
            };
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
            if (arguments != initializer.Arguments)
            {
                return Expression.ElementInit(initializer.AddMethod, arguments);
            }
            return initializer;
        }

        protected virtual Expression VisitUnary(UnaryExpression unaryExp)
        {
            Expression operand = this.Visit(unaryExp.Operand);
            if (operand != unaryExp.Operand)
            {
                return Expression.MakeUnary(unaryExp.NodeType, operand, unaryExp.Type, unaryExp.Method);
            }
            return unaryExp;
        }

        protected virtual Expression VisitBinary(BinaryExpression binaryExp)
        {
            var left = this.Visit(binaryExp.Left);
            var right = this.Visit(binaryExp.Right);
            var conversion = this.Visit(binaryExp.Conversion);
            if (left != binaryExp.Left || right != binaryExp.Right || conversion != binaryExp.Conversion)
            {
                if (binaryExp.NodeType == ExpressionType.Coalesce && binaryExp.Conversion != null)
                {
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                }

                return Expression.MakeBinary(binaryExp.NodeType, left, right, binaryExp.IsLiftedToNull, binaryExp.Method);
            }
            return binaryExp;
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression typeBinaryExp)
        {
            var visitedExpression = this.Visit(typeBinaryExp.Expression);
            if (visitedExpression != typeBinaryExp.Expression)
            {
                return Expression.TypeIs(visitedExpression, typeBinaryExp.TypeOperand);
            }
            return typeBinaryExp;
        }

        protected virtual Expression VisitConstant(ConstantExpression constantExp)
        {
            return constantExp;
        }

        protected virtual Expression VisitConditional(ConditionalExpression conditionalExp)
        {
            var test = this.Visit(conditionalExp.Test);
            var ifTrue = this.Visit(conditionalExp.IfTrue);
            var ifFalse = this.Visit(conditionalExp.IfFalse);
            if (test != conditionalExp.Test || ifTrue != conditionalExp.IfTrue || ifFalse != conditionalExp.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }
            return conditionalExp;
        }

        protected virtual Expression VisitParameter(ParameterExpression parameterExp)
        {
            return parameterExp;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression memberExpr )
        {
            var visitedExpression = this.Visit(memberExpr.Expression);
            if (visitedExpression != memberExpr.Expression)
            {
                return Expression.MakeMemberAccess(visitedExpression, memberExpr.Member);
            }
            return memberExpr;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression methodCallExp)
        {
            var obj = this.Visit(methodCallExp.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(methodCallExp.Arguments);
            if (obj != methodCallExp.Object || !ReferenceEquals(args, methodCallExp.Arguments))
            {
                return Expression.Call(obj, methodCallExp.Method, args);
            }
            return methodCallExp;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> expressions)
        {
            List<Expression> list = null!;
            for (int i = 0, n = expressions.Count; i < n; i++)
            {
                var p = this.Visit(expressions[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != expressions[i])
                {
                    list = new List<Expression>(n);
                    for (var j = 0; j < i; j++)
                    {
                        list.Add(expressions[j]);
                    }
                    list.Add(p);
                }
            }
            if (list != null)
            {
                return new ReadOnlyCollection<Expression>(list);
            }
            return expressions;
        }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            var e = this.Visit(assignment.Expression);
            if (e != assignment.Expression)
            {
                return Expression.Bind(assignment.Member, e);
            }
            return assignment;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            var bindings = this.VisitBindingList(binding.Bindings);
            if (!ReferenceEquals(bindings, binding.Bindings))
            {
                return Expression.MemberBind(binding.Member, bindings);
            }
            return binding;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            var visitedInitializerList = this.VisitElementInitializerList(binding.Initializers);
            if (!ReferenceEquals(visitedInitializerList, binding.Initializers))
            {
                return Expression.ListBind(binding.Member, visitedInitializerList);
            }
            return binding;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> bindings)
        {
            List<MemberBinding> list = null!;
            for (int i = 0, n = bindings.Count; i < n; i++)
            {
                var b = this.VisitBinding(bindings[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != bindings[i])
                {
                    list = new List<MemberBinding>(n);
                    for (var j = 0; j < i; j++)
                    {
                        list.Add(bindings[j]);
                    }
                    list.Add(b);
                }
            }
            if (list != null)
            {
                return list;
            }

            return bindings;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> elementInits)
        {
            List<ElementInit> list = null!;
            for (int i = 0, n = elementInits.Count; i < n; i++)
            {
                var init = this.VisitElementInitializer(elementInits[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != elementInits[i])
                {
                    list = new List<ElementInit>(n);
                    for (var j = 0; j < i; j++)
                    {
                        list.Add(elementInits[j]);
                    }
                    list.Add(init);
                }
            }
            if (list != null)
            {
                return list;
            }

            return elementInits;
        }

        protected virtual Expression VisitLambda(LambdaExpression lambdaExp)
        {
            var body = this.Visit(lambdaExp.Body);
            if (body != lambdaExp.Body)
            {
                return Expression.Lambda(lambdaExp.Type, body, lambdaExp.Parameters);
            }
            return lambdaExp;
        }

        protected virtual NewExpression VisitNew(NewExpression newExp)
        {
            IEnumerable<Expression> visitedArguments = this.VisitExpressionList(newExp.Arguments);
            if (!ReferenceEquals(visitedArguments, newExp.Arguments))
            {
                if (newExp.Members != null)
                {
                    return Expression.New(newExp.Constructor!, visitedArguments, newExp.Members);
                }

                return Expression.New(newExp.Constructor!, visitedArguments);
            }
            return newExp;
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression memberInitExp)
        {
            var visitedExpression = this.VisitNew(memberInitExp.NewExpression);
            var bindings = this.VisitBindingList(memberInitExp.Bindings);
            if (visitedExpression != memberInitExp.NewExpression || !ReferenceEquals(bindings, memberInitExp.Bindings))
            {
                return Expression.MemberInit(visitedExpression, bindings);
            }
            return memberInitExp;
        }

        protected virtual Expression VisitListInit(ListInitExpression listInitExp)
        {
            var visitedExpression = this.VisitNew(listInitExp.NewExpression);
            var visitedInitializerList = this.VisitElementInitializerList(listInitExp.Initializers);
            if (visitedExpression != listInitExp.NewExpression || !ReferenceEquals(visitedInitializerList, listInitExp.Initializers))
            {
                return Expression.ListInit(visitedExpression, visitedInitializerList);
            }
            return listInitExp;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression newArrayExp)
        {
            IEnumerable<Expression> visitedExpressionList = this.VisitExpressionList(newArrayExp.Expressions);
            if (!ReferenceEquals(visitedExpressionList, newArrayExp.Expressions))
            {
                if (newArrayExp.NodeType == ExpressionType.NewArrayInit)
                {
                    return Expression.NewArrayInit(newArrayExp.Type.GetElementType()!, visitedExpressionList);
                }

                return Expression.NewArrayBounds(newArrayExp.Type.GetElementType()!, visitedExpressionList);
            }
            return newArrayExp;
        }

        protected virtual Expression VisitInvocation(InvocationExpression invocationExp)
        {
            IEnumerable<Expression> visitedExpressionList = this.VisitExpressionList(invocationExp.Arguments);
            var expr = this.Visit(invocationExp.Expression);
            if (!ReferenceEquals(visitedExpressionList, invocationExp.Arguments) || expr != invocationExp.Expression)
            {
                return Expression.Invoke(expr, visitedExpressionList);
            }
            return invocationExp;
        }
    }
}
