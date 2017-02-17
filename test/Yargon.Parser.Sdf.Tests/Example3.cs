using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using System.Reflection;
using Yargon.Parser.Sdf.ParseTrees;
using Yargon.Parser.Sdf.Productions.IO;
using Yargon.Parsing;
using Yargon.ATerms;
using Yargon.ATerms.IO;

namespace Yargon.Parser.Sdf
{
	[TestFixture]
	public class Example3
	{
	    private const string Name = "Example3";

        [Test]
		public void ReadParseTableFromFile()
		{
            // Arrange
		    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yargon.Parser.Sdf.{Name}.tbl");
            Debug.Assert(stream != null);
            var termFactory = new TrivialTermFactory();
            var productionFormat = new TermProductionFormat(termFactory);
            var reader = new SdfParseTableReader(productionFormat);

            // Act
            var parseTable = reader.Read(new StreamReader(stream));

            // Assert
            // No exceptions.

            // Cleanup
            stream.Dispose();
		}

        [Test]
	    public void ParseString()
        {
            // Arrange
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yargon.Parser.Sdf.{Name}.tbl");
            var termFactory = new TrivialTermFactory();
            var productionFormat = new TermProductionFormat(termFactory);
            var reader = new SdfParseTableReader(productionFormat);
            var parseTable = reader.Read(new StreamReader(stream));            
            var parseTreeBuilder = new TrivialParseNodeFactory();
            var errorHandler = new FailingErrorHandler<SdfStateRef, Token<CodePoint>>();
            var parser = new YargonParser<SdfStateRef, Token<CodePoint>, IParseNode>(parseTable, parseTreeBuilder, errorHandler);

            // Act
            string input = "keyword";
            var result = parser.Parse(new CharacterProvider(new StringReader(input)));

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Tree.ToString(), Is.EqualTo("appl(\"<START>\" -> CF-layout?CF-\"Start\"CF-layout?; [appl(CF-layout? -> ; []), appl(CF-\"Start\" -> \"\"keyword\"\"; [appl(\"\"keyword\"\" -> [107][101][121][119][111][114][100]; [107, 101, 121, 119, 111, 114, 100])]), appl(CF-layout? -> ; [])])"));

            // Cleanup
            stream.Dispose();
        }

        [Test]
	    public void BuildAsFixTree()
	    {
            // Arrange
	        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yargon.Parser.Sdf.{Name}.tbl");
	        var termFactory = new TrivialTermFactory();
	        var productionFormat = new TermProductionFormat(termFactory);
	        var reader = new SdfParseTableReader(productionFormat);
	        var parseTable = reader.Read(new StreamReader(stream));
	        var parseTreeBuilder = new TrivialParseNodeFactory();
	        var errorHandler = new FailingErrorHandler<SdfStateRef, Token<CodePoint>>();
	        var parser = new YargonParser<SdfStateRef, Token<CodePoint>, IParseNode>(parseTable, parseTreeBuilder, errorHandler);
	        var asfixTreeBuilder = new AsFixTreeBuilder(termFactory, productionFormat);
	        string input = "keyword";
	        var result = parser.Parse(new CharacterProvider(new StringReader(input)));

	        // Act
	        var tree = asfixTreeBuilder.Transform(result.Tree);

            // Assert
            Assert.That(tree.ToString(), Is.EqualTo("parsetree(appl(prod([cf(opt(layout())),cf(lit(\"\\\"Start\\\"\")),cf(opt(layout()))],lit(\"\\\"<START>\\\"\"),no-attrs()),[appl(prod([],cf(opt(layout())),no-attrs()),[]),appl(prod([lit(\"\\\"keyword\\\"\")],cf(lit(\"\\\"Start\\\"\")),no-attrs()),[appl(prod([char-class([107]),char-class([101]),char-class([121]),char-class([119]),char-class([111]),char-class([114]),char-class([100])],lit(\"\\\"keyword\\\"\"),no-attrs()),[107,101,121,119,111,114,100])]),appl(prod([],cf(opt(layout())),no-attrs()),[])]),0)"));

            // Cleanup
	        stream.Dispose();
	    }
    }
}
