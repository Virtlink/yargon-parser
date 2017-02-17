using System;
using System.Collections.Generic;
using System.Text;
using Yargon.ATerms;

namespace Yargon.Tego
{
    /// <summary>
    /// An application of a strategy.
    /// </summary>
    public interface IStrategyTerm : ITerm
    {
        /// <summary>
        /// Applies the strategy to the term.
        /// </summary>
        /// <param name="term">The input term.</param>
        /// <returns>The output term.</returns>
        ITerm Apply(ITerm term);
    }
}
