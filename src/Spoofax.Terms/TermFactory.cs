using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Spoofax.Terms
{
	/// <summary>
	/// Abstract term factory.
	/// </summary>
	public abstract class TermFactory
	{
	    /// <summary>
	    /// Gets an empty list of terms.
	    /// </summary>
	    /// <value>An empty list of terms.</value>
	    public static IReadOnlyList<ITerm> EmptyTermList { get; } = new ITerm[0];

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TermFactory"/> class.
		/// </summary>
		protected TermFactory()
		{
			
		}
		#endregion

		/// <summary>
		/// Determines whether the specified term was built by this factory.
		/// </summary>
		/// <param name="term">The term to check.</param>
		/// <returns><see langword="true"/> when the term was built by this factory;
		/// otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// <para>Any tree of terms may only consist of terms built by the same factory.</para>
		/// <para>When <paramref name="term"/> is <see langword="null"/>, this method returns
		/// <see langword="false"/>.</para>
		/// </remarks>
		[Pure]
		public abstract bool IsBuiltByThisFactory(ITerm term);

		/// <summary>
		/// Builds a new integer term.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <returns>The built term.</returns>
		public IIntTerm Int(int value)
		{
			return Int(value, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new integer term.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The built term.</returns>
		public abstract IIntTerm Int(int value, IReadOnlyCollection<ITerm> annotations);

		/// <summary>
		/// Builds a new floating-point term.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <returns>The floating-point term.</returns>
		public IRealTerm Real(float value)
		{
			return Real(value, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new floating-point term.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The floating-point term.</returns>
		public abstract IRealTerm Real(float value, IReadOnlyCollection<ITerm> annotations);

		/// <summary>
		/// Builds a new string term.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <returns>The string term.</returns>
		public IStringTerm String(string value)
        {
            #region Contract
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            #endregion

			return String(value, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new string term.
		/// </summary>
		/// <param name="value">The value of the term.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The string term.</returns>
		public abstract IStringTerm String(string value, IReadOnlyCollection<ITerm> annotations);

		/// <summary>
		/// Builds a new list term.
		/// </summary>
		/// <param name="head">The head of the list.</param>
		/// <param name="tail">The tail of the list.</param>
		/// <returns>The list term.</returns>
		public IListTerm ListConsNil(ITerm head, IListTerm tail)
		{
            #region Contract
            if (head == null)
                throw new ArgumentNullException(nameof(head));
            if (tail == null)
                throw new ArgumentNullException(nameof(tail));
            #endregion

            return ListConsNil(head, tail, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new list term.
		/// </summary>
		/// <param name="head">The head of the list.</param>
		/// <param name="tail">The tail of the list.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The list term.</returns>
		public abstract IListTerm ListConsNil(ITerm head, IListTerm tail, IReadOnlyCollection<ITerm> annotations);

		/// <summary>
		/// Builds a new list term.
		/// </summary>
		/// <param name="terms">The terms in the list.</param>
		/// <returns>The list term.</returns>
		public IListTerm List(IEnumerable<ITerm> terms)
        {
            #region Contract
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            #endregion

            return List(terms, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new list term.
		/// </summary>
		/// <param name="terms">The terms in the list.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The list term.</returns>
		public virtual IListTerm List(IEnumerable<ITerm> terms, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            IListTerm tail;
			var enumerable = terms as ITerm[] ?? terms.ToArray();
			if (enumerable.Any())
			{
				tail = EmptyList(TermFactory.EmptyTermList);
				tail = enumerable.Skip(1).Reverse().Aggregate(tail, (current, term) => ListConsNil(term, current));
				tail = ListConsNil(enumerable.First(), tail, annotations);
			}
			else
			{
				tail = EmptyList(annotations);
			}
			return tail;
		}

		/// <summary>
		/// Builds a new list term.
		/// </summary>
		/// <param name="terms">The terms in the list.</param>
		/// <returns>The list term.</returns>
		public IListTerm List(params ITerm[] terms)
        {
            #region Contract
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            #endregion

            return List(terms, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new empty list term.
		/// </summary>
		/// <returns>The list term.</returns>
		public IListTerm EmptyList()
		{
			return EmptyList(TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new empty list term.
		/// </summary>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The list term.</returns>
		public abstract IListTerm EmptyList(IReadOnlyCollection<ITerm> annotations);

		/// <summary>
		/// Builds a new tuple term.
		/// </summary>
		/// <param name="terms">The sub terms of the tuple.</param>
		/// <returns>The tuple term.</returns>
		public IConsTerm Tuple(params ITerm[] terms)
        {
            #region Contract
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            #endregion

            return Tuple(terms, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new tuple term.
		/// </summary>
		/// <param name="terms">The sub terms of the tuple.</param>
		/// <returns>The tuple term.</returns>
		public IConsTerm Tuple(IReadOnlyList<ITerm> terms)
        {
            #region Contract
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            #endregion

            return Tuple(terms, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new tuple term.
		/// </summary>
		/// <param name="terms">The sub terms of the tuple.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The tuple term.</returns>
		public IConsTerm Tuple(IReadOnlyList<ITerm> terms, IReadOnlyCollection<ITerm> annotations)
        {
            #region Contract
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            if (annotations == null)
                throw new ArgumentNullException(nameof(annotations));
            #endregion

            return Cons(System.String.Empty, terms, annotations);
		}

		/// <summary>
		/// Builds a new constructor term.
		/// </summary>
		/// <param name="name">The name of the constructor;
		/// or <see cref="System.String.Empty"/> for a tuple.</param>
		/// <param name="terms">The sub terms of the constructor.</param>
		/// <returns>The constructor term.</returns>
		public IConsTerm Cons(string name, params ITerm[] terms)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            #endregion

            return Cons(name, terms, TermFactory.EmptyTermList);
		}
		
		/// <summary>
		/// Builds a new constructor term.
		/// </summary>
		/// <param name="name">The name of the constructor;
		/// or <see cref="System.String.Empty"/> for a tuple.</param>
		/// <param name="terms">The sub terms of the constructor.</param>
		/// <returns>The constructor term.</returns>
		public IConsTerm Cons(string name, IReadOnlyList<ITerm> terms)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (terms == null)
                throw new ArgumentNullException(nameof(terms));
            #endregion

            return Cons(name, terms, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new constructor term.
		/// </summary>
		/// <param name="name">The name of the constructor;
		/// or <see cref="System.String.Empty"/> for a tuple.</param>
		/// <param name="terms">The sub terms of the constructor.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The constructor term.</returns>
		public abstract IConsTerm Cons(string name, IReadOnlyList<ITerm> terms, IReadOnlyCollection<ITerm> annotations);

		/// <summary>
		/// Builds a new placeholder term.
		/// </summary>
		/// <param name="template">The placeholder template.</param>
		/// <returns>The placeholder term.</returns>
		public IPlaceholderTerm Placeholder(ITerm template)
        {
            #region Contract
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            #endregion

            return Placeholder(template, TermFactory.EmptyTermList);
		}

		/// <summary>
		/// Builds a new placeholder term.
		/// </summary>
		/// <param name="template">The placeholder template.</param>
		/// <param name="annotations">A set of annotations for the term.</param>
		/// <returns>The placeholder term.</returns>
		public abstract IPlaceholderTerm Placeholder(ITerm template, IReadOnlyCollection<ITerm> annotations);
	}
}
