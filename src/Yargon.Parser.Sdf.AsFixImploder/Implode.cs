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
        public static ITerm ImplodeAsfix(this ITerm term) => ImplodeAsfix(term, true);

        public static ITerm ImplodeAsfixSkipConcreteSyntax(this ITerm term) => ImplodeAsfix(term, false);

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
