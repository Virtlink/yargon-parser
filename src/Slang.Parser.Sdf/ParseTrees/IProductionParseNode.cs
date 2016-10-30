namespace Slang.Parser.Sdf.ParseTrees
{
	/// <summary>
	/// A production parse node.
	/// </summary>
	public interface IProductionParseNode : IParseNode
	{
		CodePoint Token { get; }
	}
}
