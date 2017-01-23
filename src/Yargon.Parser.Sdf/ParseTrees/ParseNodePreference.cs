namespace Yargon.Parser.Sdf.ParseTrees
{
	// NOTE: Make this a 'preference level', with higher numbers indicating
	// higher preference. Avoid would equal -1, none 0, prefer 1, something like that.
	/// <summary>
	/// Specifies the disambiguation preference of the parse node.
	/// </summary>
	public enum ParseNodePreference
	{
		/// <summary>
		/// No preference.
		/// </summary>
		None,
		/// <summary>
		/// Prefer this node.
		/// </summary>
		Prefer,
		/// <summary>
		/// Avoid this node.
		/// </summary>
		Avoid
	}
}
