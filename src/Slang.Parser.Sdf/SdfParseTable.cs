using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser.Sdf
{
    /// <summary>
    /// An SDF parse table.
    /// </summary>
    public class SdfParseTable : IParseTable<int>
    {
        /// <inheritdoc />
        public int StartState { get; }
        
        /// <inheritdoc />
        public bool TryGetShift(int state, ITokenType token, out int nextState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TryGetGoto(int state, ISort label, out int nextState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<IReduction> GetReductions(int state, ITokenType lookahead)
        {
            throw new NotImplementedException();
        }
    }
}
