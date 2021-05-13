﻿using System.Collections.Generic;
using System.Linq;
using LinqExpr = System.Linq.Expressions.Expression;
using ParamExpr = System.Linq.Expressions.ParameterExpression;

namespace ComputerAlgebra.LinqCompiler
{
    /// <summary>
    /// Represents a scope in which expressions can be declared and mapped.
    /// </summary>
    public class DeclContext
    {
        private List<ParamExpr> decls = new List<ParamExpr>();
        private Dictionary<Expression, LinqExpr> map = new Dictionary<Expression, LinqExpr>();

        private DeclContext parent = null;
        public DeclContext Parent { get { return parent; } }

        public DeclContext() { }
        public DeclContext(DeclContext Parent) { parent = Parent; }

        public IEnumerable<ParamExpr> Decls { get { return decls; } }

        public void Declare(IEnumerable<ParamExpr> Decls) { decls.AddRange(Decls); }
        public void Declare(Expression Map, LinqExpr To) { map[Map] = To; }

        public virtual LinqExpr LookUp(Expression Expr)
        {
            LinqExpr expr;
            if (map.TryGetValue(Expr, out expr))
                return expr;

            if (parent != null)
                return parent.LookUp(Expr);

            return null;
        }

        public virtual LinqExpr LookUp(string Name)
        {
            ParamExpr expr = decls.SingleOrDefault(i => i.Name == Name);
            if (expr != null)
                return expr;

            if (parent != null)
                return parent.LookUp(Name);

            return null;
        }
    }
}
