namespace Yargon.Parser.Sdf.ParseTrees
{
	public interface IApplicationParseNode : IParseNode
	{
		/// <summary>
		/// Gets the label for this parse node.
		/// </summary>
		/// <value>A production.</value>
		/// <remarks>
		/// Interior nodes are labelled with productions for disambiguation.
		/// </remarks>
		Production Production { get; }      // TODO: Rename to Label

		/// <summary>
		/// Gets the parse node preference.
		/// </summary>
		/// <value>A member of the <see cref="ParseNodePreference"/> enumeration.</value>
		ParseNodePreference Preference { get; }
	}
}
