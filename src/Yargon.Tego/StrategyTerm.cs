using System;
using System.Collections.Generic;
using System.Text;
using Yargon.ATerms;

namespace Yargon.Tego
{
    /// <summary>
    /// A term that wraps a strategy.
    /// </summary>
    public sealed class StrategyTerm : IStrategyTerm
    {
        private readonly Func<ITerm, ITerm> strategy;

        /// <inheritdoc />
        public IReadOnlyList<ITerm> SubTerms { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<ITerm> Annotations { get; }

        /// <inheritdoc />
        public ITerm this[int index] => throw new ArgumentOutOfRangeException();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyTerm"/> class.
        /// </summary>
        /// <param name="strategy">The strategy to wrap.</param>
        public StrategyTerm(Func<ITerm, ITerm> strategy)
        {
            #region Contract
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));
            #endregion

            this.strategy = strategy;
        }
        #endregion

        /// <inheritdoc />
        public ITerm Apply(ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            return this.strategy(term);
        }

        /// <inheritdoc />
        public void Accept(ITermVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
