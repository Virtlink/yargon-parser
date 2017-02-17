using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yargon.Parser.Sdf.Productions.IO;
using Yargon.ATerms;

namespace Yargon.Parser.Sdf.ParseTrees
{
    /// <summary>
    /// A parse tree imploder.
    /// </summary>
    public sealed class AsFixImploder
    {
        private readonly TermFactory factory;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AsFixImploder"/> class.
        /// </summary>
        /// <param name="factory">The term factory to use.</param>
        public AsFixImploder(TermFactory factory)
        {
            #region Contract
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            #endregion

            this.factory = factory;
        }
        #endregion

        /// <summary>
        /// Implodes the specified parse tree.
        /// </summary>
        /// <param name="asfixTree">The AsFix parse tree to implode.</param>
        /// <returns>The resulting AST.</returns>
        public ITerm Implode(ITerm asfixTree)
        {
            #region Contract
            if (asfixTree == null)
                throw new ArgumentNullException(nameof(asfixTree));
            #endregion
            
            throw new NotImplementedException();
        }
    }
}
