using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Slang.Parsing;

namespace Slang.Parser
{
    /// <summary>
    /// An example that has an error in the input.
    /// </summary>
    [TestFixture]
    public sealed class Example4 : ExampleBase
    {
        [Test]
        public void Test()
        {
            // Arrange
            // Sorts
            var S = new Sort("S");
            var E = new Sort("E");
            // Token types
            var pls = new TokenType("+");
            var exp = new TokenType("^");
            var ddd = new TokenType("d");
            var eof = new TokenType("$");
            var tokens = new[] {pls, ddd, eof};
            // Reductions
            var r0 = new Reduction(S, 2);   // S → E $
            var r1 = new Reduction(E, 3);   // E → E + E
            var r2 = new Reduction(E, 1);   // T → d
            // States
            var s0 = new State("s0");
            var s1 = new State("s1");
            var s2 = new State("s2");
            var s3 = new State("s3");
            var s4 = new State("s4");
            var s5 = new State("s5");
            var s6 = new State("s6");

            // Parse table
            var startState = s0;
            var gotos = new Dictionary<Tuple<State, ISymbol>, State>()
            {
                [Tuple.Create(s0, (ISymbol)ddd)] = s2,
                [Tuple.Create(s0, (ISymbol)S)] = s6,
                [Tuple.Create(s0, (ISymbol)E)] = s1,

                [Tuple.Create(s1, (ISymbol)pls)] = s4,
                [Tuple.Create(s1, (ISymbol)eof)] = s3,
                
                [Tuple.Create(s4, (ISymbol)ddd)] = s2,
                [Tuple.Create(s4, (ISymbol)E)] = s5,

                [Tuple.Create(s5, (ISymbol)pls)] = s4,
            };
            var reductions = new Dictionary<State, Reduction>()
            {
                [s2] = r2,
                [s3] = r0,
                [s5] = r1,
            };
            var parseTable = new ParseTable(startState, gotos, Expand(reductions, tokens));
            var parseTreeBuilder = new ParseTreeBuilder();
            var parser = new SlangParser<State, String, IParseTree>(parseTable, parseTreeBuilder, new FailingErrorHandler<State, String>());

            // Input: 1+(2^3)$
            var input = Collect(new[]
            {
                Tuple.Create("1", ddd),
                Tuple.Create("+", pls),
                Tuple.Create("2", ddd),
                Tuple.Create("^", exp),
                Tuple.Create("3", ddd),
                Tuple.Create("$", eof),
            });

            // Act
            var result = parser.Parse(TokenProvider.From(input));

            // Assert
            Assert.That(result.Success, Is.False);
        }
    }
}
