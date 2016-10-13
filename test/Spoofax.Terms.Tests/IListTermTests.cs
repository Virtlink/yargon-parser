using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace Spoofax.Terms
{
	public abstract class IListTermTests : TestBase
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <param name="values">The values of the term.</param>
		/// <returns>The instance to test.</returns>
		public abstract IListTerm CreateSUT(IEnumerable<ITerm> values);

		[Test]
		public void HasExpectedPropertyValues()
		{
			// Arrange
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var val2 = Factory.Int(2);
			var values = new ITerm[] { val0, val1, val2 };
			var sut = CreateSUT(values);

			// Assert
			Assert.AreEqual(values, sut.SubTerms);
			Assert.AreEqual(3, sut.Count);
			Assert.IsFalse(sut.IsEmpty);
			Assert.AreEqual(val0, sut.Head);
			Assert.AreEqual(new ITerm[]{ val1, val2 }, sut.Tail.SubTerms);
			Assert.IsEmpty(sut.Annotations);
		}

		[Test]
		public void ToStringReturnsAString()
		{
			// Arrange
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var val2 = Factory.Int(2);
			var values = new ITerm[] { val0, val1, val2 };
			var sut = CreateSUT(values);

			// Act
			var result = sut.ToString();

			// Assert
			Assert.AreEqual("[0,1,2]", result);
		}

		[Test]
		public void TwoEqualTermsAreEqual()
		{
			// Arrange
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var val2 = Factory.Int(2);
			var values = new ITerm[] { val0, val1, val2 };
			var sut = CreateSUT(values);
			var other = CreateSUT(new []{val0,val1, val2});

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void TwoDifferentTermsAreDifferent()
		{
			// Arrange
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var val2 = Factory.Int(2);
			var values = new ITerm[] { val0, val1, val2 };
			var sut = CreateSUT(values);
			var other = CreateSUT(new[] { val0, val2 });

			// Act
			var result = sut.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void CallsVisitorMethod()
		{
			// Arrange
			var val0 = Factory.Int(0);
			var val1 = Factory.Int(1);
			var val2 = Factory.Int(2);
			var values = new ITerm[] { val0, val1, val2 };
			var sut = CreateSUT(values);
			var visitor = new Mock<ITermVisitor>();

			// Act
			sut.Accept(visitor.Object);

			// Assert
			visitor.Verify(v => v.VisitList(sut));
		}
	}
}
