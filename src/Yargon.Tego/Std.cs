using System;
using System.Collections.Generic;
using System.Text;
using Yargon.ATerms;

namespace Yargon.Tego
{
    public static class Std
    {
        // Strategy combinators

        // Not needed, use `??`
        public static ITerm LeftChoice(this ITerm term, Func<ITerm, ITerm> s1, Func<ITerm, ITerm> s2)
            // = s1 <+ s2
            => term.Apply(s1) ?? term.Apply(s2);

        // Not needed, use `??`
        public static ITerm NonDeterministicChoice(this ITerm term, Func<ITerm, ITerm> s1, Func<ITerm, ITerm> s2)
            // = s1 + s2
            => term.LeftChoice(s1, s2);

        // Not needed, use `?.`
        public static ITerm Seq(this ITerm term, Func<ITerm, ITerm> s1, Func<ITerm, ITerm> s2)
            // = s1 ; s2
            => term.Apply(s1)?.Apply(s2);

        // Not needed, use `null`.
        public static ITerm Fail(this ITerm term)
            // = fail
            => null;

        // Not needed, use `term`.
        public static ITerm Id(this ITerm term)
            // = id
            => term;

        public static ITerm GuardedCoice(this ITerm term, Func<ITerm, ITerm> s1, Func<ITerm, ITerm> s2, Func<ITerm, ITerm> s3)
        {
            // = s1 < s2 + s3
            var t = term.Apply(s1);
            return t != null ? t.Apply(s2) : term.Apply(s3);
        }


        // Special built-in

        public static ITerm All(this ITerm term, Func<ITerm, ITerm> s)
            => throw new NotImplementedException();

        public static ITerm Some(this ITerm term, Func<ITerm, ITerm> s)
            => throw new NotImplementedException();

        public static ITerm One(this ITerm term, Func<ITerm, ITerm> s)
            => throw new NotImplementedException();

        public static ITerm Match(this ITerm term, ITerm pattern)
            => throw new NotImplementedException();

        public static ITerm Build(this ITerm term, ITerm pattern)
            => throw new NotImplementedException();

        // Traversal

        public static ITerm Topdown(this ITerm term, Func<ITerm, ITerm> s)
            // = s; all(topdown(s))
            => term.Apply(s)?.All(t1 => t1.Topdown(s));
        
        public static ITerm Bottomup(this ITerm term, Func<ITerm, ITerm> s)
            // = all(bottomup(s)); s
            => term.All(t1 => t1.Bottomup(s))?.Apply(s);

        public static ITerm Alltd(this ITerm term, Func<ITerm, ITerm> s)
            // = s <+ all(alltd(s))
            => term.Apply(s) ?? term.All(t1 => t1.Alltd(s));

        public static ITerm Oncetd(this ITerm term, Func<ITerm, ITerm> s)
            // = s <+ one(oncetd(s))
            => term.Apply(s) ?? term.One(t1 => t1.Oncetd(s));
        
        public static ITerm Innermost(this ITerm term, Func<ITerm, ITerm> s)
            // = bottomup(try(s; innermost(s)))
            => term.Bottomup(t1 => t1.Try(t2 => t2.Apply(s)?.Innermost(s)));

        public static ITerm Try(this ITerm term, Func<ITerm, ITerm> s)
            // = s <+ id
            => term.Apply(s) ?? term;

        public static ITerm Repeat(this ITerm term, Func<ITerm, ITerm> s)
            // = try(s; repeat(s))
            => term.Try(t => t.Apply(s)?.Repeat(s));

        public static ITerm Not(this ITerm term, Func<ITerm, ITerm> s)
            // = s < fail + id
            => term.GuardedCoice(t1 => t1.Apply(s), Fail, Id);

        public static ITerm Apply(this ITerm term, Func<ITerm, ITerm> s)
            // = s
            => s(term);

        public static ITerm Where(this ITerm term, Func<ITerm, ITerm> s)
            // = where(s)
            => term.Apply(s) != null ? term : null;
        
        public static ITerm If(this ITerm term, Func<ITerm, ITerm> s1, Func<ITerm, ITerm> s2, Func<ITerm, ITerm> s3)
            // = where(s1) < s2 + s3
            => term.GuardedCoice(t1 => t1.Where(s1), s2, s3);

        public static ITerm Or(this ITerm term, Func<ITerm, ITerm> s1, Func<ITerm, ITerm> s2)
            // = if s1 then try(where(s2)) else where(s2) end
            => term.If(s1, t1 => t1.Try(t2 => t2.Where(s2)), t3 => t3.Where(s2));

        public static ITerm And(this ITerm term, Func<ITerm, ITerm> s1, Func<ITerm, ITerm> s2)
            // = if s1 then where(s2) else where(s2); fail end
            => term.If(s1, t1 => t1.Where(s2), t3 => t3.Where(s2)?.Fail());

        public static ITerm Map(this ITerm term, Func<ITerm, ITerm> s)
        {
            // = [] + [s | map(s)]
            throw new NotImplementedException();
        }
    }
}
