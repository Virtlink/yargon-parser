using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Spoofax.Terms
{
	public abstract class TermFactoryTests
	{
		/// <summary>
		/// Creates the Subject Under Test.
		/// </summary>
		/// <returns>The instance to test.</returns>
		public abstract TermFactory CreateSUT();

		[Test]
		public void CanBuildIntTerm()
		{
			// Arrange
			var sut = CreateSUT();
			const int value = 3;

			// Act
			var result = sut.Int(value);

			// Assert
			Assert.AreEqual(value, result.Value);
			Assert.IsEmpty(result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildIntTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var annos = BuildAnnos(sut);
			const int value = 3;

			// Act
			var result = sut.Int(value, annos);

			// Assert
			Assert.AreEqual(value, result.Value);
			Assert.AreEqual(annos, result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildRealTerm()
		{
			// Arrange
			var sut = CreateSUT();
			const float value = 4.2f;

			// Act
			var result = sut.Real(value);

			// Assert
			Assert.AreEqual(value, result.Value);
			Assert.IsEmpty(result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildRealTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var annos = BuildAnnos(sut);
			const float value = 4.2f;

			// Act
			var result = sut.Real(value, annos);

			// Assert
			Assert.AreEqual(value, result.Value);
			Assert.AreEqual(annos, result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildStringTerm()
		{
			// Arrange
			var sut = CreateSUT();
			const string value = "Test";

			// Act
			var result = sut.String(value);

			// Assert
			Assert.AreEqual(value, result.Value);
			Assert.IsEmpty(result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildStringTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var annos = BuildAnnos(sut);
			const string value = "Test";

			// Act
			var result = sut.String(value, annos);

			// Assert
			Assert.AreEqual(value, result.Value);
			Assert.AreEqual(annos, result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildListTerm()
		{
			// Arrange
			var sut = CreateSUT();
			var elem0 = sut.Int(0);
			var elem1 = sut.Int(1);
			var elem2 = sut.Int(2);

			// Act
			var result = sut.List(elem0, elem1, elem2);

			// Assert
			Assert.AreEqual(new ITerm[] { elem0, elem1, elem2 }, result.SubTerms);
			Assert.IsEmpty(result.Annotations);
			Assert.AreEqual(3, result.Count);
			Assert.IsFalse(result.IsEmpty);
			Assert.AreEqual(elem0, result.Head);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildListTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var elem0 = sut.Int(0);
			var elem1 = sut.Int(1);
			var elem2 = sut.Int(2);
			var annos = BuildAnnos(sut);

			// Act
			var result = sut.List(new ITerm[]{elem0, elem1, elem2}, annos);

			// Assert
			Assert.AreEqual(new ITerm[] { elem0, elem1, elem2 }, result.SubTerms);
			Assert.AreEqual(annos, result.Annotations);
			Assert.AreEqual(3, result.Count);
			Assert.IsFalse(result.IsEmpty);
			Assert.AreEqual(elem0, result.Head);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildEmptyListTerm()
		{
			// Arrange
			var sut = CreateSUT();

			// Act
			var result = sut.List();

			// Assert
			Assert.IsEmpty(result.Annotations);
			Assert.AreEqual(0, result.Count);
			Assert.IsTrue(result.IsEmpty);
			Assert.IsNull(result.Head);
			Assert.IsNull(result.Tail);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildEmptyListTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var annos = BuildAnnos(sut);

			// Act
			var result = sut.List(TermFactory.EmptyTermList, annos);

			// Assert
			Assert.AreEqual(annos, result.Annotations);
			Assert.AreEqual(0, result.Count);
			Assert.IsTrue(result.IsEmpty);
			Assert.IsNull(result.Head);
			Assert.IsNull(result.Tail);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildConsTerm()
		{
			// Arrange
			var sut = CreateSUT();
			string consName = "Cons";
			var arg0 = sut.Int(0);

			// Act
			var result = sut.Cons(consName, new ITerm[] { arg0 });

			// Assert
			Assert.AreEqual(consName, result.Name);
			Assert.AreEqual(new ITerm[] { arg0 }, result.SubTerms);
			Assert.IsEmpty(result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildConsTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var annos = BuildAnnos(sut);
			string consName = "Cons";
			var arg0 = sut.Int(0);

			// Act
			var result = sut.Cons(consName, new ITerm[] { arg0 }, annos);

			// Assert
			Assert.AreEqual(consName, result.Name);
			Assert.AreEqual(new ITerm[] { arg0 }, result.SubTerms);
			Assert.AreEqual(annos, result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildTupleTerm()
		{
			// Arrange
			var sut = CreateSUT();
			var arg0 = sut.Int(0);

			// Act
			var result = sut.Tuple(new ITerm[] { arg0 });

			// Assert
			Assert.AreEqual(String.Empty, result.Name);
			Assert.AreEqual(new ITerm[] { arg0 }, result.SubTerms);
			Assert.IsEmpty(result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildTupleTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var annos = BuildAnnos(sut);
			var arg0 = sut.Int(0);

			// Act
			var result = sut.Tuple(new ITerm[] { arg0 }, annos);

			// Assert
			Assert.AreEqual(String.Empty, result.Name);
			Assert.AreEqual(new ITerm[] { arg0 }, result.SubTerms);
			Assert.AreEqual(annos, result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildPlaceholderTerm()
		{
			// Arrange
			var sut = CreateSUT();
			var template = sut.Int(0);

			// Act
			var result = sut.Placeholder(template);

			// Assert
			Assert.AreEqual(new ITerm[] { template }, result.SubTerms);
			Assert.IsEmpty(result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		[Test]
		public void CanBuildPlaceholderTermWithAnnotations()
		{
			// Arrange
			var sut = CreateSUT();
			var annos = BuildAnnos(sut);
			var template = sut.Int(0);

			// Act
			var result = sut.Placeholder(template, annos);

			// Assert
			Assert.AreEqual(new ITerm[] { template }, result.SubTerms);
			Assert.AreEqual(annos, result.Annotations);
			Assert.IsTrue(sut.IsBuiltByThisFactory(result));
		}

		/// <summary>
		/// Builds a collection of annotations.
		/// </summary>
		/// <param name="sut">The Subject Under Test.</param>
		/// <returns>The collection of annotations.</returns>
		private IReadOnlyCollection<ITerm> BuildAnnos(TermFactory sut)
		{
			var annos = new ITerm[]
			{
				sut.String("Annotation")
			};
			return annos;
		}
	}
}
