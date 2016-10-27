using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// An action item base class.
	/// </summary>
    public abstract class ActionItem : IEquatable<ActionItem>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionItem"/> class.
        /// </summary>
        protected ActionItem()
        { /* Nothing to do. */ }
        #endregion
    }
}
