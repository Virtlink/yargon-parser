using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;
using Virtlink.Utilib;

namespace Slang.Parser
{
    /// <summary>
    /// Base class for examples.
    /// </summary>
    public abstract class ExampleBase
    {

        public IReadOnlyList<Token<String>> Collect(params Tuple<String, TokenType>[] tuples)
        {
            var location = new SourceLocation();
            var tokens = new List<Token<String>>();
            foreach (var tuple in tuples)
            {
                var str = tuple.Item1;
                var type = tuple.Item2;

                var from = location;
                var to = location.AddString(str);
                location = to;
                var range = new SourceRange(from, to);

                tokens.Add(new Token<string>(str, type, range));
            }
            return tokens;
        }

        public ParseTreeNode Node(Sort symbol, params IParseTree[] children) => new ParseTreeNode(symbol, children);
        public ParseTreeToken<T> Token<T>(TokenType type, T value) => new ParseTreeToken<T>(new Token<T>(value, type, null));

        public IParseTree StripLocations(IParseTree tree)
        {
            var node = tree as ParseTreeNode;
            if (node != null)
            {
                return new ParseTreeNode(node.Symbol, node.Children.Select(StripLocations).ToArray());
            }

            if (tree is ParseTreeToken<String>)
            {
                var token = ((ParseTreeToken<String>)tree).Token;
                return new ParseTreeToken<String>(new Token<String>(token.Value, token.Type, null));
            }

            throw new InvalidOperationException();
        }

        public Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>> Expand(Dictionary<State, Reduction> reductions, IReadOnlyCollection<TokenType> tokens)
        {
            var result = new Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>>();
            foreach (var pair in reductions)
            {
                foreach (var token in tokens)
                {
                    result.Add(Tuple.Create(pair.Key, (ITokenType)token), new[] { pair.Value });
                }
            }
            return result;
        }

        public Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>> Expand(Dictionary<State, IReadOnlyCollection<Reduction>> reductions, IReadOnlyCollection<TokenType> tokens)
        {
            var result = new Dictionary<Tuple<State, ITokenType>, IReadOnlyCollection<IReduction>>();
            foreach (var pair in reductions)
            {
                foreach (var token in tokens)
                {
                    result.Add(Tuple.Create(pair.Key, (ITokenType)token), pair.Value);
                }
            }
            return result;
        }
    }
}
