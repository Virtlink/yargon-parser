using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Spoofax.Terms
{
	partial class TrivialTermFactoryTests
	{
		[TestFixture]
		public sealed class ConsTermTests : TestBase
		{
			#region SUT
			public TrivialTermFactory.ConsTerm CreateSUT(string name, IEnumerable<ITerm> terms)
			{
				return (TrivialTermFactory.ConsTerm)Factory.Cons(name, terms.ToArray());
			}

			public sealed class ConsTerm_IConsTerm : IConsTermTests
			{
				public override IConsTerm CreateSUT(string name, IEnumerable<ITerm> terms)
				{
					return new ConsTermTests().CreateSUT(name, terms);
				}
			}
			#endregion
		}
	}
}
