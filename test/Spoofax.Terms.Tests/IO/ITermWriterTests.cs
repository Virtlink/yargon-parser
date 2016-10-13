using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Spoofax.Terms;
using Spoofax.Terms.IO;

namespace Spoofax.Terms.IO
{
	public abstract class ITermWriterTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <returns>The instance to test.</returns>
		public abstract ITermWriter CreateSUT();

		/// <summary>
		/// Creates a reader, used in the test.
		/// </summary>
		/// <returns>The reader.</returns>
		public abstract ITermReader CreateReader();

		[Test]
		public void CanWriteToStream()
		{
			// Arrange
			var sut = CreateSUT();
			var term = Factory.Int(1);
			var output = new MemoryStream();

			// Act
			sut.Write(term, output);

			// Assert
			Assert.GreaterOrEqual(1, output.Length);
			Assert.IsTrue(output.CanWrite);
		}
	}
}
