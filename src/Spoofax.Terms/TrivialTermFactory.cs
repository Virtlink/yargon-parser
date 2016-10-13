using System;
using System.Collections.Generic;

namespace Spoofax.Terms
{
	/// <summary>
	/// The trivial term factory has a trivial implementation for the terms
	/// and the factory that doesn't do any optimizations such as term sharing.
	/// </summary>
	public sealed partial class TrivialTermFactory : TermFactory
	{
		/// <inheritdoc />
		public override bool IsBuiltByThisFactory(ITerm term)
		{
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion
            
            return term is Term;
		}

		/// <inheritdoc />
		public override IIntTerm Int(int value, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return new IntTerm(value, annotations);
		}

		/// <inheritdoc />
		public override IRealTerm Real(float value, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return new RealTerm(value, annotations);
		}

		/// <inheritdoc />
		public override IStringTerm String(string value, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return new StringTerm(value, annotations);
		}

		/// <inheritdoc />
		public override IListTerm ListConsNil(ITerm head, IListTerm tail, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (head == null)
                throw new ArgumentNullException(nameof(head));
            if (tail == null)
                throw new ArgumentNullException(nameof(tail));
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return new ListTerm(head, tail, annotations);
		}

		/// <inheritdoc />
		public override IListTerm EmptyList(IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return new ListTerm(annotations);
		}

		/// <inheritdoc />
		public override IConsTerm Cons(string name, IReadOnlyList<ITerm> terms, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return new ConsTerm(name, terms, annotations);
		}

		/// <inheritdoc />
		public override IPlaceholderTerm Placeholder(ITerm template, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return new PlaceholderTerm(template, annotations);
		}
	}
}
