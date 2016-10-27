using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// 
	/// </summary>
	public sealed class AcceptActionItem : ActionItem
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptActionItem"/> class.
        /// </summary>
        public AcceptActionItem()
        { }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return "accept";
        }
    }
}
