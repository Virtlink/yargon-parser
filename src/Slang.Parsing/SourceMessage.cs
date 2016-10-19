using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// A message in the source.
    /// </summary>
    public class SourceMessage
    {
        /// <summary>
        /// Gets the location of the message.
        /// </summary>
        /// <value>A source range.</value>
        public SourceRange Location { get; }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>The message text.</value>
        public string Message { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMessage"/> class.
        /// </summary>
        /// <param name="location">The source location.</param>
        /// <param name="message">The message text.</param>
        public SourceMessage(SourceRange location, string message)
        {
            #region Contract
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            #endregion

            this.Location = location;
            this.Message = message;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as SourceMessage);

        /// <inheritdoc />
        public bool Equals(SourceMessage other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return this.Location == other.Location
                && this.Message == other.Message;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Location.GetHashCode();
                hash = hash * 29 + this.Message.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SourceMessage"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(SourceMessage left, SourceMessage right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="SourceMessage"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(SourceMessage left, SourceMessage right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"{this.Location.Start}: {this.Message}";
    }
}
