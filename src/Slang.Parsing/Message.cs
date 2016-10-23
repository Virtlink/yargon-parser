using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Parsing
{
    /// <summary>
    /// A message about a source file.
    /// </summary>
    public class Message : IMessage
    {
        /// <inheritdoc />
        public MessageKind Kind { get; }

        /// <inheritdoc />
        public SourceRange? Location { get; }

        /// <inheritdoc />
        public string Text { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="kind">The message kind.</param>
        /// <param name="text">The message text.</param>
        /// <param name="location">The source location; or <see langword="null"/>.</param>
        public Message(MessageKind kind, string text, SourceRange? location)
        {
            #region Contract
            if (!Enum.IsDefined(typeof(MessageKind), kind))
                throw new InvalidEnumArgumentException(nameof(kind), (int)kind, typeof(MessageKind));
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            #endregion

            this.Kind = kind;
            this.Text = text;
            this.Location = location;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Message);

        /// <inheritdoc />
        public bool Equals(Message other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return this.Kind == other.Kind
                && this.Text == other.Text
                && this.Location == other.Location;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Kind.GetHashCode();
                hash = hash * 29 + this.Text.GetHashCode();
                hash = hash * 29 + this.Location.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Message"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Message left, Message right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Message"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Message left, Message right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (this.Location != null)
            {
                sb.Append(((SourceRange) this.Location).Start);
                sb.Append(": ");
            }
            sb.Append(this.Kind.ToString().ToLowerInvariant());
            sb.Append(": ");
            sb.Append(this.Text);

            return sb.ToString();
        }
    }
}
