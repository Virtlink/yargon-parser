using System;
using JetBrains.Annotations;
using Yargon.ATerms;
using Yargon.Tego;

namespace Yargon.Parser.Sdf.AsFixImploder
{
    /// <summary>
    /// Implosion of AsFix to an AST.
    /// </summary>
    public static class Implode
    {
        // https://github.com/metaborg/strategoxt/tree/master/strategoxt/stratego-libraries/sglr/lib/stratego/asfix
        public static ITerm ImplodeAsfix(this ITerm term)
            => term.ImplodeAsfix(true);

        public static ITerm ImplodeAsfixSkipConcreteSyntax(this ITerm term)
            => term.ImplodeAsfix(false);

        public static ITerm ImplodeAsfix(this ITerm term, bool implodeConcrete)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixApplToSort(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixFlatLex(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixRemoveLayout(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixRemoveLit(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixFlatAlt(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixReplaceAppl(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixFlatInjections(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixFlatList(this ITerm term)
        {
            throw new NotImplementedException();
        }

        public static ITerm AsfixRemoveSeq(this ITerm term)
        {
            throw new NotImplementedException();
        }
    }
}
