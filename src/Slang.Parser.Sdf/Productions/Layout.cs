using Slang.Parsing;

namespace Slang.Parser.Sdf.Productions
{
	/// <summary>
	/// The special layout sort.
	/// </summary>
	public sealed class Layout : ISort
    {
		/// <summary>
		/// The default instance.
		/// </summary>
		public static Layout Instance { get; } = new Layout();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Layout"/> class.
		/// </summary>
		private Layout()
		{
			// Nothing to do.
		}
		#endregion

		public override string ToString()
		{
			return "layout";
		}
	}
}
