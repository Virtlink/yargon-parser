using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parser
{
    /// <summary>
    /// Builds DOT directed graphs.
    /// </summary>
    public sealed class DotDigraphBuilder<TNode>
    {
        private int nodeCount = 0;
        private readonly Dictionary<TNode, String> nodes = new Dictionary<TNode, String>();
        private readonly List<Tuple<String, String, String>> edges = new List<Tuple<String, String, String>>();

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(TNode node)
        {
            GetOrCreateNode(node);
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="from">The source node.</param>
        /// <param name="to">The target node.</param>
        /// <param name="label">The label of the edge.</param>
        public void AddEdge(TNode from, TNode to, string label)
        {
            string fromName = GetOrCreateNode(from);
            string toName = GetOrCreateNode(to);
            edges.Add(Tuple.Create(fromName, toName, label));
        }

        /// <summary>
        /// Gets the name of a node. Adds the node if necessary.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The name of the node.</returns>
        private string GetOrCreateNode(TNode node)
        {
            string nodeName;
            if (!this.nodes.TryGetValue(node, out nodeName))
            {
                nodeName = "n" + this.nodeCount;
                this.nodeCount += 1;
                this.nodes.Add(node, nodeName);
            }
            return nodeName;
        }

        /// <summary>
        /// Escapes the specified string.
        /// </summary>
        /// <param name="str">The string to escape.</param>
        /// <returns>The escaped string.</returns>
        private string Escape(string str)
        {
            #region Contract
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            #endregion

            return str.Replace("\"", "\\\"");
        }

        /// <summary>
        /// Returns a string representation of the graph.
        /// </summary>
        /// <returns>The graph's string representation.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("digraph g {");
            foreach (var node in this.nodes)
            {
                sb.AppendLine($"  {node.Value} [label=\"{Escape(node.Key.ToString())}\"];");
            }
            sb.AppendLine();
            foreach (var edge in this.edges)
            {
                sb.Append($"  {edge.Item1} -> {edge.Item2}");
                if (!String.IsNullOrEmpty(edge.Item3))
                {
                    sb.Append($" [label=\"{Escape(edge.Item3)}\"]");
                }
                sb.AppendLine(";");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
