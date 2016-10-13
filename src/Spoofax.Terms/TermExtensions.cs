using System;
using System.Diagnostics.Contracts;

namespace Spoofax.Terms
{
	/// <summary>
	/// Extension methods for terms.
	/// </summary>
	public static class TermExtensions
	{
		/// <summary>
		/// Converts the term to an <see cref="Int32"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="InvalidCastException">
		/// The term could not be converted.
		/// </exception>
		public static Int32 ToInt32(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
			#endregion

			var result = TermExtensions.AsInt32(term);
			if (result == null) throw new InvalidCastException($"Can't cast term {term} to Int32.");
			return (Int32)result;
		}

		/// <summary>
		/// Attempts to convert the term to an <see cref="Int32"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns>The resulting value;
		/// or <see langword="null"/> when it could not be converted.</returns>
		[Pure]
		public static Int32? AsInt32(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            var cterm = term as IIntTerm;
			return cterm?.Value;
		}

		/// <summary>
		/// Determines whether the term can be converted to an <see cref="Int32"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns><see langword="true"/> when the term can be converted;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsInt32(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            return term is IIntTerm;
		}


		/// <summary>
		/// Converts the term to an <see cref="Single"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="InvalidCastException">
		/// The term could not be converted.
		/// </exception>
		public static Single ToSingle(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            var result = TermExtensions.AsSingle(term);
			if (result == null) throw new InvalidCastException($"Can't cast term {term} to Single.");
			return (Single)result;
		}

		/// <summary>
		/// Attempts to convert the term to an <see cref="Single"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns>The resulting value;
		/// or <see langword="null"/> when it could not be converted.</returns>
		[Pure]
		public static Single? AsSingle(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            var cterm = term as IRealTerm;
			return cterm?.Value;	// TODO: Use C# 6 null coalescing operator.
		}

		/// <summary>
		/// Determines whether the term can be converted to a <see cref="Single"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns><see langword="true"/> when the term can be converted;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsSingle(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            return term is IRealTerm;
		}



		/// <summary>
		/// Converts the term to a <see cref="String"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="InvalidCastException">
		/// The term could not be converted.
		/// </exception>
		public static String ToString(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            var result = TermExtensions.AsString(term);
			if (result == null) throw new InvalidCastException($"Can't cast term {term} to String.");
			return result;
		}

		/// <summary>
		/// Attempts to convert the term to a <see cref="String"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns>The resulting value;
		/// or <see langword="null"/> when it could not be converted.</returns>
		[Pure]
		public static String AsString(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            var cterm = term as IStringTerm;
			return cterm?.Value;
		}

		/// <summary>
		/// Determines whether the term can be converted to a <see cref="String"/>.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <returns><see langword="true"/> when the term can be converted;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsString(this ITerm term)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            #endregion

            return term is IStringTerm;
		}



		/// <summary>
		/// Converts the term to a constructor with the specified name and arity.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <param name="name">The expected name.</param>
		/// <param name="arity">The expected arity.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="InvalidCastException">
		/// The term could not be converted.
		/// </exception>
		public static IConsTerm ToCons(this ITerm term, string name, int arity)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (arity < 0)
                throw new ArgumentOutOfRangeException(nameof(arity));
            #endregion

            var result = TermExtensions.AsCons(term, name, arity);
			if (result == null) throw new InvalidCastException($"Can't cast term {term} to {name}`{arity}.");
			return result;
		}

		/// <summary>
		/// Attempts to convert the term to a constructor with the specified name and arity.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <param name="name">The expected name.</param>
		/// <param name="arity">The expected arity.</param>
		/// <returns>The resulting constructor;
		/// or <see langword="null"/> when it could not be converted.</returns>
		[Pure]
		public static IConsTerm AsCons(this ITerm term, string name, int arity)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (arity < 0)
                throw new ArgumentOutOfRangeException(nameof(arity));
            #endregion

			var cterm = term as IConsTerm;
			if (cterm != null && cterm.Name == name && cterm.SubTerms.Count == arity)
				return cterm;
			else
				return null;
		}

		/// <summary>
		/// Determines whether the term can be converted to a constructor with the specified name and arity.
		/// </summary>
		/// <param name="term">The term to convert.</param>
		/// <param name="name">The expected name.</param>
		/// <param name="arity">The expected arity.</param>
		/// <returns><see langword="true"/> when the term can be converted;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsCons(this ITerm term, string name, int arity)
        {
            #region Contract
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (arity < 0)
                throw new ArgumentOutOfRangeException(nameof(arity));
            #endregion

            var cterm = term as IConsTerm;
			return cterm != null && cterm.Name == name && cterm.SubTerms.Count == arity;
		}
	}
}
