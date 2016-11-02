using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;
using Virtlink.ATerms;

namespace Slang.Parser.Sdf
{
    /// <summary>
    /// Builds parse trees using ATerms.
    /// </summary>
    public sealed class ATermParseTreeBuilder : IParseTreeBuilder<Token<CodePoint>, ITerm>
    {
        public ITerm BuildToken(Token<CodePoint> token)
        {
            throw new NotImplementedException();
        }

        public ITerm BuildProduction(IReduction production, IEnumerable<ITerm> arguments)
        {
            throw new NotImplementedException();
        }

        public ITerm BuildAmbiguity(IEnumerable<ITerm> alternatives)
        {
            throw new NotImplementedException();
        }
    }
}
