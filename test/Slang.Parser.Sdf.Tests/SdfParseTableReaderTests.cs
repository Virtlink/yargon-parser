using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using System.Reflection;
using Virtlink.ATerms;
using Virtlink.ATerms.IO;

namespace Slang.Parser.Sdf
{
	[TestFixture]
	public class SdfParseTableReaderTests
    {
		[Test]
		public void ReadParseTableFromFile()
		{
            // Arrange
		    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Slang.Parser.Sdf.Parsetable1.tbl");
            Debug.Assert(stream != null);
		    var reader = new SdfParseTableReader();

            
            // Act
            var parseTable = reader.Read(new StreamReader(stream));

            // Assert

            // Cleanup
            stream.Dispose();
		}
	}
}
