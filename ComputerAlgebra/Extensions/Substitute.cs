﻿using System.Collections.Generic;
using System.Linq;

namespace ComputerAlgebra
{
    /// <summary>
    /// Expression visitor for substiting expressions in for other expressions.
    /// </summary>
    class SubstituteVisitor : RecursiveExpressionVisitor
    {
        protected IDictionary<Expression, Expression> x0;
        protected bool transform;

        public SubstituteVisitor(IDictionary<Expression, Expression> x0, bool IsTransform) { this.x0 = x0; transform = IsTransform; }

        public override Expression Visit(Expression E)
        {
            if (x0.TryGetValue(E, out Expression xE))
                return xE;
            return base.Visit(E);
        }

        protected override Expression VisitCall(Call F)
        {
            return F.Target.Substitute(F, x0, transform);
        }
    }

    public static class SubstituteExtension
    {
        /// <summary>
        /// Substitute variables x0 into f.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x0"></param>
        /// <returns></returns>
        public static Expression Substitute(this Expression f, IDictionary<Expression, Expression> x0, bool IsTransform = false)
        {
            if (x0.Empty())
                return f;
            return new SubstituteVisitor(x0, IsTransform).Visit(f);
        }

        /// <summary>
        /// Evaluate an expression at x = x0.
        /// </summary>
        /// <param name="f">Expression to evaluate.</param>
        /// <param name="x">Arrow expressions representing substitutions to evaluate.</param>
        /// <returns>The evaluated expression.</returns>
        public static Expression Substitute(this Expression f, IEnumerable<Arrow> x) { return f.Substitute(x.ToDictionary(i => i.Left, i => i.Right)); }
        public static Expression Substitute(this Expression f, params Arrow[] x) { return f.Substitute(x.AsEnumerable()); }

        /// <summary>
        /// Evaluate an expression at x = x0.
        /// </summary>
        /// <param name="f">Expression to evaluate.</param>
        /// <param name="x">Variable to evaluate at.</param>
        /// <param name="x0">Value to evaluate for.</param>
        /// <returns>The evaluated expression.</returns>
        public static Expression Substitute(this Expression f, Expression x, Expression x0) { return f.Substitute(new Dictionary<Expression, Expression> { { x, x0 } }); }

        /// <summary>
        /// Substitute variables x0 into f.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x0"></param>
        /// <returns></returns>
        public static IEnumerable<Expression> Substitute(this IEnumerable<Expression> f, IDictionary<Expression, Expression> x0, bool IsTransform = false)
        {
            if (x0.Empty())
                return f;
            SubstituteVisitor V = new SubstituteVisitor(x0, IsTransform);
            return f.Select(i => V.Visit(i));
        }

        /// <summary>
        /// Evaluate an expression at x = x0.
        /// </summary>
        /// <param name="f">Expression to evaluate.</param>
        /// <param name="x">Arrow expressions representing substitutions to evaluate.</param>
        /// <returns>The evaluated expression.</returns>
        public static IEnumerable<Expression> Substitute(this IEnumerable<Expression> f, IEnumerable<Arrow> x) { return f.Substitute(x.ToDictionary(i => i.Left, i => i.Right)); }
        public static IEnumerable<Expression> Substitute(this IEnumerable<Expression> f, params Arrow[] x) { return f.Substitute(x.AsEnumerable()); }

        /// <summary>
        /// Evaluate an expression at x = x0.
        /// </summary>
        /// <param name="f">Expression to evaluate.</param>
        /// <param name="x">Variable to evaluate at.</param>
        /// <param name="x0">Value to evaluate for.</param>
        /// <returns>The evaluated expression.</returns>
        public static IEnumerable<Expression> Substitute(this IEnumerable<Expression> f, Expression x, Expression x0) { return f.Substitute(new Dictionary<Expression, Expression> { { x, x0 } }); }
    }
}
