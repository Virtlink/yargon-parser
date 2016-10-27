using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;

namespace Slang.Parser.Sdf
{
    /// <summary>
    /// A sort.
    /// </summary>
    public struct Sort : ISort
    {
        /// <summary>
        /// Gets the name of the sort.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Sort"/> class.
        /// </summary>
        public Sort(string name)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            #endregion

            this.Name = name;
        }
        #endregion

        /// <inheritdoc />
        public override string ToString() => this.Name;
    }
}
