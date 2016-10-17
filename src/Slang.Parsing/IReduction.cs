using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// A parser reduction.
    /// </summary>
    public interface IReduction
    {
        /// <summary>
        /// Gets the non-terminal symbol resulting from the reduction.
        /// </summary>
        /// <value>The non-terminal symbol.</value>
        ISort Symbol { get; }

        /// <summary>
        /// Gets the number of symbols consumed by the reduction.
        /// </summary>
        /// <value>The arity.</value>
        int Arity { get; }

        /// <summary>
        /// Gets whether this reduction rejects.
        /// </summary>
        /// <value><see langword="true"/> when this reduction rejects;
        /// otherwise, <see langword="false"/>.</value>
        bool Rejects { get; }
    }
}
