using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parser.Sdf
{
    /// <summary>
	/// Specifies flags that apply to a <see cref="Production"/>.
	/// </summary>
	[Flags]
    public enum ProductionFlags
    {
        /// <summary>
        /// No flags apply.
        /// </summary>
        None = 0,
        Recover = 1 << 0,
        Completion = 1 << 1,
        IgnoreLayout = 1 << 2,
        NewlineEnforced = 1 << 3,
        LongestMatch = 1 << 4,
    }
}
