using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Yargon.Parser.Sdf
{
    [TestFixture]
    public class OrderedRangedSetTests
    {
        [Test]
        public void NewOrderedRangedSetHasNoElements()
        {
            // Act
            var sut = new OrderedRangedSet<int>();

            // Assert
            Assert.That(sut.IsEmpty, Is.True);
            Assert.That(sut.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddOneElement()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int v = 123;

            // Act
            sut.Add(v);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(1));
            Assert.That(sut, Is.EquivalentTo(new[] { v }));
            Assert.That(sut.Contains(v), Is.True);
            Assert.That(sut.Contains(v - 1), Is.False);
            Assert.That(sut.Contains(v + 1), Is.False);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(new[] { new Interval<int>(v, v + 1) }));
        }

        [Test]
        public void AddManyUnrelatedElements()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int[] arr = new[] { 1234, 123, 456, 5678, 789 };

            // Act
            foreach (int v in arr)
                sut.Add(v);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(arr.Length));
            Assert.That(sut, Is.EquivalentTo(arr.OrderBy(i => i)));
            foreach (int v in arr)
                Assert.That(sut.Contains(v), Is.True);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(arr.Select(i => new Interval<int>(i, i + 1))));
        }

        [Test]
        public void AddAdjacentElementAfterMerges()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int v1 = 123;
            int v2 = v1 + 1;

            // Act
            sut.Add(v1);
            sut.Add(v2);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(2));
            Assert.That(sut, Is.EquivalentTo(new[] { v1, v2 }));
            Assert.That(sut.Contains(v1), Is.True);
            Assert.That(sut.Contains(v2), Is.True);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(new[] { new Interval<int>(v1, v2 + 1) }));
        }

        [Test]
        public void AddAdjacentElementBeforeMerges()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int v1 = 123;
            int v2 = v1 + 1;

            // Act
            sut.Add(v2);
            sut.Add(v1);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(2));
            Assert.That(sut, Is.EquivalentTo(new[] { v1, v2 }));
            Assert.That(sut.Contains(v1), Is.True);
            Assert.That(sut.Contains(v2), Is.True);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(new[] { new Interval<int>(v1, v2 + 1) }));
        }


        [Test]
        public void AddAdjacentElementInBetweenMerges()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int v1 = 123;
            int v2 = v1 + 1;
            int v3 = v2 + 1;
            sut.Add(v1);
            sut.Add(v3);

            // Act
            sut.Add(v2);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(3));
            Assert.That(sut, Is.EquivalentTo(new[] { v1, v2, v3 }));
            Assert.That(sut.Contains(v1), Is.True);
            Assert.That(sut.Contains(v2), Is.True);
            Assert.That(sut.Contains(v3), Is.True);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(new[] { new Interval<int>(v1, v3 + 1) }));
        }

        [Test]
        public void RemoveStandAloneElement()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int[] arr = new[] { 1234, 123, 456, 5678, 789 };
            foreach (int v in arr) sut.Add(v);

            // Act
            sut.Remove(arr[2]);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(arr.Length - 1));
            Assert.That(sut, Is.EquivalentTo(arr.Where(i => i != arr[2]).OrderBy(i => i)));
            foreach (int v in arr.Where(i => i != arr[2]))
                Assert.That(sut.Contains(v), Is.True);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(arr.Where(i => i != arr[2]).Select(i => new Interval<int>(i, i + 1))));
        }

        [Test]
        public void RemoveElementAtStartOfRange()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int v1 = 123;
            int v2 = v1 + 1;
            int v3 = v2 + 1;
            sut.Add(v1);
            sut.Add(v2);
            sut.Add(v3);

            // Act
            sut.Remove(v1);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(2));
            Assert.That(sut, Is.EquivalentTo(new[] { v2, v3 }));
            Assert.That(sut.Contains(v1), Is.False);
            Assert.That(sut.Contains(v2), Is.True);
            Assert.That(sut.Contains(v3), Is.True);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(new[] { new Interval<int>(v2, v3 + 1) }));
        }

        [Test]
        public void RemoveElementAtEndOfRange()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int v1 = 123;
            int v2 = v1 + 1;
            int v3 = v2 + 1;
            sut.Add(v1);
            sut.Add(v2);
            sut.Add(v3);

            // Act
            sut.Remove(v3);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(2));
            Assert.That(sut, Is.EquivalentTo(new[] { v1, v2 }));
            Assert.That(sut.Contains(v1), Is.True);
            Assert.That(sut.Contains(v2), Is.True);
            Assert.That(sut.Contains(v3), Is.False);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(new[] { new Interval<int>(v1, v2 + 1) }));
        }

        [Test]
        public void RemoveElementInMiddleOfRange()
        {
            // Arrange
            var sut = new OrderedRangedSet<int>();
            int v1 = 123;
            int v2 = v1 + 1;
            int v3 = v2 + 1;
            sut.Add(v1);
            sut.Add(v2);
            sut.Add(v3);

            // Act
            sut.Remove(v2);

            // Assert
            Assert.That(sut.IsEmpty, Is.False);
            Assert.That(sut.Count, Is.EqualTo(2));
            Assert.That(sut, Is.EquivalentTo(new[] { v1, v3 }));
            Assert.That(sut.Contains(v1), Is.True);
            Assert.That(sut.Contains(v2), Is.False);
            Assert.That(sut.Contains(v3), Is.True);
            Assert.That(sut.GetRanges(), Is.EquivalentTo(new[] { new Interval<int>(v1, v1 + 1), new Interval<int>(v3, v3 + 1) }));
        }
    }
}
