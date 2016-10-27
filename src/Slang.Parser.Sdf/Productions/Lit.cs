using System;
using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	public sealed class Lit : ISort
    {
		public string Text
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Lit"/> class.
		/// </summary>
		public Lit(string text)
        {
            #region Contract
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            #endregion

            this.Text = text;
		}
		#endregion

		/// <inheritdoc />
		public override string ToString()
		{
			return $"\"{this.Text}\"";
		}
	}
}
