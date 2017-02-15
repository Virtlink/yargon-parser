using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yargon.Parsing
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExt
    {
        /// <summary>
        /// Returns the elements of the first sequence, or the elements of the second sequence when the first
        /// sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="source">The first sequence.</param>
        /// <param name="otherwise">The second sequence, only to be returned when the first sequence is empty.</param>
        /// <returns>The resulting enumerable.</returns>
        public static IEnumerable<T> OrIfEmpty<T>(this IEnumerable<T> source, IEnumerable<T> otherwise)
        {
            #region Contract
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (otherwise == null)
                throw new ArgumentNullException(nameof(otherwise));
            #endregion

            return EnumerableExt.OrIfEmptyEnumerator(source, otherwise);
        }

        /// <summary>
        /// Enumerator for the <see cref="OrIfEmpty{T}"/> method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The first sequence.</param>
        /// <param name="otherwise">The second sequence.</param>
        /// <returns>The resulting sequence.</returns>
        private static IEnumerable<T> OrIfEmptyEnumerator<T>(IEnumerable<T> source, IEnumerable<T> otherwise)
        {
            #region Contract
            Debug.Assert(source != null);
            Debug.Assert(otherwise != null);
            #endregion

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    do
                    {
                        yield return enumerator.Current;
                    } while (enumerator.MoveNext());
                }
                else
                {
                    foreach (var e in otherwise)
                    {
                        yield return e;
                    }
                }
            }
        }
    }
}
