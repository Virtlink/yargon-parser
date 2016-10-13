using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace Spoofax.Terms
{
	public abstract class IConsTermTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <param name="name">The name of the constructor.</param>
		/// <param name="terms">The terms of the constructor.</param>
		/// <returns>The instance to test.</returns>
		public abstract IConsTerm CreateSUT(string name, IEnumerable<ITerm> terms);

		[Test]
		public void HasExpectedPropertyValues()
		{
			// Arrange
			string consName = "Cons";
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var values = new ITerm[] { val0, val1 };
			var sut = CreateSUT(consName, values);

			// Assert
			Assert.AreEqual(consName, sut.Name);
			Assert.AreEqual(values, sut.SubTerms);
			Assert.IsEmpty(sut.Annotations);
		}

		[Test]
		public void ToStringReturnsAString()
		{
			// Arrange
			string consName = "Cons";
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var values = new ITerm[] { val0, val1 };
			var sut = CreateSUT(consName, values);

			// Act
			var result = sut.ToString();

			// Assert
			Assert.AreEqual("Cons(0,1)", result);
		}

		[Test]
		public void TwoEqualTermsAreEqual()
		{
			// Arrange
			string consName = "Cons";
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var values = new ITerm[] { val0, val1 };
			var sut = CreateSUT(consName, values);
			var other = CreateSUT(consName, new[] { val0, val1 });

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void TwoDifferentTermsAreDifferent()
		{
			// Arrange
			string cons0Name = "Cons";
			string cons1Name = "Other";
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var values = new ITerm[] { val0, val1 };
			var sut = CreateSUT(cons0Name, values);
			var other = CreateSUT(cons1Name, new[] { val0 });

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void CallsVisitorMethod()
		{
			// Arrange
			var sut = CreateSUT("Cons", new[] { Factory.Int(0) });
			var visitor = new Mock<ITermVisitor>();
			
			// Act
			sut.Accept(visitor.Object);

			// Assert
			visitor.Verify(v => v.VisitCons(sut));
		}
	}
}
