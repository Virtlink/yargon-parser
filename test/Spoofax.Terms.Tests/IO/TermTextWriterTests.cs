using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Spoofax.Terms.IO
{
	public abstract class TermTextWriterTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <returns>The instance to test.</returns>
		public abstract TermTextWriter CreateSUT();

		/// <summary>
		/// Creates a reader, used in the test.
		/// </summary>
		/// <returns>The reader.</returns>
		public abstract TermTextReader CreateReader();

		[Test]
		public void CanWriteToTextWriter()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Int(1);
			var writer = new StringWriter();

			// Act
			sut.Write(term, writer);

			// Assert
			Assert.IsNotEmpty(writer.ToString());
		}

		[Test]
		public void CanWriteToString()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Int(1);

			// Act
			var result = sut.ToString(term);

			// Assert
			Assert.IsNotEmpty(result);
		}
	}
}
