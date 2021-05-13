﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerAlgebra
{
    /// <summary>
    /// Represents an unordered collection of elements.
    /// </summary>
    public class FiniteSet : Set
    {
        protected IEnumerable<Expression> members;
        /// <summary>
        /// Elements contained in this set.
        /// </summary>
        public override IEnumerable<Expression> Members { get { return members; } }

        protected FiniteSet(IEnumerable<Expression> Members) { members = Members; }

        public new static FiniteSet New(IEnumerable<Expression> Members) { return new FiniteSet(Members.OrderBy(i => i).Distinct().Buffer()); }

        public override bool Contains(Expression x) { return members.Contains(x); }

        public override bool Matches(Expression Expr, MatchContext Matched)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Atom> Atoms
        {
            get
            {
                foreach (Expression i in members)
                    foreach (Atom j in i.Atoms)
                        yield return j;
            }
        }
        public override int CompareTo(Expression R)
        {
            if (R is FiniteSet RS)
                return members.LexicalCompareTo(RS.Members);
            return base.CompareTo(R);
        }

        public override bool Equals(Expression E)
        {
            if (E is FiniteSet S)
                return Members.SequenceEqual(S.Members);
            return base.Equals(E);
        }
        public override int GetHashCode() { return Members.UnorderedHashCode(); }

        public static FiniteSet Union(FiniteSet A, FiniteSet B) { return FiniteSet.New(A.Members.Union(B.Members)); }
        public static FiniteSet Intersection(FiniteSet A, FiniteSet B) { return FiniteSet.New(A.Members.Intersect(B.Members)); }
    }
}
